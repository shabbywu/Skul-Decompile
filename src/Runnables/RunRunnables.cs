using UnityEngine;

namespace Runnables;

public sealed class RunRunnables : Runnable
{
	[SerializeField]
	private Runnable[] _runnables;

	public override void Run()
	{
		Runnable[] runnables = _runnables;
		for (int i = 0; i < runnables.Length; i++)
		{
			runnables[i].Run();
		}
	}
}
