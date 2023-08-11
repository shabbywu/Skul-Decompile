using UnityEngine;

namespace Runnables.Chances;

public sealed class Constant : Chance
{
	[Range(0f, 1f)]
	[SerializeField]
	private float _truePercent;

	public override bool IsTrue()
	{
		return MMMaths.Chance(_truePercent);
	}
}
