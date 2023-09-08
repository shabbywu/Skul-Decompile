using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public class Dash : Behaviour
{
	[UnityEditor.Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Characters.Actions.Action _readyAction;

	[UnityEditor.Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Characters.Actions.Action _attackAction;

	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2 _distanceRange;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(MoveToDestination))]
	private MoveToDestination _moveToDestination;

	[SerializeField]
	private Curve curve;

	[SerializeField]
	private float _durationMultiplierPerDistance = 1f;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		_readyAction.TryStart();
		while (_readyAction.running)
		{
			yield return null;
		}
		SetDestination(controller);
		_attackAction.TryStart();
		yield return CMoveToDestination(controller, controller.character);
		if (!controller.dead)
		{
			controller.character.CancelAction();
		}
		base.result = Result.Success;
	}

	private void SetDestination(AIController controller)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(_distanceRange.x, _distanceRange.y);
		if ((Object)(object)controller.target == (Object)null)
		{
			throw new Exception("target is null");
		}
		Bounds bounds = controller.character.movement.controller.collisionState.lastStandingCollider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		float num2;
		if (MMMaths.RandomBool())
		{
			bounds = controller.character.movement.controller.collisionState.lastStandingCollider.bounds;
			num2 = Math.Min(((Bounds)(ref bounds)).max.x, ((Component)controller.target).transform.position.x + num);
		}
		else
		{
			bounds = controller.character.movement.controller.collisionState.lastStandingCollider.bounds;
			num2 = Math.Max(((Bounds)(ref bounds)).min.x, ((Component)controller.target).transform.position.x - num);
		}
		controller.destination = new Vector2(num2, y);
	}

	private IEnumerator CMoveToDestination(AIController controller, Character owner)
	{
		Vector2 destination = controller.destination;
		Vector3 source = ((Component)owner).transform.position;
		float elapsed = 0f;
		float num = Mathf.Abs(destination.x - source.x);
		float duration = num * owner.stat.GetInterpolatedMovementSpeed() / 60f;
		curve.duration = duration * _durationMultiplierPerDistance;
		while (elapsed < curve.duration)
		{
			yield return null;
			if (!owner.stunedOrFreezed)
			{
				if (elapsed < duration)
				{
					owner.ForceToLookAt(destination.x);
				}
				float num2 = Mathf.Lerp(source.x, destination.x, curve.Evaluate(elapsed));
				owner.movement.force = new Vector2(num2 - ((Component)owner).transform.position.x, 0f);
				elapsed += owner.chronometer.master.deltaTime;
			}
		}
	}
}
