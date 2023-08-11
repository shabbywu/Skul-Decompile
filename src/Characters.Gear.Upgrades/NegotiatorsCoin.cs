using Level.BlackMarket;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class NegotiatorsCoin : UpgradeAbility
{
	[SerializeField]
	private int _extraFreeRefreshCount;

	[SerializeField]
	private float _extraCollectorPriceMultiplier;

	public override void Attach(Character target)
	{
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogError((object)"Player is null");
			return;
		}
		GlobalSettings marketSettings = Settings.instance.marketSettings;
		marketSettings.collectorItemPriceMultiplier += _extraCollectorPriceMultiplier;
		marketSettings.collectorFreeRefreshCount += _extraFreeRefreshCount;
	}

	public override void Detach()
	{
		GlobalSettings marketSettings = Settings.instance.marketSettings;
		marketSettings.collectorItemPriceMultiplier -= _extraCollectorPriceMultiplier;
		marketSettings.collectorFreeRefreshCount -= _extraFreeRefreshCount;
	}
}
