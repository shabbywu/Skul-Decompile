using Characters;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes;

public class PlayerMovementBlock : State
{
	public override void Attach()
	{
		Singleton<Service>.Instance.levelManager.player.movement.blocked.Attach(State.key);
	}

	public override void Detach()
	{
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if (!((Object)(object)levelManager == (Object)null))
		{
			Character player = levelManager.player;
			if ((Object)(object)player != (Object)null)
			{
				player.movement.blocked.Detach(State.key);
			}
		}
	}

	private void OnDisable()
	{
		Detach();
	}

	private void OnDestroy()
	{
		Detach();
	}
}
