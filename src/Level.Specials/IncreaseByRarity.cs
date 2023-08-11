using Level.BlackMarket;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Specials;

[RequireComponent(typeof(TimeCostEventReward))]
public class IncreaseByRarity : MonoBehaviour
{
	[SerializeField]
	private TimeCostEvent _costReward;

	[SerializeField]
	private TimeCostEventReward _reward;

	[SerializeField]
	private ValueByRarity _multiplierByRarity;

	private void Start()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
		GlobalSettings marketSettings2 = Settings.instance.marketSettings;
		float costSpeed = (float)marketSettings2.collectorItemPrices[_reward.rarity] * marketSettings.collectorItemPriceMultiplier * marketSettings2.collectorItemPriceMultiplier;
		_costReward.SetCostSpeed(costSpeed);
	}
}
