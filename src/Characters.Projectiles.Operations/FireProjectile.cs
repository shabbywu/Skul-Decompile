using Characters.Utils;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class FireProjectile : Operation
{
	public enum DirectionType
	{
		RotationOfFirePosition,
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private CustomFloat _speedMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _damageMultiplier = new CustomFloat(1f);

	[SerializeField]
	private Transform _fireTransform;

	[SerializeField]
	private bool _group;

	[SerializeField]
	private DirectionType _directionType;

	[SerializeField]
	private Reorderable _directions;

	public CustomAngle[] directions => ((ReorderableArray<CustomAngle>)(object)_directions).values;

	private void Awake()
	{
		if ((Object)(object)_fireTransform == (Object)null)
		{
			_fireTransform = ((Component)this).transform;
		}
	}

	private void OnDestroy()
	{
		_projectile = null;
	}

	public override void Run(IProjectile projectile)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		Character owner = projectile.owner;
		CustomAngle[] values = ((ReorderableArray<CustomAngle>)(object)_directions).values;
		float attackDamage = projectile.baseDamage * _damageMultiplier.value;
		HitHistoryManager hitHistoryManager = (_group ? new HitHistoryManager(15) : null);
		for (int i = 0; i < values.Length; i++)
		{
			float direction;
			bool flipX;
			switch (_directionType)
			{
			case DirectionType.RotationOfFirePosition:
			{
				Quaternion rotation = _fireTransform.rotation;
				direction = ((Quaternion)(ref rotation)).eulerAngles.z + values[i].value;
				flipX = _fireTransform.lossyScale.x < 0f;
				break;
			}
			case DirectionType.OwnerDirection:
				direction = values[i].value;
				flipX = owner.lookingDirection == Character.LookingDirection.Left;
				break;
			default:
				direction = values[i].value;
				flipX = false;
				break;
			}
			((Component)_projectile.reusable.Spawn(_fireTransform.position, true)).GetComponent<Projectile>().Fire(owner, attackDamage, direction, flipX, flipY: false, _speedMultiplier.value, _group ? hitHistoryManager : null);
		}
	}
}
