using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Data;
using Data.Hardmode.Upgrades;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class UpgradeShop : Singleton<UpgradeShop>
{
	public const int cursedCount = 2;

	private const int _randomSeed = 2028506624;

	private Random _random;

	private UpgradeShopContext _context;

	private List<UpgradeResource.Reference> _cursedLineUp;

	private List<UpgradeResource.Reference> _sortedUpgradables;

	private List<UpgradeResource.Reference> _upgradableCache;

	public GameData.Currency saleCurrency => GameData.Currency.heartQuartz;

	public GameData.Currency removeCurrency => GameData.Currency.gold;

	public event Action onChanged;

	public event Action<UpgradeResource.Reference> onUpgraded;

	public event Action<UpgradeResource.Reference> onLevelUp;

	protected override void Awake()
	{
		base.Awake();
		((Object)this).name = "UpgradeShop";
		Sort();
		_upgradableCache = new List<UpgradeResource.Reference>(_sortedUpgradables.Count);
		CreateNewContext();
	}

	private void Sort()
	{
		if (_sortedUpgradables == null)
		{
			_sortedUpgradables = new List<UpgradeResource.Reference>();
		}
		else
		{
			_sortedUpgradables.Clear();
		}
		foreach (UpgradeResource.Reference allUnlocked in Singleton<UpgradeManager>.Instance.GetAllUnlockedList())
		{
			_sortedUpgradables.Add(allUnlocked);
		}
		_sortedUpgradables.Sort(delegate(UpgradeResource.Reference refer1, UpgradeResource.Reference refer2)
		{
			if (refer1.orderInShop > refer2.orderInShop)
			{
				return 1;
			}
			return (refer1.orderInShop < refer2.orderInShop) ? (-1) : 0;
		});
	}

	public int GetRemoveCost(UpgradeObject.Type type)
	{
		return _context.GetRemoveCost(type);
	}

	public void LoadCusredLineUp()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		List<UpgradeResource.Reference> list = Singleton<UpgradeManager>.Instance.GetList(UpgradeObject.Type.Cursed);
		list.PseudoShuffle(_random);
		if (_cursedLineUp == null)
		{
			_cursedLineUp = new List<UpgradeResource.Reference>();
		}
		else
		{
			_cursedLineUp.Clear();
		}
		int num = 0;
		int num2 = 0;
		int balance = saleCurrency.balance;
		string typeName = Gear.Type.Upgrade.ToString();
		while (num < 2 && num2 < list.Count)
		{
			UpgradeResource.Reference reference = list[num2++];
			if ((!reference.needUnlock || GameData.Gear.IsUnlocked(typeName, reference.name)) && reference.price <= balance)
			{
				_cursedLineUp.Add(reference);
				num++;
			}
		}
		if (num >= 2)
		{
			return;
		}
		num = 0;
		num2 = 0;
		_cursedLineUp.Clear();
		while (num < 2 && num2 < list.Count)
		{
			UpgradeResource.Reference reference2 = list[num2++];
			if (!reference2.needUnlock || GameData.Gear.IsUnlocked(typeName, reference2.name))
			{
				_cursedLineUp.Add(reference2);
				num++;
			}
		}
	}

	public List<UpgradeResource.Reference> GetRiskObjects()
	{
		return _cursedLineUp;
	}

	public List<UpgradeResource.Reference> GetUpgradables()
	{
		Sort();
		return _sortedUpgradables;
	}

	public bool TryUpgrade(UpgradeResource.Reference reference)
	{
		if (!CheckPurchasable(reference))
		{
			return false;
		}
		if (!Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(reference))
		{
			Upgrade(reference);
			return true;
		}
		return false;
	}

	public bool TryLevelUp(UpgradeResource.Reference reference)
	{
		if (!CheckPurchasable(reference))
		{
			return false;
		}
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		int num = upgrade.IndexOf(reference);
		if (num == -1)
		{
			return false;
		}
		UpgradeObject upgradeObject = upgrade.upgrades[num];
		if (reference.GetCurrentLevel() + 1 <= upgradeObject.maxLevel)
		{
			saleCurrency.Consume(reference.price);
			LevelUp(upgradeObject);
			this.onChanged?.Invoke();
			this.onLevelUp?.Invoke(reference);
			return true;
		}
		return false;
	}

	private void Upgrade(UpgradeResource.Reference reference)
	{
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		saleCurrency.Consume(reference.price);
		upgrade.TryEquip(reference);
		this.onChanged?.Invoke();
		this.onUpgraded?.Invoke(reference);
	}

	public bool TryRemoveUpgradeObjet(UpgradeResource.Reference reference, ref int index)
	{
		int removeCost = GetRemoveCost(reference.type);
		if (!CheckRemovable(removeCost))
		{
			return false;
		}
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		index = upgrade.IndexOf(reference);
		if (index == -1)
		{
			Debug.LogError((object)"해당 강화 요소를 가지고 있지 않습니다.");
			return false;
		}
		UpgradeObject @object = upgrade.upgrades[index];
		upgrade.Remove(index);
		upgrade.Trim();
		SettleRemovedObject(@object, removeCost);
		this.onChanged?.Invoke();
		return true;
	}

	public bool CheckRemovable(UpgradeResource.Reference reference)
	{
		return CheckRemovable(GetRemoveCost(reference.type));
	}

	private void CreateNewContext()
	{
		_context = new UpgradeShopContext();
	}

	private bool CheckRemovable(int cost)
	{
		if (!removeCurrency.Has(cost))
		{
			return false;
		}
		return true;
	}

	private void SettleRemovedObject(UpgradeObject @object, int cost)
	{
		removeCurrency.Consume(cost);
		saleCurrency.Earn(@object.returnCost);
	}

	private bool CheckPurchasable(UpgradeResource.Reference reference)
	{
		UpgradeInventory upgrade2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		if (!upgrade2.upgrades.Any((UpgradeObject upgrade) => (Object)(object)upgrade == (Object)null) && !upgrade2.Has(reference))
		{
			return false;
		}
		if (reference == null)
		{
			return false;
		}
		if (reference.GetCurrentLevel() == reference.maxLevel)
		{
			return false;
		}
		if (!saleCurrency.Has(reference.price))
		{
			return false;
		}
		return true;
	}

	private void LevelUp(UpgradeObject @object)
	{
		@object.LevelUpOrigin();
	}
}
