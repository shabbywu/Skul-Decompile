using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class MoveToBehindWithFly : Behaviour
{
	[Subcomponent(typeof(MoveToDestinationWithFly))]
	[SerializeField]
	private MoveToDestinationWithFly _moveToDestinationWithFly;

	[SerializeField]
	private float _distanceX;

	[SerializeField]
	private float _midPointHeight;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Character target = controller.target;
		float num = ((Component)target).transform.position.x - ((Component)character).transform.position.x;
		Vector2 midPoint = new Vector2(((Component)target).transform.position.x, ((Component)target).transform.position.y + _midPointHeight);
		float behindPosition = ((num > 0f) ? (((Component)target).transform.position.x - _distanceX) : (((Component)target).transform.position.x + _distanceX));
		base.result = Result.Doing;
		while (base.result == Result.Doing)
		{
			yield return null;
			controller.destination = midPoint;
			yield return _moveToDestinationWithFly.CRun(controller);
			controller.destination = new Vector2(behindPosition, ((Component)target).transform.position.y);
			yield return _moveToDestinationWithFly.CRun(controller);
		}
	}
}
