using System;
using Characters.Abilities;
using Data;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level.BlackMarket;

public class Chef : Npc
{
	private const int _randomSeed = 1177618293;

	[SerializeField]
	private TMP_Text _priceDisplay;

	[SerializeField]
	private Transform _slot;

	[SerializeField]
	private AbilityBuffList _foodList;

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private GameObject _talk;

	private AbilityBuff _foodInstance;

	private int _price;

	private Random _random;

	public string submitLine => Localization.GetLocalizedStringArray("npc/chef/submit/line").Random();

	private void Start()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 1177618293 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		if (MMMaths.PercentChance(_random, currentChapter.currentStage.marketSettings.masterPossibility))
		{
			Activate();
		}
		else
		{
			Deactivate();
		}
	}

	protected override void OnActivate()
	{
		SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
		GlobalSettings marketSettings2 = Settings.instance.marketSettings;
		Rarity rarity = marketSettings.masterDishPossibilities.Evaluate(_random);
		AbilityBuff abilityBuff = _foodList.Take(_random, rarity);
		float num = (float)marketSettings2.masterDishPrices[rarity] * marketSettings.masterDishPriceMultiplier;
		num *= Random.Range(0.95f, 1.05f);
		_price = (int)(num / 10f) * 10;
		_foodInstance = Object.Instantiate<AbilityBuff>(abilityBuff, _slot);
		((Object)_foodInstance).name = ((Object)abilityBuff).name;
		_foodInstance.price = _price;
		_foodInstance.onSold += delegate
		{
			_price = 0;
			_priceDisplay.text = "---";
			_lineText.Run(submitLine);
		};
		_foodInstance.Initialize();
		_priceDisplay.text = _price.ToString();
		((Component)_lineText).gameObject.SetActive(true);
		_talk.SetActive(true);
	}

	protected override void OnDeactivate()
	{
		((Component)_lineText).gameObject.SetActive(false);
		_priceDisplay.text = string.Empty;
	}

	public void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_priceDisplay).color = (GameData.Currency.gold.Has(_price) ? Color.white : Color.red);
	}
}
