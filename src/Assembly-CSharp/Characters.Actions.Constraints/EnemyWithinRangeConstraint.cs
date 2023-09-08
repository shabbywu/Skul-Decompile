using PhysicsUtils;
using UnityEngine;

namespace Characters.Actions.Constraints;

public class EnemyWithinRangeConstraint : Constraint
{
	private static readonly NonAllocOverlapper _enemyOverlapper;

	[SerializeField]
	private Collider2D _searchRange;

	static EnemyWithinRangeConstraint()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_enemyOverlapper = new NonAllocOverlapper(1);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	public override bool Pass()
	{
		using (new UsingCollider(_searchRange))
		{
			return _enemyOverlapper.OverlapCollider(_searchRange).results.Count > 0;
		}
	}
}
