using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull;

public class FireLowTideProjectile : CharacterOperation
{
	public enum DirectionType
	{
		RotationOfFirePosition,
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private Projectile _projectile;

	[Header("Special Setting")]
	[SerializeField]
	private Projectile[] _projectilesToCount;

	[SerializeField]
	private float[] _scalesByCount;

	[Space]
	[SerializeField]
	private CustomFloat _speedMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _damageMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	[Space]
	private Transform _fireTransform;

	[SerializeField]
	private bool _group;

	[SerializeField]
	private bool _flipXByOwnerDirection;

	[SerializeField]
	private bool _flipYByOwnerDirection;

	[Space]
	[SerializeField]
	private DirectionType _directionType;

	[SerializeField]
	private CustomAngle.Reorderable _directions;

	private IAttackDamage _attackDamage;

	public CustomFloat scale => _scale;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_projectile = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		if ((Object)(object)_fireTransform == (Object)null)
		{
			_fireTransform = ((Component)this).transform;
		}
	}

	private float GetProjectileScale()
	{
		int num = 0;
		Projectile[] projectilesToCount = _projectilesToCount;
		foreach (Projectile projectile in projectilesToCount)
		{
			num += projectile.reusable.spawnedCount;
		}
		int num2 = Mathf.Clamp(num, 0, _scalesByCount.Length - 1);
		return _scalesByCount[num2];
	}

	public override void Run(Character owner)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		CustomAngle[] values = _directions.values;
		float attackDamage = _attackDamage.amount * _damageMultiplier.value;
		bool flipX = false;
		bool flipY = false;
		HitHistoryManager hitHistoryManager = (_group ? new HitHistoryManager(15) : null);
		float projectileScale = GetProjectileScale();
		for (int i = 0; i < values.Length; i++)
		{
			float num;
			switch (_directionType)
			{
			case DirectionType.RotationOfFirePosition:
			{
				Quaternion rotation = _fireTransform.rotation;
				num = ((Quaternion)(ref rotation)).eulerAngles.z + values[i].value;
				if (_fireTransform.lossyScale.x < 0f)
				{
					num = (180f - num) % 360f;
				}
				break;
			}
			case DirectionType.OwnerDirection:
			{
				num = values[i].value;
				bool flag = owner.lookingDirection == Character.LookingDirection.Left || _fireTransform.lossyScale.x < 0f;
				flipX = _flipXByOwnerDirection && flag;
				flipY = _flipYByOwnerDirection && flag;
				num = (flag ? ((180f - num) % 360f) : num);
				break;
			}
			default:
				num = values[i].value;
				break;
			}
			Projectile component = ((Component)_projectile.reusable.Spawn(_fireTransform.position, true)).GetComponent<Projectile>();
			((Component)component).transform.localScale = Vector3.one * _scale.value * projectileScale;
			component.Fire(owner, attackDamage, num, flipX, flipY, _speedMultiplier.value, _group ? hitHistoryManager : null);
		}
	}
}
