using Services;
using Singletons;

namespace CutScenes.Shots.Events;

public class PlayerSkinUpdate : Event
{
	public override void Run()
	{
		Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.UpdateSkin();
	}
}
