using System.Collections;
using Characters;
using UnityEngine;

namespace Runnables;

public sealed class CharacterTranslateTo : CRunnable
{
	[SerializeField]
	private Target _target;

	[SerializeField]
	private Transform _destination;

	[SerializeField]
	private Curve curve;

	public override IEnumerator CRun()
	{
		Character character = _target.character;
		Vector3 start = ((Component)character).transform.position;
		Vector3 end = _destination.position;
		for (float elapsed = 0f; elapsed < curve.duration; elapsed += ((ChronometerBase)Chronometer.global).deltaTime)
		{
			yield return null;
			((Component)character).transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(start), Vector2.op_Implicit(end), curve.Evaluate(elapsed)));
		}
		((Component)character).transform.position = end;
	}
}
