using System.Collections;
using UnityEngine;

namespace Runnables;

public sealed class ScaleTo : CRunnable
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Vector2 _destination;

	[SerializeField]
	private Curve curve;

	public override IEnumerator CRun()
	{
		Vector3 start = ((Component)_target).transform.localScale;
		Vector2 end = _destination;
		for (float elapsed = 0f; elapsed < curve.duration; elapsed += ((ChronometerBase)Chronometer.global).deltaTime)
		{
			yield return null;
			((Component)_target).transform.localScale = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(start), end, curve.Evaluate(elapsed)));
		}
		((Component)_target).transform.localScale = Vector2.op_Implicit(end);
	}
}
