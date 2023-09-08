using Characters.Gear.Upgrades;
using Singletons;

namespace Runnables;

public sealed class DecideRiskUpgradeLineUp : Runnable
{
	public override void Run()
	{
		Singleton<UpgradeShop>.Instance.LoadCusredLineUp();
	}
}
