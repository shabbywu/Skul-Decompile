using System.Collections;
using Characters.AI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations;

public class LeapWithFly : CharacterOperation
{
	[SerializeField]
	private AIController _aiController;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Curve curve;

	[FormerlySerializedAs("_chaseTime")]
	[SerializeField]
	private float _lookingTime;

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CMoveToTarget(owner));
	}

	public override void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private IEnumerator CMoveToTarget(Character owner)
	{
		Vector3 destination = (((Object)(object)_target == (Object)null) ? ((Component)_aiController.target).transform.position : ((Component)_target).transform.position);
		Vector3 source = ((Component)owner).transform.position;
		for (float elapsed = 0f; elapsed < curve.duration; elapsed += owner.chronometer.master.deltaTime)
		{
			yield return null;
			while (owner.stunedOrFreezed)
			{
				yield return null;
			}
			if (elapsed < _lookingTime)
			{
				destination = (((Object)(object)_target == (Object)null) ? ((Component)_aiController.target).transform.position : ((Component)_target).transform.position);
				owner.ForceToLookAt(destination.x);
			}
			Vector2 val = Vector2.Lerp(Vector2.op_Implicit(source), Vector2.op_Implicit(destination), curve.Evaluate(elapsed));
			owner.movement.force = val - Vector2.op_Implicit(((Component)owner).transform.position);
		}
	}
}
