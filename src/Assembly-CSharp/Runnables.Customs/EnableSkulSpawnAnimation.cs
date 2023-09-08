using Services;
using Singletons;
using UnityEngine;

namespace Runnables.Customs;

public sealed class EnableSkulSpawnAnimation : Runnable
{
	[SerializeField]
	private bool _enable;

	public override void Run()
	{
		Singleton<Service>.Instance.levelManager.skulSpawnAnimaionEnable = _enable;
	}
}
