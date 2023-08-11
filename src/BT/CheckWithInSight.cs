using BT.SharedValues;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BT;

public sealed class CheckWithInSight : Node
{
	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private Collider2D _range;

	private Character _owner;

	private static readonly NonAllocOverlapper _overlapper;

	static CheckWithInSight()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(31);
	}

	protected override NodeState UpdateDeltatime(Context context)
	{
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = context.Get<Character>(Key.OwnerCharacter);
		}
		Character character = FindTarget();
		if ((Object)(object)character == (Object)null)
		{
			return NodeState.Fail;
		}
		context.Set(Key.Target, new SharedValue<Character>(character));
		return NodeState.Success;
	}

	private Character FindTarget()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_owner).gameObject));
		return TargetFinder.FindClosestTarget(_overlapper, _range, (Collider2D)(object)_owner.collider);
	}
}
