using System;
using System.Collections;
using Characters;
using Characters.Gear;
using Data;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level.BlackMarket;

public class TombRaider : Npc
{
	private const int _randomSeed = 1485841739;

	[SerializeField]
	private TMP_Text _priceDisplay;

	[SerializeField]
	private Transform _slot;

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private GameObject _talk;

	private int _unlockPrice;

	private GearReference _gearToUnlock;

	private Random _random;

	public string submitLine => Localization.GetLocalizedStringArray("npc/TombRaider/submit/line").Random();

	private void Start()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		SettingsByStage marketSettings = currentChapter.currentStage.marketSettings;
		_random = new Random(GameData.Save.instance.randomSeed + 1485841739 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		Rarity rarity = marketSettings.tombRaiderGearPossibilities.Evaluate(_random);
		_gearToUnlock = Singleton<Service>.Instance.gearManager.GetGearToUnlock(_random, rarity);
		bool flag = MMMaths.PercentChance(_random, currentChapter.currentStage.marketSettings.tombRaiderPossibility);
		if (_gearToUnlock == null || !flag)
		{
			Deactivate();
			return;
		}
		_unlockPrice = marketSettings.tombRaiderUnlockPrices[_gearToUnlock.rarity];
		Activate();
	}

	private IEnumerator CDropGear()
	{
		GearRequest request = _gearToUnlock.LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		Gear droppedGear = levelManager.DropGear(request, _slot.position);
		droppedGear.dropped.price = _unlockPrice;
		droppedGear.dropped.priceCurrency = GameData.Currency.Type.DarkQuartz;
		bool destructible = droppedGear.destructible;
		droppedGear.destructible = false;
		_priceDisplay.text = droppedGear.dropped.price.ToString();
		((Graphic)_priceDisplay).color = (GameData.Currency.darkQuartz.Has(_unlockPrice) ? Color.white : Color.red);
		droppedGear.dropped.onLoot += OnLoot;
		void OnLoot(Character character)
		{
			droppedGear.destructible = destructible;
			_unlockPrice = 0;
			_priceDisplay.text = "---";
			_lineText.Run(submitLine);
			_gearToUnlock.Unlock();
			droppedGear.dropped.onLoot -= OnLoot;
		}
	}

	protected override void OnActivate()
	{
		((Component)_lineText).gameObject.SetActive(true);
		_talk.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(CDropGear());
	}

	protected override void OnDeactivate()
	{
		((Component)_lineText).gameObject.SetActive(false);
		_priceDisplay.text = string.Empty;
	}
}
