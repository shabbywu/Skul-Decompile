using System;
using System.Collections;
using Characters;
using Characters.Gear;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class AdventurerReward : MonoBehaviour, ILootable
{
	private const int _randomSeed = -1222149140;

	[SerializeField]
	private SpriteRenderer _choiceTable;

	[SerializeField]
	private SoundInfo _buySound;

	[SerializeField]
	private AdventurerRewardSlot[] _slots;

	[SerializeField]
	private BonusCurrencyWithDroppedGear _hardmodeBonus;

	private GearReference[] _gearInfosToDrop;

	private GearRequest[] _gearRequests;

	private int _discardCount;

	private bool _looted;

	public bool looted
	{
		get
		{
			return _looted;
		}
		private set
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (!_looted && value)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(_buySound, ((Component)this).transform.position);
			}
			_looted = value;
		}
	}

	public event Action onLoot;

	private void Awake()
	{
		_choiceTable.sprite = Singleton<Service>.Instance.levelManager.currentChapter.gateChoiceTable;
	}

	private void Load()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		Random random = new Random(GameData.Save.instance.randomSeed + -1222149140 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_gearInfosToDrop = new GearReference[_slots.Length];
		_gearRequests = new GearRequest[_slots.Length];
		for (int i = 0; i < _slots.Length; i++)
		{
			((Component)_slots[i]).gameObject.SetActive(true);
			RarityPossibilities gearPossibilities = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities;
			GearManager gearManager = Singleton<Service>.Instance.gearManager;
			WeaponReference weaponToTake = gearManager.GetWeaponToTake(random, gearPossibilities.Evaluate(random));
			EssenceReference quintessenceToTake = gearManager.GetQuintessenceToTake(random, gearPossibilities.Evaluate(random));
			ItemReference itemToTake = gearManager.GetItemToTake(random, gearPossibilities.Evaluate(random));
			_gearInfosToDrop[0] = weaponToTake;
			_gearInfosToDrop[1] = quintessenceToTake;
			_gearInfosToDrop[2] = itemToTake;
		}
		for (int j = 0; j < _slots.Length; j++)
		{
			_gearRequests[j] = _gearInfosToDrop[j].LoadAsync();
		}
	}

	public void Activate()
	{
		Load();
		((MonoBehaviour)this).StartCoroutine(CDisplayItems());
	}

	private IEnumerator CDisplayItems()
	{
		for (int i = 0; i < _slots.Length; i++)
		{
			_ = i;
			while (!_gearRequests[i].isDone)
			{
				yield return null;
			}
			Gear gear = Singleton<Service>.Instance.levelManager.DropGear(_gearRequests[i], _slots[i].displayPosition);
			gear.onDiscard += OnDiscard2;
			bool destructible = gear.destructible;
			_slots[i].droppedGear = gear.dropped;
			if (destructible)
			{
				gear.dropped.onLoot += OnLoot2;
			}
		}
	}

	private void OnLoot2(Character character)
	{
		this.onLoot?.Invoke();
		looted = true;
		for (int i = 0; i < _slots.Length; i++)
		{
			AdventurerRewardSlot adventurerRewardSlot = _slots[i];
			adventurerRewardSlot.droppedGear.onLoot -= OnLoot2;
			adventurerRewardSlot.droppedGear.gear.onDiscard -= OnDiscard2;
			if (adventurerRewardSlot.droppedGear.gear.state == Gear.State.Dropped)
			{
				adventurerRewardSlot.droppedGear.gear.destructible = false;
				((Component)adventurerRewardSlot.droppedGear.gear).gameObject.SetActive(false);
			}
		}
	}

	private void OnDiscard2(Gear gear)
	{
		this.onLoot?.Invoke();
		looted = true;
		for (int i = 0; i < _slots.Length; i++)
		{
			AdventurerRewardSlot adventurerRewardSlot = _slots[i];
			adventurerRewardSlot.droppedGear.onLoot -= OnLoot2;
			adventurerRewardSlot.droppedGear.gear.onDiscard -= OnDiscard2;
			if ((Object)(object)gear != (Object)(object)adventurerRewardSlot.droppedGear.gear && adventurerRewardSlot.droppedGear.gear.state == Gear.State.Dropped)
			{
				adventurerRewardSlot.droppedGear.gear.destructible = false;
				((Component)adventurerRewardSlot.droppedGear.gear).gameObject.SetActive(false);
			}
		}
	}
}
