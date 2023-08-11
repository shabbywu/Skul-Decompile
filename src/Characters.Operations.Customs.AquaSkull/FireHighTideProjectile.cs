using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull;

public class FireHighTideProjectile : CharacterOperation
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
	[Tooltip("발사 순서 * _fireInterval 만큼 대기한 후 발사됨")]
	private float _fireInterval;

	[SerializeField]
	private float[] _countsByCount;

	[Space]
	[SerializeField]
	private CustomFloat _speedMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _damageMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[Space]
	[SerializeField]
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
	private CustomAngle _direction;

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

	private float GetPorjectileCount()
	{
		int num = 0;
		Projectile[] projectilesToCount = _projectilesToCount;
		foreach (Projectile projectile in projectilesToCount)
		{
			num += projectile.reusable.spawnedCount;
		}
		int num2 = Mathf.Clamp(num, 0, _countsByCount.Length - 1);
		return _countsByCount[num2];
	}

	public override void Run(Character owner)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		float porjectileCount = GetPorjectileCount();
		float attackDamage = _attackDamage.amount * _damageMultiplier.value;
		bool flipX = false;
		bool flipY = false;
		HitHistoryManager hitHistoryManager = (_group ? new HitHistoryManager(15) : null);
		for (int i = 0; (float)i < porjectileCount; i++)
		{
			float num;
			switch (_directionType)
			{
			case DirectionType.RotationOfFirePosition:
			{
				Quaternion rotation = _fireTransform.rotation;
				num = ((Quaternion)(ref rotation)).eulerAngles.z + _direction.value;
				if (_fireTransform.lossyScale.x < 0f)
				{
					num = (180f - num) % 360f;
				}
				break;
			}
			case DirectionType.OwnerDirection:
			{
				num = _direction.value;
				bool flag = owner.lookingDirection == Character.LookingDirection.Left || _fireTransform.lossyScale.x < 0f;
				flipX = _flipXByOwnerDirection && flag;
				flipY = _flipYByOwnerDirection && flag;
				num = (flag ? ((180f - num) % 360f) : num);
				break;
			}
			default:
				num = _direction.value;
				break;
			}
			Projectile component = ((Component)_projectile.reusable.Spawn(_fireTransform.position, true)).GetComponent<Projectile>();
			((Component)component).transform.localScale = Vector3.one * _scale.value;
			component.Fire(owner, attackDamage, num, flipX, flipY, _speedMultiplier.value, _group ? hitHistoryManager : null, _fireInterval * (float)i);
		}
	}
}
