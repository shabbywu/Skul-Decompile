using Characters.Operations.Fx;
using Characters.Projectiles;
using FX;
using Level;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class WeaponMaster : CharacterOperation
{
	public enum DirectionType
	{
		OwnerDirection,
		Constant,
		RotationOfReferenceTransform
	}

	[SerializeField]
	private WeaponMasterProjectile _projectile;

	[Space]
	[SerializeField]
	private CustomFloat _speedMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _damageMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	[Space]
	private Collider2D _area;

	[SerializeField]
	private EffectInfo _spawnEffect;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(PlaySound))]
	private PlaySound _spawnSound;

	[Space]
	[SerializeField]
	private bool _flipXByOwnerDirection;

	[SerializeField]
	private bool _flipYByOwnerDirection;

	[Space]
	[SerializeField]
	private int _minCount = 1;

	[SerializeField]
	private int _maxEnemyCount = 15;

	[SerializeField]
	private float _multiplier;

	[SerializeField]
	private DirectionType _directionType;

	[SerializeField]
	[Tooltip("DirectionType을 ReferenceTransform으로 설정했을 경우 이 Transform을 참조합니다.")]
	private Transform _rotationReference;

	[SerializeField]
	private CustomAngle _direction;

	private IAttackDamage _attackDamage;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		if ((Object)(object)_rotationReference == (Object)null)
		{
			_rotationReference = ((Component)this).transform;
		}
		_spawnSound.Initialize();
	}

	public override void Run(Character owner)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)_minCount + _multiplier * (float)math.min(_maxEnemyCount, Map.Instance.waveContainer.GetAllSpawnedEnemiesCount());
		bool flipX = false;
		bool flipY = false;
		for (int i = 0; (float)i < num; i++)
		{
			float num2;
			switch (_directionType)
			{
			case DirectionType.RotationOfReferenceTransform:
			{
				Quaternion rotation = _rotationReference.rotation;
				num2 = ((Quaternion)(ref rotation)).eulerAngles.z + _direction.value;
				if (_rotationReference.lossyScale.x < 0f)
				{
					num2 = (180f - num2) % 360f;
				}
				break;
			}
			case DirectionType.OwnerDirection:
			{
				num2 = _direction.value;
				bool flag = owner.lookingDirection == Character.LookingDirection.Left || ((Component)_area).transform.lossyScale.x < 0f;
				flipX = _flipXByOwnerDirection && flag;
				flipY = _flipYByOwnerDirection && flag;
				num2 = (flag ? ((180f - num2) % 360f) : num2);
				break;
			}
			default:
				num2 = _direction.value;
				break;
			}
			IProjectile component = ((Component)_projectile.reusable.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_area.bounds)), true)).GetComponent<IProjectile>();
			component.transform.localScale = Vector3.one * _scale.value;
			component.Fire(owner, _attackDamage.amount * _damageMultiplier.value, num2, flipX, flipY, _speedMultiplier.value);
			_spawnEffect.Spawn(component.transform.position);
			_spawnSound.Run(owner);
		}
	}
}
