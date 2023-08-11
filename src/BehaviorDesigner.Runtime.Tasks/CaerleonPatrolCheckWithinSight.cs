using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Allows multiple action tasks to be added to a single node.")]
[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
public sealed class CaerleonPatrolCheckWithinSight : Conditional
{
	private static readonly NonAllocCaster _reusableCaster;

	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCollider _range;

	[SerializeField]
	private SharedCharacter _target;

	private TargetLayer _targetLayer;

	private NonAllocOverlapper _overlapper;

	private Character _ownerValue;

	private Collider2D _rangeValue;

	[SerializeField]
	private LayerMask _blockLayerMask = LayerMask.op_Implicit(8);

	static CaerleonPatrolCheckWithinSight()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_reusableCaster = new NonAllocCaster(15);
	}

	public override void OnAwake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);
		_overlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _reusableCaster.contactFilter)).SetLayerMask(_blockLayerMask);
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_rangeValue = ((SharedVariable<Collider2D>)_range).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Character character = FindTarget();
		if (!((Object)(object)character == (Object)null))
		{
			float num = Mathf.Abs(((Component)character).transform.position.x - ((Component)_ownerValue).transform.position.x);
			Vector3 position = ((Component)_ownerValue).transform.position;
			if (_reusableCaster.RayCast(Vector2.op_Implicit(position), (_ownerValue.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right, num).results.Count <= 0)
			{
				if (!character.stealth.value)
				{
					((SharedVariable)_target).SetValue((object)character);
					return (TaskStatus)2;
				}
				return (TaskStatus)1;
			}
			return (TaskStatus)1;
		}
		return (TaskStatus)1;
	}

	private Character FindTarget()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_ownerValue).gameObject));
		return TargetFinder.FindClosestTarget(_overlapper, _rangeValue, (Collider2D)(object)_ownerValue.collider);
	}
}
