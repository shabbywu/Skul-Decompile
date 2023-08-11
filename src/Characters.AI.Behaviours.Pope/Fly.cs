using System.Collections;
using Characters.AI.Pope;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class Fly : Move
{
	[SerializeField]
	private Navigation _navigation;

	[SerializeField]
	private Curve curve;

	[SerializeField]
	private float _durationMultiplierPerDistance = 1f;

	public override IEnumerator CRun(AIController controller)
	{
		controller.destination = Vector2.op_Implicit(_navigation.destination.position);
		yield return CMoveToTarget(controller, controller.character);
	}

	public override void SetDestination(Point.Tag tag)
	{
		_navigation.destinationTag = tag;
	}

	private IEnumerator CMoveToTarget(AIController controller, Character owner)
	{
		Vector2 destination = controller.destination;
		Vector3 source = ((Component)owner).transform.position;
		float elapsed = 0f;
		float num = Mathf.Abs(Vector2.Distance(destination, Vector2.op_Implicit(source)));
		double duration = (double)num * owner.stat.GetConstant(Stat.Kind.MovementSpeed) / 60.0;
		curve.duration = (float)duration;
		while (elapsed < curve.duration)
		{
			yield return null;
			if (!owner.stunedOrFreezed)
			{
				if ((double)elapsed < duration)
				{
					owner.ForceToLookAt(destination.x);
				}
				Vector2 val = Vector2.Lerp(Vector2.op_Implicit(source), destination, curve.Evaluate(elapsed));
				owner.movement.force = val - Vector2.op_Implicit(((Component)owner).transform.position);
				elapsed += ((ChronometerBase)owner.chronometer.master).deltaTime;
			}
		}
	}
}
