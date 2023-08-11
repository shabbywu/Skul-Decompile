using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Weapons;
using GameResources;
using Singletons;
using UnityEngine;

namespace Services;

public class GearManager : MonoBehaviour
{
	private readonly EnumArray<Rarity, ItemReference[]> _items = new EnumArray<Rarity, ItemReference[]>();

	private readonly EnumArray<Rarity, EssenceReference[]> _quintessences = new EnumArray<Rarity, EssenceReference[]>();

	private readonly EnumArray<Rarity, WeaponReference[]> _weapons = new EnumArray<Rarity, WeaponReference[]>();

	private readonly EnumArray<Rarity, List<GearReference>> _lockedGears = new EnumArray<Rarity, List<GearReference>>();

	private readonly List<Gear> _itemInstances = new List<Gear>();

	private readonly List<Gear> _essenceInstances = new List<Gear>();

	private readonly List<Gear> _weaponInstances = new List<Gear>();

	private List<Item> _itemInstancesCached = new List<Item>();

	private string[] _keywordRandomizerItems = new string[4] { "PrincesBox", "PrincesBox_2", "CloneStamp", "CloneStamp_2" };

	public bool initialized { get; private set; }

	public event Action onItemInstanceChanged;

	public event Action onEssenceInstanceChanged;

	public event Action onWeaponInstanceChanged;

	public void Initialize()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		GearResource instance = GearResource.instance;
		ReadOnlyCollection<ItemReference> items = instance.items;
		ReadOnlyCollection<EssenceReference> essences = instance.essences;
		ReadOnlyCollection<WeaponReference> weapons = instance.weapons;
		foreach (IGrouping<Rarity, ItemReference> item in from item in items
			group item by item.rarity)
		{
			_items[item.Key] = item.ToArray();
		}
		foreach (IGrouping<Rarity, EssenceReference> item2 in from quintessence in essences
			group quintessence by quintessence.rarity)
		{
			_quintessences[item2.Key] = item2.ToArray();
		}
		foreach (IGrouping<Rarity, WeaponReference> item3 in from weapon in weapons
			group weapon by weapon.rarity)
		{
			_weapons[item3.Key] = item3.ToArray();
		}
		initialized = true;
	}

	public void RegisterItemInstance(Gear item)
	{
		_itemInstances.Add(item);
		this.onItemInstanceChanged?.Invoke();
	}

	public void UnregisterItemInstance(Gear item)
	{
		_itemInstances.Remove(item);
		this.onItemInstanceChanged?.Invoke();
	}

	public void RegisterEssenceInstance(Gear essence)
	{
		_essenceInstances.Add(essence);
		this.onEssenceInstanceChanged?.Invoke();
	}

	public void UnregisterEssenceInstance(Gear essence)
	{
		_essenceInstances.Remove(essence);
		this.onEssenceInstanceChanged?.Invoke();
	}

	public void RegisterWeaponInstance(Gear weapon)
	{
		_weaponInstances.Add(weapon);
		this.onWeaponInstanceChanged?.Invoke();
	}

	public void UnregisterWeaponInstance(Gear weapon)
	{
		_weaponInstances.Remove(weapon);
		this.onWeaponInstanceChanged?.Invoke();
	}

	public void DestroyDroppedInstaces()
	{
		DestroyGearInstances(_itemInstances);
		DestroyGearInstances(_essenceInstances);
		DestroyGearInstances(_weaponInstances);
		static void DestroyGearInstances(List<Gear> instances)
		{
			for (int num = instances.Count - 1; num >= 0; num--)
			{
				Gear gear = instances[num];
				if (gear.state == Gear.State.Dropped && gear.currencyByDiscard != 0)
				{
					Object.Destroy((Object)(object)((Component)gear).gameObject);
				}
			}
		}
	}

	private void UpdateLockedGearList()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		GearResource instance = GearResource.instance;
		ReadOnlyCollection<ItemReference> items = instance.items;
		ReadOnlyCollection<EssenceReference> essences = instance.essences;
		ReadOnlyCollection<WeaponReference> weapons = instance.weapons;
		List<GearReference> list = new List<GearReference>(items.Count + essences.Count + weapons.Count);
		list.AddRange(items);
		list.AddRange(essences);
		list.AddRange(weapons);
		foreach (IGrouping<Rarity, GearReference> item in from item in list
			group item by item.rarity)
		{
			_lockedGears[item.Key] = item.Where((GearReference gear) => gear.obtainable && !gear.unlocked).ToList();
		}
	}

	public GearReference GetGearToUnlock(Random random, Rarity rarity)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		UpdateLockedGearList();
		_ = _lockedGears[rarity];
		if (((IEnumerable<List<GearReference>>)_lockedGears).Select((List<GearReference> list) => list.Count).Sum() == 0)
		{
			return null;
		}
		if (TryGetGearToUnlock(random, rarity, out var gearReference))
		{
			return gearReference;
		}
		List<Rarity> list2 = EnumValues<Rarity>.Values.ToList();
		int num = list2.IndexOf(rarity);
		if ((int)rarity == 0)
		{
			for (int i = 1; i < list2.Count; i++)
			{
				int index = (num + i) % list2.Count;
				if (TryGetGearToUnlock(random, list2[index], out gearReference))
				{
					return gearReference;
				}
			}
		}
		else
		{
			for (int j = 1; j < list2.Count; j++)
			{
				int num2 = num - j;
				if (num2 < 0)
				{
					num2 += list2.Count;
				}
				if (TryGetGearToUnlock(random, list2[num2], out gearReference))
				{
					return gearReference;
				}
			}
		}
		return null;
	}

	private bool TryGetGearToUnlock(Random random, Rarity rarity, out GearReference gearReference)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		gearReference = null;
		List<GearReference> list2 = _lockedGears[rarity];
		if (((IEnumerable<List<GearReference>>)_lockedGears).Select((List<GearReference> list) => list.Count).Sum() == 0)
		{
			return false;
		}
		if (list2.Count == 0)
		{
			return false;
		}
		gearReference = ExtensionMethods.Random<GearReference>((IEnumerable<GearReference>)list2, random);
		return true;
	}

	public GearReference GetGearToTake(Rarity rarity)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		return ExtensionMethods.Random<Gear.Type>((IEnumerable<Gear.Type>)EnumValues<Gear.Type>.Values) switch
		{
			Gear.Type.Item => GetItemToTake(rarity), 
			Gear.Type.Quintessence => GetQuintessenceToTake(rarity), 
			Gear.Type.Weapon => GetWeaponToTake(rarity), 
			_ => null, 
		};
	}

	public ItemReference GetItemToTake(Rarity rarity)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetItemToTake(MMMaths.random, rarity);
	}

	public ItemReference GetItemToTake(Random random, Rarity rarity)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
		{
			return ExtensionMethods.Random<ItemReference>(_items[rarity].Where((ItemReference item) => item.obtainable && item.unlocked), random);
		}
		_ = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
		IEnumerable<ItemReference> enumerable = _items[rarity].Where(delegate(ItemReference item)
		{
			if (!item.obtainable)
			{
				return false;
			}
			if (!item.unlocked)
			{
				return false;
			}
			for (int i = 0; i < _itemInstances.Count; i++)
			{
				Gear gear = _itemInstances[i];
				if (item.name.Equals(((Object)gear).name))
				{
					return false;
				}
				string[] groupItemKeys = gear.groupItemKeys;
				foreach (string value in groupItemKeys)
				{
					if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		});
		if (enumerable.Count() == 0)
		{
			return GetItemToTake(random, (Rarity)(((int)rarity == 0) ? (rarity + 1) : (rarity - 1)));
		}
		return ExtensionMethods.Random<ItemReference>(enumerable, random);
	}

	public ItemReference GetItemToTake(Random random, Gear.Tag tag, bool optainable = true)
	{
		GearResource instance = GearResource.instance;
		if ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
		{
			return ExtensionMethods.Random<ItemReference>(instance.items.Where((ItemReference item) => item.obtainable && item.unlocked && item.gearTag.HasFlag(tag)), random);
		}
		_ = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
		IEnumerable<ItemReference> enumerable = instance.items.Where(delegate(ItemReference item)
		{
			if (optainable && !item.obtainable)
			{
				return false;
			}
			if (!item.unlocked)
			{
				return false;
			}
			if (!item.gearTag.HasFlag(tag))
			{
				return false;
			}
			for (int i = 0; i < _itemInstances.Count; i++)
			{
				Gear gear = _itemInstances[i];
				if (item.name.Equals(((Object)gear).name))
				{
					return false;
				}
				string[] groupItemKeys = gear.groupItemKeys;
				foreach (string value in groupItemKeys)
				{
					if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		});
		if (enumerable.Count() == 0)
		{
			return GetItemToTake(random, (Rarity)0);
		}
		return ExtensionMethods.Random<ItemReference>(enumerable, random);
	}

	public ItemReference GetOmenItems(Random random)
	{
		GearResource instance = GearResource.instance;
		Gear.Tag tag = Gear.Tag.Omen;
		Gear.Tag except = Gear.Tag.UpgradedOmen;
		if ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
		{
			return ExtensionMethods.Random<ItemReference>(instance.items.Where((ItemReference item) => item.obtainable && item.unlocked && item.gearTag.HasFlag(tag)), random);
		}
		_ = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
		IEnumerable<ItemReference> enumerable = instance.items.Where(delegate(ItemReference item)
		{
			if (!item.gearTag.HasFlag(tag))
			{
				return false;
			}
			if (item.gearTag.HasFlag(except))
			{
				return false;
			}
			for (int i = 0; i < _itemInstances.Count; i++)
			{
				Gear gear = _itemInstances[i];
				if (item.name.Equals(((Object)gear).name))
				{
					return false;
				}
				string[] groupItemKeys = gear.groupItemKeys;
				foreach (string value in groupItemKeys)
				{
					if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		});
		if (enumerable.Count() == 0)
		{
			return GetItemToTake(random, (Rarity)0);
		}
		return ExtensionMethods.Random<ItemReference>(enumerable, random);
	}

	public EssenceReference GetQuintessenceToTake(Rarity rarity)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetQuintessenceToTake(MMMaths.random, rarity);
	}

	public EssenceReference GetQuintessenceToTake(Random random, Rarity rarity)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
		{
			return ExtensionMethods.Random<EssenceReference>(_quintessences[rarity].Where((EssenceReference essence) => essence.obtainable && essence.unlocked), random);
		}
		_ = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.quintessence;
		IEnumerable<EssenceReference> enumerable = _quintessences[rarity].Where(delegate(EssenceReference essence)
		{
			if (!essence.obtainable)
			{
				return false;
			}
			if (!essence.unlocked)
			{
				return false;
			}
			for (int i = 0; i < _essenceInstances.Count; i++)
			{
				Gear gear = _essenceInstances[i];
				if (essence.name.Equals(((Object)gear).name))
				{
					return false;
				}
				string[] groupItemKeys = gear.groupItemKeys;
				foreach (string value in groupItemKeys)
				{
					if (essence.name.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		});
		if (enumerable.Count() == 0)
		{
			return GetQuintessenceToTake(random, (Rarity)(rarity - 1));
		}
		return ExtensionMethods.Random<EssenceReference>(enumerable, random);
	}

	private string StripAwakeNumber(string name)
	{
		int num = name.IndexOf('_');
		if (num == -1)
		{
			return name;
		}
		return name.Substring(0, num);
	}

	public ICollection<GearReference> GetGearListByRarity(Gear.Type type, Rarity rarity)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		return type switch
		{
			Gear.Type.Item => GetItemListByRarity(rarity), 
			Gear.Type.Weapon => GetWeaponListByRarity(rarity), 
			Gear.Type.Quintessence => GetEssenceListByRarity(rarity), 
			_ => null, 
		};
	}

	public ICollection<GearReference> GetItemListByRarity(Rarity rarity)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		ICollection<GearReference> collection = new List<GearReference>();
		ItemReference[] array = _items[rarity];
		foreach (ItemReference itemReference in array)
		{
			if (itemReference.obtainable && itemReference.unlocked)
			{
				collection.Add(itemReference);
			}
		}
		return collection;
	}

	public ICollection<GearReference> GetWeaponListByRarity(Rarity rarity)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		ICollection<GearReference> collection = new List<GearReference>();
		WeaponReference[] array = _weapons[rarity];
		foreach (WeaponReference weaponReference in array)
		{
			if (weaponReference.obtainable && weaponReference.unlocked)
			{
				collection.Add(weaponReference);
			}
		}
		return collection;
	}

	public ICollection<GearReference> GetEssenceListByRarity(Rarity rarity)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		ICollection<GearReference> collection = new List<GearReference>();
		EssenceReference[] array = _quintessences[rarity];
		foreach (EssenceReference essenceReference in array)
		{
			if (essenceReference.obtainable && essenceReference.unlocked)
			{
				collection.Add(essenceReference);
			}
		}
		return collection;
	}

	public WeaponReference GetWeaponByName(string name)
	{
		foreach (WeaponReference weapon in GearResource.instance.weapons)
		{
			if (weapon.name.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				return weapon;
			}
		}
		return null;
	}

	public WeaponReference GetWeaponToTake(Rarity rarity)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetWeaponToTake(MMMaths.random, rarity);
	}

	public WeaponReference GetWeaponToTake(Random random, Rarity rarity)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
		{
			return ExtensionMethods.Random<WeaponReference>(_weapons[rarity].Where((WeaponReference weapon) => weapon.obtainable && weapon.unlocked), random);
		}
		_ = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
		IEnumerable<WeaponReference> enumerable = _weapons[rarity].Where(delegate(WeaponReference weapon)
		{
			if (!weapon.obtainable)
			{
				return false;
			}
			if (!weapon.unlocked)
			{
				return false;
			}
			for (int i = 0; i < _weaponInstances.Count; i++)
			{
				if (StripAwakeNumber(weapon.name).Equals(StripAwakeNumber(((Object)_weaponInstances[i]).name)))
				{
					return false;
				}
			}
			return true;
		});
		if (enumerable.Count() == 0)
		{
			return GetWeaponToTake(random, (Rarity)(rarity - 1));
		}
		return ExtensionMethods.Random<WeaponReference>(enumerable, random);
	}

	public WeaponReference GetWeaponByCategory(Random random, Rarity rarity, Weapon.Category category)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		IEnumerable<WeaponReference> enumerable = _weapons[rarity].Where(Pass);
		if (enumerable.Count() == 0)
		{
			return GetWeaponByCategory(random, (Rarity)(rarity - 1), category);
		}
		return ExtensionMethods.Random<WeaponReference>(enumerable, random);
		bool Pass(WeaponReference weapon)
		{
			if (!weapon.obtainable)
			{
				return false;
			}
			if (!weapon.unlocked)
			{
				return false;
			}
			if (weapon.category != category)
			{
				return false;
			}
			for (int i = 0; i < _weaponInstances.Count; i++)
			{
				Gear gear = _weaponInstances[i];
				if (weapon.name.Equals(((Object)gear).name))
				{
					return false;
				}
				for (int j = 0; j < _weaponInstances.Count; j++)
				{
					if (StripAwakeNumber(weapon.name).Equals(StripAwakeNumber(((Object)_weaponInstances[i]).name), StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public ItemReference GetItemByKey(string key)
	{
		foreach (ItemReference[] item in _items)
		{
			IEnumerable<ItemReference> enumerable = item.Where((ItemReference item) => item.name.Equals(key, StringComparison.OrdinalIgnoreCase));
			if (enumerable.Count() != 0)
			{
				ItemReference itemReference = ExtensionMethods.Random<ItemReference>(enumerable);
				if (itemReference != null)
				{
					return itemReference;
				}
			}
		}
		return null;
	}

	public ItemReference GetItemByKeyword(Random random, Rarity rarity, Inscription.Key key)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		IEnumerable<ItemReference> enumerable = _items[rarity].Where(delegate(ItemReference item)
		{
			if (!item.obtainable)
			{
				return false;
			}
			if (!item.unlocked)
			{
				return false;
			}
			string[] keywordRandomizerItems = _keywordRandomizerItems;
			foreach (string value in keywordRandomizerItems)
			{
				if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}
			for (int j = 0; j < _itemInstances.Count; j++)
			{
				Gear gear = _itemInstances[j];
				if (item.name.Equals(((Object)gear).name, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				keywordRandomizerItems = gear.groupItemKeys;
				foreach (string value2 in keywordRandomizerItems)
				{
					if (item.name.Equals(value2, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
			return (item.prefabKeyword1.Equals(key) || item.prefabKeyword2.Equals(key)) ? true : false;
		});
		if (enumerable.Count() == 0)
		{
			return null;
		}
		ItemReference itemReference = ExtensionMethods.Random<ItemReference>(enumerable, random);
		if (itemReference != null)
		{
			return itemReference;
		}
		return null;
	}

	public ICollection<Item> GetItemInstanceByKeyword(Inscription.Key key)
	{
		_itemInstancesCached.Clear();
		foreach (Item itemInstance in _itemInstances)
		{
			if (itemInstance.keyword1 == key || itemInstance.keyword2 == key)
			{
				_itemInstancesCached.Add(itemInstance);
			}
		}
		return _itemInstancesCached;
	}

	public bool CanDrop(Inscription.Key key)
	{
		int num = 0;
		foreach (Item itemInstance in _itemInstances)
		{
			if (!((Object)(object)itemInstance == (Object)null) && itemInstance.obtainable && (itemInstance.keyword1 == key || itemInstance.keyword2 == key))
			{
				num++;
			}
		}
		foreach (ItemReference item in GearResource.instance.items)
		{
			if (item != null && item.obtainable && (item.prefabKeyword1 == key || item.prefabKeyword2 == key))
			{
				num--;
			}
		}
		return num < 0;
	}
}
