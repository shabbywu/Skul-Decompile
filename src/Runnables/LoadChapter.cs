using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public sealed class LoadChapter : Runnable
{
	[SerializeField]
	private Chapter.Type _chapter;

	public override void Run()
	{
		Singleton<Service>.Instance.levelManager.Load(_chapter);
	}
}
