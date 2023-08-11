using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Gear;
using Characters.Gear.Items;
using Data;
using FX;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using UnityEngine;

namespace Level.BlackMarket;

public class Collector : Npc
{
	private const int _randomSeed = 716722307;

	[SerializeField]
	protected SoundInfo _buySound;

	[SerializeField]
	private CollectorReroll _reroll;

	[SerializeField]
	private CollectorGearSlot[] _slots;

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private GameObject _talk;

	private Random _random;

	private Dictionary<string, int> _keywordRandomItemRerollCount;

	public string submitLine => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray("npc/collector/submit/line"));

	public bool loadCompleted { get; set; }

	private void Awake()
	{
		_reroll.onInteracted += Reroll;
		_keywordRandomItemRerollCount = new Dictionary<string, int>();
	}

	private void Start()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 716722307 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		if (MMMaths.PercentChance(_random, currentChapter.currentStage.marketSettings.collectorPossibility))
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
		((Component)_lineText).gameObject.SetActive(true);
		_talk.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(CDisplayItems());
	}

	protected override void OnDeactivate()
	{
		((Component)_lineText).gameObject.SetActive(false);
		CollectorGearSlot[] slots = _slots;
		for (int i = 0; i < slots.Length; i++)
		{
			((Component)slots[i]).gameObject.SetActive(false);
		}
	}

	private void Reroll()
	{
		((MonoBehaviour)this).StartCoroutine(CDisplayItems(reroll: true));
	}

	private IEnumerator CDisplayItems(bool reroll = false)
	{
		loadCompleted = false;
		_ = Singleton<Service>.Instance.levelManager.currentChapter;
		GearReference[] gearInfosToDrop = new GearReference[_slots.Length];
		GearRequest[] gearRequests = new GearRequest[_slots.Length];
		for (int j = 0; j < _slots.Length; j++)
		{
			((Component)_slots[j]).gameObject.SetActive(true);
			ItemReference itemToTake;
			do
			{
				Rarity rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings.collectorItemPossibilities.Evaluate(_random);
				itemToTake = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, rarity);
			}
			while (Duplicated(itemToTake.name));
			gearInfosToDrop[j] = itemToTake;
		}
		for (int k = 0; k < _slots.Length; k++)
		{
			gearRequests[k] = gearInfosToDrop[k].LoadAsync();
		}
		for (int i = 0; i < _slots.Length; i++)
		{
			while (!gearRequests[i].isDone)
			{
				yield return null;
			}
			Gear gear = Singleton<Service>.Instance.levelManager.DropGear(gearRequests[i], _slots[i].itemPosition);
			SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
			GlobalSettings marketSettings2 = Settings.instance.marketSettings;
			int price = (int)((float)marketSettings2.collectorItemPrices[gear.rarity] * marketSettings.collectorItemPriceMultiplier * marketSettings2.collectorItemPriceMultiplier * Random.Range(0.95f, 1.05f) / 10f) * 10;
			gear.dropped.price = price;
			bool destructible = gear.destructible;
			gear.destructible = false;
			gear.dropped.onLoot += OnLoot;
			CollectorGearSlot collectorGearSlot = _slots[i];
			if ((Object)(object)collectorGearSlot.droppedGear != (Object)null && collectorGearSlot.droppedGear.price > 0 && collectorGearSlot.droppedGear.gear.state == Gear.State.Dropped)
			{
				Object.Destroy((Object)(object)((Component)collectorGearSlot.droppedGear.gear).gameObject);
			}
			_slots[i].droppedGear = gear.dropped;
			if (reroll)
			{
				KeywordRandomizer component = ((Component)gear).GetComponent<KeywordRandomizer>();
				if ((Object)(object)component != (Object)null)
				{
					if (!_keywordRandomItemRerollCount.ContainsKey(((Object)gear).name))
					{
						_keywordRandomItemRerollCount.Add(((Object)gear).name, 0);
					}
					component.UpdateKeword(_keywordRandomItemRerollCount[((Object)gear).name]);
					_keywordRandomItemRerollCount[((Object)gear).name]++;
				}
			}
			gear.dropped.dropMovement.Stop();
			gear.dropped.dropMovement.Float();
			void OnLoot(Character character)
			{
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				gear.destructible = destructible;
				_lineText.Run(submitLine);
				PersistentSingleton<SoundManager>.Instance.PlaySound(_buySound, ((Component)this).transform.position);
				gear.dropped.onLoot -= OnLoot;
			}
		}
		loadCompleted = true;
		bool Duplicated(string gearNameToDrop)
		{
			for (int l = 0; l < gearInfosToDrop.Length; l++)
			{
				if (gearInfosToDrop[l] != null && gearInfosToDrop[l].name.Equals(gearNameToDrop))
				{
					return true;
				}
			}
			CollectorGearSlot[] slots = _slots;
			foreach (CollectorGearSlot collectorGearSlot2 in slots)
			{
				if ((Object)(object)collectorGearSlot2.droppedGear != (Object)null && collectorGearSlot2.droppedGear.price > 0 && collectorGearSlot2.droppedGear.gear.state == Gear.State.Dropped && ((Object)collectorGearSlot2.droppedGear.gear).name.Equals(gearNameToDrop))
				{
					return true;
				}
			}
			return false;
		}
	}
}
