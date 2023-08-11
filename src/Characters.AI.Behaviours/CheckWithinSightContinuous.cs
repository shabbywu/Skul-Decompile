using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class CheckWithinSightContinuous : Behaviour
{
	[SerializeField]
	private Collider2D _sightCollider;

	[SerializeField]
	private LayerMask _blockedLayers;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		while (base.result == Result.Doing)
		{
			yield return null;
			if (!((Object)(object)controller.target != (Object)null))
			{
				Character character = controller.FindClosestPlayerBody(_sightCollider, ((Component)controller.character).transform.position, _blockedLayers);
				if ((Object)(object)character != (Object)null && !character.stealth.value)
				{
					controller.target = character;
					controller.FoundEnemy();
					base.result = Result.Done;
				}
			}
		}
	}
}
