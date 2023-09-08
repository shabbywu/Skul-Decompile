using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Services;
using Singletons;

namespace Characters.Gear.Upgrades;

public sealed class UpgradeManager : Singleton<UpgradeManager>
{
	private List<UpgradeResource.Reference> _upgrades;

	protected override void Awake()
	{
		base.Awake();
		UpgradeResource.instance.Initialize();
		Initialize();
	}

	private void Initialize()
	{
		UpgradeResource instance = UpgradeResource.instance;
		_upgrades = instance.upgradeReferences;
	}

	public List<UpgradeResource.Reference> GetAllList()
	{
		return _upgrades;
	}

	public List<UpgradeResource.Reference> GetAllUnlockedList()
	{
		return _upgrades.Where((UpgradeResource.Reference info) => (!info.needUnlock || GameData.Gear.IsUnlocked(Gear.Type.Upgrade.ToString(), info.name)) ? true : false).ToList();
	}

	public List<UpgradeResource.Reference> GetList(UpgradeObject.Type type)
	{
		return _upgrades.Where(delegate(UpgradeResource.Reference @object)
		{
			if (@object.type != type)
			{
				return false;
			}
			return (!Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(@object)) ? true : false;
		}).ToList();
	}

	public List<UpgradeResource.Reference> GetUnlockList(UpgradeObject.Type type)
	{
		return _upgrades.Where(delegate(UpgradeResource.Reference @object)
		{
			if (@object.type != type)
			{
				return false;
			}
			if (@object.needUnlock && !GameData.Gear.IsUnlocked(Gear.Type.Upgrade.ToString(), @object.name))
			{
				return false;
			}
			return (!Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(@object)) ? true : false;
		}).ToList();
	}

	public UpgradeResource.Reference FindByName(string name)
	{
		foreach (UpgradeResource.Reference upgrade in _upgrades)
		{
			if (upgrade.name.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				return upgrade;
			}
		}
		return null;
	}
}
