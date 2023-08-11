using Characters.Gear.Upgrades;

namespace Data.Hardmode.Upgrades;

public sealed class UpgradeShopContext
{
	private UpgradeShopSettings _settings;

	public UpgradeShopContext()
	{
		_settings = UpgradeShopSettings.instance;
	}

	public int GetRemoveCost(UpgradeObject.Type type)
	{
		return _settings.GetRemoveCost(type);
	}
}
