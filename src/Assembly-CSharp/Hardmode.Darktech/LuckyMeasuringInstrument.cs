using System;
using System.Collections;
using Characters;
using Characters.Gear;
using Characters.Gear.Items;
using Data;
using GameResources;
using Level;
using Level.BlackMarket;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class LuckyMeasuringInstrument : MonoBehaviour
{
	private const int _randomSeed = 716722307;

	[SerializeField]
	private GameObject _body;

	[SerializeField]
	private LuckyMeasuringInstrumentSlot _slot;

	[SerializeField]
	private LuckyMeasuringInstrumentReroll _reroll;

	[SerializeField]
	private Collector _collector;

	private Random _random;

	private Rarity _rarity;

	public int lootCount => Singleton<DarktechManager>.Instance.setting.행운계측기설정.lootableCount;

	public int remainLootCount => lootCount - GameData.HardmodeProgress.luckyMeasuringInstrument.lootCount.value;

	private void Start()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 716722307 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		int value = GameData.HardmodeProgress.luckyMeasuringInstrument.lootCount.value;
		int lootableCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.lootableCount;
		_reroll.@base = this;
		_reroll.onInteracted += Load;
		_reroll.Initialize();
		if (value >= lootableCount)
		{
			_reroll.Deactivate();
		}
		else
		{
			Load();
		}
	}

	private void EvaluateRarity()
	{
		int uniquePityCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.uniquePityCount;
		int legendaryPityCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.legendaryPityCount;
		GameData.HardmodeProgress.LuckyMeasuringInstrument luckyMeasuringInstrument = GameData.HardmodeProgress.luckyMeasuringInstrument;
		IntData lastUniqueDropOrder = luckyMeasuringInstrument.lastUniqueDropOrder;
		IntData lastLegendaryDropOrder = luckyMeasuringInstrument.lastLegendaryDropOrder;
		_rarity = Singleton<DarktechManager>.Instance.setting.행운계측기설정.weightByRarity.Evaluate(_random);
		IntData refreshCount = luckyMeasuringInstrument.refreshCount;
		if (refreshCount.value - lastLegendaryDropOrder.value >= legendaryPityCount)
		{
			_rarity = Rarity.Legendary;
			lastLegendaryDropOrder.value = refreshCount.value;
		}
		else if (refreshCount.value - lastUniqueDropOrder.value >= uniquePityCount && refreshCount.value - lastLegendaryDropOrder.value >= uniquePityCount)
		{
			_rarity = Rarity.Unique;
			lastUniqueDropOrder.value = refreshCount.value;
		}
		switch (_rarity)
		{
		case Rarity.Unique:
			lastUniqueDropOrder.value = refreshCount.value;
			break;
		case Rarity.Legendary:
			lastLegendaryDropOrder.value = refreshCount.value;
			break;
		}
	}

	private IEnumerator CLoad()
	{
		while (!_collector.loadCompleted)
		{
			yield return null;
		}
		_ = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
		GameData.HardmodeProgress.LuckyMeasuringInstrument luckyMeasuringInstrumentData = GameData.HardmodeProgress.luckyMeasuringInstrument;
		_ = luckyMeasuringInstrumentData.refreshCount;
		ItemReference itemReference = null;
		while (itemReference == null)
		{
			bool flag = false;
			EvaluateRarity();
			itemReference = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _rarity);
			for (int i = 0; i < luckyMeasuringInstrumentData.items.length; i++)
			{
				string text = luckyMeasuringInstrumentData.items[i];
				if (text != null && itemReference.name.Equals(text, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
				}
			}
			if (flag)
			{
				itemReference = null;
			}
		}
		ItemRequest gearRequest = itemReference.LoadAsync();
		while (!gearRequest.isDone)
		{
			yield return null;
		}
		Item gear = Singleton<Service>.Instance.levelManager.DropItem(gearRequest, _slot.dropPoint);
		bool destructible = gear.destructible;
		gear.destructible = false;
		gear.dropped.onLoot += OnLoot;
		string name = ((Object)gear).name;
		if (luckyMeasuringInstrumentData.refreshCount.value < luckyMeasuringInstrumentData.maxRefreshCount)
		{
			luckyMeasuringInstrumentData.items[luckyMeasuringInstrumentData.refreshCount.value] = name;
		}
		if ((Object)(object)_slot.droppedGear != (Object)null && _slot.droppedGear.gear.state == Gear.State.Dropped)
		{
			Object.Destroy((Object)(object)((Component)_slot.droppedGear.gear).gameObject);
		}
		_slot.droppedGear = gear.dropped;
		gear.dropped.dropMovement.Stop();
		gear.dropped.dropMovement.Float();
		void OnLoot(Character character)
		{
			gear.destructible = destructible;
			gear.dropped.onLoot -= OnLoot;
			luckyMeasuringInstrumentData.lootCount.value++;
			_ = Singleton<DarktechManager>.Instance.setting.행운계측기설정.lootableCount;
			_reroll.Deactivate();
		}
	}

	private void Load()
	{
		((MonoBehaviour)this).StartCoroutine(CLoad());
	}
}
