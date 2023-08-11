using System;
using System.Collections.Generic;
using Characters.Controllers;
using Characters.Gear;
using Characters.Gear.Quintessences;
using UnityEngine;

namespace Characters.Player;

public class QuintessenceInventory : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Character _character;

	[SerializeField]
	[GetComponent]
	private PlayerInput _input;

	public List<Quintessence> items { get; } = new List<Quintessence> { null };


	public event Action onChanged;

	private void Trim()
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
				ExtensionMethods.Swap<Quintessence>((IList<Quintessence>)items, i, i - num);
			}
		}
	}

	public void UseAt(int index)
	{
		Quintessence quintessence = items[index];
		if ((Object)(object)quintessence != (Object)null)
		{
			quintessence.Use();
		}
	}

	public bool TryEquip(Quintessence item)
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

	public void EquipAt(Quintessence item, int index)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		Quintessence quintessence = items[index];
		if ((Object)(object)quintessence != (Object)null)
		{
			_character.stat.DetachValues(quintessence.stat);
			quintessence.state = Characters.Gear.Gear.State.Dropped;
		}
		quintessence = item;
		((Component)quintessence).transform.parent = _character.@base;
		((Component)quintessence).transform.localPosition = Vector3.zero;
		_character.stat.AttachValues(quintessence.stat);
		items[index] = quintessence;
		this.onChanged?.Invoke();
		item.SetOwner(_character);
		item.state = Characters.Gear.Gear.State.Equipped;
	}

	public bool Drop(int index)
	{
		Quintessence quintessence = items[index];
		if ((Object)(object)quintessence == (Object)null)
		{
			return false;
		}
		_character.stat.DetachValues(quintessence.stat);
		quintessence.state = Characters.Gear.Gear.State.Dropped;
		items[index] = null;
		this.onChanged?.Invoke();
		return true;
	}

	public bool Remove(int index)
	{
		Quintessence quintessence = items[index];
		if (!Drop(index))
		{
			return false;
		}
		quintessence.destructible = false;
		Object.Destroy((Object)(object)((Component)quintessence).gameObject);
		return true;
	}

	public void RemoveAll()
	{
		for (int i = 0; i < items.Count; i++)
		{
			Remove(i);
		}
	}

	public bool Discard(int index)
	{
		Quintessence quintessence = items[index];
		if (!Drop(index))
		{
			return false;
		}
		Object.Destroy((Object)(object)((Component)quintessence).gameObject);
		return true;
	}

	public void Change(Quintessence old, Quintessence @new)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (!((Object)(object)items[i] == (Object)null) && ((Object)items[i]).name.Equals(((Object)old).name, StringComparison.OrdinalIgnoreCase))
			{
				ChangeAt(@new, i);
			}
		}
	}

	public void ChangeAt(Quintessence @new, int index)
	{
		Remove(index);
		EquipAt(@new, index);
	}

	public int GetCountByRarity(Rarity rarity)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (Quintessence item in items)
		{
			if (!((Object)(object)item == (Object)null) && item.rarity == rarity)
			{
				num++;
			}
		}
		return num;
	}
}
