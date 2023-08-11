using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Actions.Constraints;

public class EnemyStatusWithinRangeConstraint : Constraint
{
	private static readonly NonAllocOverlapper _enemyOverlapper;

	[SerializeField]
	private CharacterStatus.Kind _status;

	[SerializeField]
	private Collider2D _searchRange;

	static EnemyStatusWithinRangeConstraint()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_enemyOverlapper = new NonAllocOverlapper(32);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	public override bool Pass()
	{
		UsingCollider val = default(UsingCollider);
		((UsingCollider)(ref val))._002Ector(_searchRange);
		try
		{
			ReadonlyBoundedList<Collider2D> results = _enemyOverlapper.OverlapCollider(_searchRange).results;
			for (int i = 0; i < results.Count; i++)
			{
				Target component = ((Component)results[i]).GetComponent<Target>();
				if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null) && !((Object)(object)component.character.status == (Object)null) && component.character.status.freezed && component.character.status.IsApplying(_status))
				{
					return true;
				}
			}
			return false;
		}
		finally
		{
			((IDisposable)(UsingCollider)(ref val)).Dispose();
		}
	}
}
