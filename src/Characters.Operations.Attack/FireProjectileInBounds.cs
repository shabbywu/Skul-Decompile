using Characters.Projectiles;
using FX;
using UnityEngine;

namespace Characters.Operations.Attack;

public class FireProjectileInBounds : CharacterOperation
{
	public enum DirectionType
	{
		OwnerDirection,
		Constant,
		RotationOfReferenceTransform
	}

	[SerializeField]
	private Projectile _projectile;

	[Space]
	[SerializeField]
	private CustomFloat _speedMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _damageMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[Space]
	[SerializeField]
	private Collider2D _area;

	[SerializeField]
	private EffectInfo _spawnEffect;

	[SerializeField]
	[Space]
	private bool _flipXByOwnerDirection;

	[SerializeField]
	private bool _flipYByOwnerDirection;

	[Space]
	[SerializeField]
	private DirectionType _directionType;

	[SerializeField]
	[Tooltip("DirectionType을 ReferenceTransform으로 설정했을 경우 이 Transform을 참조합니다.")]
	private Transform _rotationReference;

	[SerializeField]
	private CustomAngle.Reorderable _directions = new CustomAngle.Reorderable(new CustomAngle(0f));

	private IAttackDamage _attackDamage;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_projectile = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		if ((Object)(object)_rotationReference == (Object)null)
		{
			_rotationReference = ((Component)this).transform;
		}
	}

	public override void Run(Character owner)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		CustomAngle[] values = _directions.values;
		bool flipX = false;
		bool flipY = false;
		for (int i = 0; i < values.Length; i++)
		{
			float num;
			switch (_directionType)
			{
			case DirectionType.RotationOfReferenceTransform:
			{
				Quaternion rotation = _rotationReference.rotation;
				num = ((Quaternion)(ref rotation)).eulerAngles.z + values[i].value;
				if (_rotationReference.lossyScale.x < 0f)
				{
					num = (180f - num) % 360f;
				}
				break;
			}
			case DirectionType.OwnerDirection:
			{
				num = values[i].value;
				bool flag = owner.lookingDirection == Character.LookingDirection.Left || ((Component)_area).transform.lossyScale.x < 0f;
				flipX = _flipXByOwnerDirection && flag;
				flipY = _flipYByOwnerDirection && flag;
				num = (flag ? ((180f - num) % 360f) : num);
				break;
			}
			default:
				num = values[i].value;
				break;
			}
			Projectile component = ((Component)_projectile.reusable.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_area.bounds)), true)).GetComponent<Projectile>();
			((Component)component).transform.localScale = Vector3.one * _scale.value;
			component.Fire(owner, _attackDamage.amount * _damageMultiplier.value, num, flipX, flipY, _speedMultiplier.value);
			_spawnEffect.Spawn(((Component)component).transform.position, owner);
		}
	}
}
