using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Allows multiple action tasks to be added to a single node.")]
[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
public sealed class CheckWithinSight : Conditional
{
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

	static CheckWithinSight()
	{
	}

	public override void OnAwake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		_targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);
		_overlapper = new NonAllocOverlapper(31);
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_rangeValue = ((SharedVariable<Collider2D>)_range).Value;
	}

	public override TaskStatus OnUpdate()
	{
		Character character = FindTarget();
		if (!((Object)(object)character == (Object)null))
		{
			((SharedVariable)_target).SetValue((object)character);
			return (TaskStatus)2;
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
