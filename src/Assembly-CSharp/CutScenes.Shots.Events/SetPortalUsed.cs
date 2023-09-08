using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events;

public class SetPortalUsed : Event
{
	[SerializeField]
	private bool _portalUsed;

	public override void Run()
	{
		Singleton<Service>.Instance.levelManager.skulPortalUsed = _portalUsed;
	}
}
