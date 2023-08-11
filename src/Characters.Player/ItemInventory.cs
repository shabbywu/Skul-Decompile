using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities.Spirits;
using Characters.Gear;
using Characters.Gear.Items;
using GameResources;
using UnityEngine;

namespace Characters.Player;

public class ItemInventory : MonoBehaviour
{
	public const int itemCount = 9;

	[SerializeField]
	[GetComponent]
	private Character _character;

	[SerializeField]
	private Transform[] _slots;

	private List<Spirit> _spirits;

	public List<Item> items { get; } = new List<Item> { null, null, null, null, null, null, null, null, null };


	public event Action onChanged;

	private void Awake()
	{
		_spirits = new List<Spirit>(_slots.Length);
	}

	public int IndexOf(Item item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if ((Object)(object)items[i] == (Object)(object)item)
			{
				return i;
			}
		}
		return -1;
	}

	public void Trim()
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if ((Object)(object)items[i] == (Object)null)
			{
				num++;
			}
			else
			{
				ExtensionMethods.Swap<Item>((IList<Item>)items, i, i - num);
			}
		}
	}

	public bool TryEquip(Item item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if ((Object)(object)items[i] == (Object)null)
			{
				EquipAt(item, i);
				return true;
			}
		}
		return false;
	}

	public void RemoveAll()
	{
		for (int i = 0; i < items.Count; i++)
		{
			Remove(i);
		}
	}

	public void Remove(Item item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (!((Object)(object)items[i] == (Object)null) && !((Object)(object)items[i] != (Object)(object)item))
			{
				Remove(i);
			}
		}
	}

	public bool Drop(int index)
	{
		Item item = items[index];
		if ((Object)(object)item == (Object)null)
		{
			return false;
		}
		_character.stat.DetachValues(item.stat);
		item.state = Characters.Gear.Gear.State.Dropped;
		items[index] = null;
		this.onChanged?.Invoke();
		return true;
	}

	public bool Remove(int index)
	{
		Item item = items[index];
		if (!Drop(index))
		{
			return false;
		}
		item.destructible = false;
		Object.Destroy((Object)(object)((Component)item).gameObject);
		return true;
	}

	public bool Discard(Item item)
	{
		int num = IndexOf(item);
		if (num == -1)
		{
			return false;
		}
		return Discard(num);
	}

	public bool Discard(int index)
	{
		Item item = items[index];
		if (!Drop(index))
		{
			return false;
		}
		Object.Destroy((Object)(object)((Component)item).gameObject);
		return true;
	}

	public void EquipAt(Item item, int index)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Drop(index);
		((Component)item).transform.parent = _character.@base;
		((Component)item).transform.localPosition = Vector3.zero;
		_character.stat.AttachValues(item.stat);
		items[index] = item;
		item.SetOwner(_character);
		item.state = Characters.Gear.Gear.State.Equipped;
		this.onChanged?.Invoke();
	}

	public void Change(Item old, Item @new)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (!((Object)(object)items[i] == (Object)null) && !((Object)(object)items[i] != (Object)(object)old))
			{
				ChangeAt(@new, i);
			}
		}
	}

	public void ChangeAt(Item @new, int index)
	{
		Remove(index);
		EquipAt(@new, index);
	}

	public void AttachSpirit(Spirit spirit)
	{
		_spirits.Add(spirit);
		SortSpiritPositions();
	}

	public void DetachSpirit(Spirit spirit)
	{
		_spirits.Remove(spirit);
		SortSpiritPositions();
	}

	private void SortSpiritPositions()
	{
		for (int i = 0; i < _spirits.Count; i++)
		{
			_spirits[i].targetPosition = _slots[i];
		}
	}

	public int GetItemCountByRarity(Rarity rarity)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (!((Object)(object)items[i] == (Object)null) && items[i].rarity == rarity)
			{
				num++;
			}
		}
		return num;
	}

	public int GetItemCountByTag(Characters.Gear.Gear.Tag tag)
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (!((Object)(object)items[i] == (Object)null) && items[i].gearTag.HasFlag(tag))
			{
				num++;
			}
		}
		return num;
	}

	public Item GetRandomItem(Random random)
	{
		int num = items.Where((Item item) => (Object)(object)item != (Object)null).Count();
		if (num == 0)
		{
			return null;
		}
		int index = random.Next(0, num);
		return items[index];
	}

	public bool Has(Item item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (!((Object)(object)items[i] == (Object)null) && ((Object)items[i]).name.Equals(((Object)item).name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public bool Has(string guid)
	{
		if (!GearResource.instance.TryGetItemReferenceByGuid(guid, out var reference))
		{
			return false;
		}
		for (int i = 0; i < items.Count; i++)
		{
			if (!((Object)(object)items[i] == (Object)null) && ((Object)items[i]).name.Equals(reference.name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasGroup(Item item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if ((Object)(object)items[i] == (Object)null)
			{
				continue;
			}
			if (((Object)items[i]).name.Equals(((Object)item).name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			string[] groupItemKeys = items[i].groupItemKeys;
			foreach (string value in groupItemKeys)
			{
				if (((Object)item).name.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}
}
