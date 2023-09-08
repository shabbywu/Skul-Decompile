using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Height : Decorator
{
	private enum Comparer
	{
		Greater,
		Lesser
	}

	[SerializeField]
	private Comparer _comparer;

	[SerializeField]
	private float _diff;

	[SerializeField]
	private bool _lastStandingCollider;

	[SerializeField]
	private LayerMask _terrainLayer = Layers.groundMask;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		Bounds bounds;
		if (!_lastStandingCollider || (Object)(object)lastStandingCollider == (Object)null)
		{
			if (!character.movement.TryGetClosestBelowCollider(out var collider, _terrainLayer))
			{
				base.result = Result.Fail;
				yield break;
			}
			switch (_comparer)
			{
			case Comparer.Greater:
			{
				float y2 = ((Component)character).transform.position.y;
				bounds = collider.bounds;
				if (y2 - ((Bounds)(ref bounds)).max.y >= _diff)
				{
					base.result = Result.Success;
				}
				else
				{
					base.result = Result.Fail;
				}
				break;
			}
			case Comparer.Lesser:
			{
				float y = ((Component)character).transform.position.y;
				bounds = collider.bounds;
				if (y - ((Bounds)(ref bounds)).max.y < _diff)
				{
					base.result = Result.Success;
				}
				else
				{
					base.result = Result.Fail;
				}
				break;
			}
			}
		}
		switch (_comparer)
		{
		case Comparer.Greater:
		{
			float y4 = ((Component)character).transform.position.y;
			bounds = lastStandingCollider.bounds;
			if (y4 - ((Bounds)(ref bounds)).max.y >= _diff)
			{
				base.result = Result.Success;
			}
			else
			{
				base.result = Result.Fail;
			}
			break;
		}
		case Comparer.Lesser:
		{
			float y3 = ((Component)character).transform.position.y;
			bounds = lastStandingCollider.bounds;
			if (y3 - ((Bounds)(ref bounds)).max.y < _diff)
			{
				base.result = Result.Success;
			}
			else
			{
				base.result = Result.Fail;
			}
			break;
		}
		}
	}
}
