using System.Collections.Generic;
using Characters.Usables;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs;

public class CanSummonGrassConstraint : Constraint
{
	[SerializeField]
	private BoxCollider2D _findRange;

	private readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(1);

	public override bool Pass()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(4096));
		List<Liquid> components = _sharedOverlapper.OverlapCollider((Collider2D)(object)_findRange).GetComponents<Liquid>(true);
		Debug.Log((object)components.Count);
		if (components.Count >= 1)
		{
			return false;
		}
		return true;
	}
}
