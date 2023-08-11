using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Gear.Quintessences.Constraints;

public sealed class EnemyWithinRangeConstraint : Constraint
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
		UsingCollider val = default(UsingCollider);
		((UsingCollider)(ref val))._002Ector(_searchRange);
		try
		{
			return _enemyOverlapper.OverlapCollider(_searchRange).results.Count > 0;
		}
		finally
		{
			((IDisposable)(UsingCollider)(ref val)).Dispose();
		}
	}
}