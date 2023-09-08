using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events;

public class DestroyPlayer : Event
{
	private Character _player;

	public override void Run()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		if (!((Object)(object)_player == (Object)null))
		{
			Object.Destroy((Object)(object)((Component)_player).gameObject);
		}
	}
}
