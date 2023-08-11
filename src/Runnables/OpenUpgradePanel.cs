using Scenes;

namespace Runnables;

public sealed class OpenUpgradePanel : Runnable
{
	public override void Run()
	{
		Scene<GameBase>.instance.uiManager.upgradeShop.Open();
	}
}
