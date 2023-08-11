using System;
using Characters.AI.Hardmode.Chapter3;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.FindOptions.Custom;

[Serializable]
public class FarFromTotem : ICondition
{
	[SerializeField]
	private BoxCollider2D _searchRange;

	private static readonly NonAllocOverlapper _overlapper;

	static FarFromTotem()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	public bool Satisfied(Character character)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		NonAllocOverlapper overlapper = _overlapper;
		Vector2 val = Vector2.op_Implicit(((Component)character).transform.position);
		Bounds bounds = ((Collider2D)_searchRange).bounds;
		overlapper.OverlapBox(val, Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f);
		foreach (Collider2D result in _overlapper.results)
		{
			if ((Object)(object)((Component)result).GetComponent<Totem>() != (Object)null)
			{
				return false;
			}
		}
		return true;
	}
}
