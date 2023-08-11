using System.Linq;
using Characters.Gear.Upgrades;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace UI.Upgrades;

public sealed class UpgradedContainer : MonoBehaviour
{
	[SerializeField]
	private UpgradedElement[] _elements;

	public void Set(Panel panel)
	{
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		for (int i = 0; i < _elements.Length; i++)
		{
			_elements[i].Set(panel, ((Object)(object)upgrade.upgrades[i] == (Object)null) ? null : upgrade.upgrades[i].reference);
		}
	}

	public void Append(Panel panel)
	{
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		int num = upgrade.upgrades.Count((UpgradeObject target) => (Object)(object)target != (Object)null);
		for (int i = 0; i < _elements.Length; i++)
		{
			_elements[i].Set(panel, ((Object)(object)upgrade.upgrades[i] == (Object)null) ? null : upgrade.upgrades[i].reference, i == num - 1);
		}
		panel.Focus(_elements[num - 1].selectable);
	}

	public UpgradedElement GetFocusElementOnRemoved(int removedIndex)
	{
		if ((Object)(object)Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.upgrades[removedIndex] != (Object)null)
		{
			return _elements[removedIndex];
		}
		if (removedIndex > 0)
		{
			return _elements[removedIndex - 1];
		}
		return null;
	}

	public UpgradedElement GetDefaultFocusElement()
	{
		UpgradedElement[] elements = _elements;
		foreach (UpgradedElement upgradedElement in elements)
		{
			if (!((Object)(object)upgradedElement == (Object)null) && upgradedElement.reference != null && upgradedElement.reference.type != UpgradeObject.Type.Cursed && Object.op_Implicit((Object)(object)upgradedElement.selectable))
			{
				return upgradedElement;
			}
		}
		return null;
	}
}
