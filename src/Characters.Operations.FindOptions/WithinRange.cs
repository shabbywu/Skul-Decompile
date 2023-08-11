using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class WithinRange : IScope
{
	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private LayerMask _layerMask;

	private static NonAllocOverlapper _overlapper;

	static WithinRange()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(31);
	}

	public List<Character> GetEnemyList()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layerMask);
		return _overlapper.OverlapCollider(_range).GetComponents<Character>(true);
	}
}
