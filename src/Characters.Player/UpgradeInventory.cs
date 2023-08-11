using System;
using System.Collections.Generic;
using Characters.Gear.Upgrades;
using Platforms;
using UnityEngine;

namespace Characters.Player;

public sealed class UpgradeInventory : MonoBehaviour
{
	public const int upgradeCount = 4;

	[SerializeField]
	[GetComponent]
	private Character _character;

	public List<UpgradeObject> upgrades { get; } = new List<UpgradeObject> { null, null, null, null };


	public List<UpgradeObject> consumableUpgrades { get; } = new List<UpgradeObject>();


	public event Action onChanged;

	public int IndexOf(UpgradeObject upgrade)
	{
		for (int i = 0; i < upgrades.Count; i++)
		{
			if (!((Object)(object)upgrades[i] == (Object)null) && upgrades[i].reference.name.Equals(upgrade.reference.name, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}
		return -1;
	}

	public int IndexOf(UpgradeResource.Reference reference)
	{
		for (int i = 0; i < upgrades.Count; i++)
		{
			if (!((Object)(object)upgrades[i] == (Object)null) && reference.name.Equals(((Object)upgrades[i]).name, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}
		return -1;
	}

	public void Trim()
	{
		int num = 0;
		for (int i = 0; i < upgrades.Count; i++)
		{
			if ((Object)(object)upgrades[i] == (Object)null)
			{
				num++;
			}
			else
			{
				ExtensionMethods.Swap<UpgradeObject>((IList<UpgradeObject>)upgrades, i, i - num);
			}
		}
	}

	public bool TryEquip(UpgradeObject upgrade)
	{
		for (int i = 0; i < upgrades.Count; i++)
		{
			if ((Object)(object)upgrades[i] == (Object)null)
			{
				EquipAt(upgrade, i);
				return true;
			}
			if (((Object)upgrades[i]).name.Equals(((Object)upgrade).name, StringComparison.OrdinalIgnoreCase))
			{
				EquipAt(upgrade, i);
				return true;
			}
		}
		Debug.LogError((object)"획득 실패!");
		return false;
	}

	public bool TryEquip(UpgradeResource.Reference reference)
	{
		for (int i = 0; i < upgrades.Count; i++)
		{
			if ((Object)(object)upgrades[i] == (Object)null)
			{
				EquipAt(reference, i);
				return true;
			}
			if (reference.name.Equals(((Object)upgrades[i]).name, StringComparison.OrdinalIgnoreCase))
			{
				EquipAt(reference, i);
				return true;
			}
		}
		Debug.LogError((object)"획득 실패!");
		return false;
	}

	public void RemoveAll()
	{
		for (int i = 0; i < upgrades.Count; i++)
		{
			Remove(i);
		}
	}

	public bool Remove(int index)
	{
		UpgradeObject upgradeObject = upgrades[index];
		if (!Object.op_Implicit((Object)(object)upgradeObject))
		{
			return false;
		}
		Object.Destroy((Object)(object)((Component)upgradeObject).gameObject);
		upgrades[index] = null;
		return true;
	}

	public void EquipAt(UpgradeResource.Reference upgrade, int index)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Remove(index);
		UpgradeObject upgradeObject = upgrade.Instantiate();
		((Component)upgradeObject).transform.parent = _character.@base;
		((Component)upgradeObject).transform.localPosition = Vector3.zero;
		((Object)((Component)upgradeObject).gameObject).name = upgrade.name;
		upgrades[index] = upgradeObject;
		this.onChanged?.Invoke();
		upgradeObject.Attach(_character);
		CheckBestConditionAcheivement();
		CheckCursedOneAcheivement();
	}

	public void EquipAt(UpgradeObject upgrade, int index)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Remove(index);
		UpgradeObject upgradeObject = Object.Instantiate<UpgradeObject>(upgrade);
		((Component)upgradeObject).transform.parent = _character.@base;
		((Component)upgradeObject).transform.localPosition = Vector3.zero;
		((Object)((Component)upgradeObject).gameObject).name = ((Object)((Component)upgrade).gameObject).name;
		upgrades[index] = upgradeObject;
		this.onChanged?.Invoke();
		upgradeObject.Attach(_character);
		CheckBestConditionAcheivement();
		CheckCursedOneAcheivement();
	}

	public bool Has(UpgradeResource.Reference reference)
	{
		for (int i = 0; i < upgrades.Count; i++)
		{
			if (!((Object)(object)upgrades[i] == (Object)null) && upgrades[i].reference.name.Equals(reference.name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public bool Has(UpgradeObject upgrade)
	{
		for (int i = 0; i < upgrades.Count; i++)
		{
			if (!((Object)(object)upgrades[i] == (Object)null) && upgrades[i].reference.name.Equals(upgrade.reference.name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private void CheckBestConditionAcheivement()
	{
		for (int i = 0; i < upgrades.Count && !((Object)(object)upgrades[i] == (Object)null); i++)
		{
			if (i == upgrades.Count - 1)
			{
				ExtensionMethods.Set((Type)64);
			}
		}
	}

	private void CheckCursedOneAcheivement()
	{
		for (int i = 0; i < upgrades.Count && !((Object)(object)upgrades[i] == (Object)null) && upgrades[i].type == UpgradeObject.Type.Cursed; i++)
		{
			if (i == upgrades.Count - 1)
			{
				ExtensionMethods.Set((Type)65);
			}
		}
	}
}
