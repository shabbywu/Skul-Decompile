using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations.Attack;

public class FireBouncyProjectile : CharacterOperation
{
	public enum DirectionType
	{
		RotationOfFirePosition,
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private BouncyProjectile _projectile;

	[Space]
	[SerializeField]
	private CustomFloat _speedMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _damageMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	[Space]
	private bool _group;

	[SerializeField]
	private bool _flipXByOwnerDirection;

	[FormerlySerializedAs("_flipY")]
	[SerializeField]
	private bool _flipYByOwnerDirection;

	[SerializeField]
	[Space]
	private DirectionType _directionType;

	[SerializeField]
	private Reorderable _directions;

	[SerializeField]
	[Space]
	private Transform _fireTransform;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private Vector2[] _positionsToInterpolate;

	private Vector2 _firePosition;

	private IAttackDamage _attackDamage;

	public CustomFloat scale => _scale;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_firePosition = Vector2.op_Implicit(_fireTransform.localPosition);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_projectile = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
	}

	private bool FindFirePosition(Vector2 offset)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		ContactFilter2D val = default(ContactFilter2D);
		((ContactFilter2D)(ref val)).SetLayerMask(Layers.groundMask);
		Collider2D[] array = (Collider2D[])(object)new Collider2D[1];
		_fireTransform.localPosition = Vector2.op_Implicit(_firePosition + offset);
		Physics2D.SyncTransforms();
		return Physics2D.OverlapCollider(_collider, val, array) == 0;
	}

	public override void Run(Character owner)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _positionsToInterpolate.Length && !FindFirePosition(_positionsToInterpolate[i]); i++)
		{
		}
		CustomAngle[] values = ((ReorderableArray<CustomAngle>)(object)_directions).values;
		bool flipX = false;
		bool flipY = false;
		HitHistoryManager hitHistoryManager = (_group ? new HitHistoryManager(15) : null);
		for (int j = 0; j < values.Length; j++)
		{
			float num;
			switch (_directionType)
			{
			case DirectionType.RotationOfFirePosition:
			{
				Quaternion rotation = _fireTransform.rotation;
				num = ((Quaternion)(ref rotation)).eulerAngles.z + values[j].value;
				if (_fireTransform.lossyScale.x < 0f)
				{
					num = (180f - num) % 360f;
				}
				break;
			}
			case DirectionType.OwnerDirection:
			{
				num = values[j].value;
				bool flag = owner.lookingDirection == Character.LookingDirection.Left || _fireTransform.lossyScale.x < 0f;
				flipX = _flipXByOwnerDirection && flag;
				flipY = _flipYByOwnerDirection && flag;
				num = (flag ? ((180f - num) % 360f) : num);
				break;
			}
			default:
				num = values[j].value;
				break;
			}
			BouncyProjectile component = ((Component)_projectile.reusable.Spawn(_fireTransform.position, true)).GetComponent<BouncyProjectile>();
			((Component)component).transform.localScale = Vector3.one * _scale.value;
			component.Fire(owner, _attackDamage.amount * _damageMultiplier.value, num, flipX, flipY, _speedMultiplier.value, _group ? hitHistoryManager : null);
		}
	}
}
