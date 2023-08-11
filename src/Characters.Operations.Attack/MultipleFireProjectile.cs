using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Attack;

public class MultipleFireProjectile : CharacterOperation
{
	public enum DirectionType
	{
		RotationOfFirePosition,
		RotationOfCenter,
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private Transform _fireTransformsParent;

	[SerializeField]
	private bool _group;

	[SerializeField]
	private DirectionType _directionType;

	[SerializeField]
	private Reorderable _directions;

	[SerializeField]
	private IAttackDamage _attackDamage;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_projectile = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
	}

	public override void Run(Character owner)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		CustomAngle[] values = ((ReorderableArray<CustomAngle>)(object)_directions).values;
		HitHistoryManager hitHistoryManager = (_group ? new HitHistoryManager(15) : null);
		foreach (Transform item in _fireTransformsParent)
		{
			Transform val = item;
			if (_directionType == DirectionType.RotationOfFirePosition)
			{
				for (int i = 0; i < values.Length; i++)
				{
					Projectile component = ((Component)_projectile.reusable.Spawn(val.position, true)).GetComponent<Projectile>();
					float amount = _attackDamage.amount;
					Quaternion localRotation = val.localRotation;
					component.Fire(owner, amount, ((Quaternion)(ref localRotation)).eulerAngles.z + values[i].value, val.lossyScale.x < 0f);
				}
			}
			else if (_directionType == DirectionType.OwnerDirection)
			{
				for (int j = 0; j < values.Length; j++)
				{
					((Component)_projectile.reusable.Spawn(val.position, true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, values[j].value, owner.lookingDirection == Character.LookingDirection.Left);
				}
			}
			else
			{
				for (int k = 0; k < values.Length; k++)
				{
					((Component)_projectile.reusable.Spawn(val.position, true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, values[k].value, flipX: false, flipY: false, 1f, _group ? hitHistoryManager : null);
				}
			}
		}
	}
}
