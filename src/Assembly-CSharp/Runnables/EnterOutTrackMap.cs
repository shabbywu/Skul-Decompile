using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public sealed class EnterOutTrackMap : Runnable
{
	[SerializeField]
	private bool _playerReset;

	[SerializeField]
	private Map _map;

	public override void Run()
	{
		Singleton<Service>.Instance.levelManager.EnterOutTrack(_map, _playerReset);
	}
}
