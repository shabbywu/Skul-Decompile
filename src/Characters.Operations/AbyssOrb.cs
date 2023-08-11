using Characters.Actions;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations;

public class AbyssOrb : CharacterOperation
{
	public enum DirectionType
	{
		RotationOfFirePosition,
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private ChargeAction _chargeAction;

	[SerializeField]
	[Space]
	private float _scaleMin = 0.2f;

	[SerializeField]
	private float _scaleMax = 1f;

	[SerializeField]
	private float _damageMultiplierMin = 0.2f;

	[SerializeField]
	private float _damageMultiplierMax = 1f;

	[SerializeField]
	[Space]
	private Projectile _incompleteProjectile;

	[SerializeField]
	private Projectile _completeProjectile;

	[SerializeField]
	[Space]
	private Transform _fireTransform;

	[SerializeField]
	private bool _flipXByOwnerDirection;

	[SerializeField]
	private bool _flipYByOwnerDirection;

	[SerializeField]
	[Space]
	private DirectionType _directionType;

	[SerializeField]
	private Reorderable _directions;

	private IAttackDamage _attackDamage;

	public CustomAngle[] directions => ((ReorderableArray<CustomAngle>)(object)_directions).values;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		if ((Object)(object)_fireTransform == (Object)null)
		{
			_fireTransform = ((Component)this).transform;
		}
	}

	public override void Run(Character owner)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		float num = (_scaleMax - _scaleMin) * _chargeAction.chargedPercent + _scaleMin;
		float num2 = (_damageMultiplierMax - _damageMultiplierMin) * _chargeAction.chargedPercent + _damageMultiplierMin;
		Projectile projectile = ((_chargeAction.chargedPercent < 1f) ? _incompleteProjectile : _completeProjectile);
		CustomAngle[] values = ((ReorderableArray<CustomAngle>)(object)_directions).values;
		float attackDamage = _attackDamage.amount * num2;
		bool flipX = false;
		bool flipY = false;
		for (int i = 0; i < values.Length; i++)
		{
			float num3;
			switch (_directionType)
			{
			case DirectionType.RotationOfFirePosition:
			{
				Quaternion rotation = _fireTransform.rotation;
				num3 = ((Quaternion)(ref rotation)).eulerAngles.z + values[i].value;
				if (_fireTransform.lossyScale.x < 0f)
				{
					num3 = (180f - num3) % 360f;
				}
				break;
			}
			case DirectionType.OwnerDirection:
			{
				num3 = values[i].value;
				bool flag = owner.lookingDirection == Character.LookingDirection.Left || _fireTransform.lossyScale.x < 0f;
				flipX = _flipXByOwnerDirection && flag;
				flipY = _flipYByOwnerDirection && flag;
				num3 = (flag ? ((180f - num3) % 360f) : num3);
				break;
			}
			default:
				num3 = values[i].value;
				break;
			}
			Projectile component = ((Component)projectile.reusable.Spawn(_fireTransform.position, true)).GetComponent<Projectile>();
			((Component)component).transform.localScale = Vector3.one * num;
			component.Fire(owner, attackDamage, num3, flipX, flipY);
		}
	}
}
