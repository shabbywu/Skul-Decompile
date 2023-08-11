using System.Collections;
using UnityEngine;

namespace Runnables;

public sealed class TransformTranslateTo : CRunnable
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _destination;

	[SerializeField]
	private Curve curve;

	public override IEnumerator CRun()
	{
		Vector3 start = ((Component)_target).transform.position;
		Vector3 end = _destination.position;
		for (float elapsed = 0f; elapsed < curve.duration; elapsed += ((ChronometerBase)Chronometer.global).deltaTime)
		{
			yield return null;
			((Component)_target).transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(start), Vector2.op_Implicit(end), curve.Evaluate(elapsed)));
		}
		((Component)_target).transform.position = end;
	}
}
