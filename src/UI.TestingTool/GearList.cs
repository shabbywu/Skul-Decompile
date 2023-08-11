using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Upgrades;
using Characters.Player;
using GameResources;
using InControl;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool;

public class GearList : MonoBehaviour
{
	private enum Filter
	{
		Weapon,
		Item,
		Essence,
		Upgrade
	}

	[SerializeField]
	private GearListElement _gearListElementPrefab;

	[SerializeField]
	private Button _upgradeListElement;

	[SerializeField]
	private Button _inscriptionElementPrefab;

	[SerializeField]
	private Button _head;

	[SerializeField]
	private Button _item;

	[SerializeField]
	private Button _essence;

	[SerializeField]
	private Button _upgrade;

	[SerializeField]
	private TMP_InputField _inputField;

	[SerializeField]
	private Transform _gridContainer;

	private Filter _currentFilter;

	private readonly List<GearListElement> _gearListElements = new List<GearListElement>();

	private readonly List<Button> _upgradeListElements = new List<Button>();

	private readonly List<Button> _inscriptionListElements = new List<Button>();

	private List<ItemReference> selected;

	private void Awake()
	{
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		selected = new List<ItemReference>();
		foreach (WeaponReference weapon in GearResource.instance.weapons)
		{
			GearListElement gearListElement = Object.Instantiate<GearListElement>(_gearListElementPrefab, _gridContainer);
			gearListElement.Set(weapon);
			_gearListElements.Add(gearListElement);
		}
		foreach (ItemReference item in GearResource.instance.items)
		{
			GearListElement gearListElement2 = Object.Instantiate<GearListElement>(_gearListElementPrefab, _gridContainer);
			gearListElement2.Set(item);
			_gearListElements.Add(gearListElement2);
		}
		foreach (EssenceReference essence in GearResource.instance.essences)
		{
			GearListElement gearListElement3 = Object.Instantiate<GearListElement>(_gearListElementPrefab, _gridContainer);
			gearListElement3.Set(essence);
			_gearListElements.Add(gearListElement3);
		}
		((UnityEvent)_head.onClick).AddListener((UnityAction)delegate
		{
			SetFilter(Filter.Weapon);
		});
		((UnityEvent)_item.onClick).AddListener((UnityAction)delegate
		{
			SetFilter(Filter.Item);
		});
		((UnityEvent)_essence.onClick).AddListener((UnityAction)delegate
		{
			SetFilter(Filter.Essence);
		});
		((UnityEvent)_upgrade.onClick).AddListener((UnityAction)delegate
		{
			SetFilter(Filter.Upgrade);
		});
		_ = Singleton<Service>.Instance.levelManager.player;
		foreach (UpgradeResource.Reference upgrade in UpgradeResource.instance.upgradeReferences)
		{
			Button val = Object.Instantiate<Button>(_upgradeListElement, _gridContainer);
			Text componentInChildren = ((Component)val).GetComponentInChildren<Text>();
			((UnityEvent)val.onClick).AddListener((UnityAction)delegate
			{
				UpgradeInventory upgrade2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
				if (!upgrade2.Has(upgrade))
				{
					if (!upgrade2.TryEquip(upgrade))
					{
						Debug.LogError((object)"획득 실패");
					}
				}
				else
				{
					int index = upgrade2.IndexOf(upgrade);
					upgrade2.upgrades[index].LevelUpOrigin();
				}
			});
			if ((Object)(object)componentInChildren != (Object)null)
			{
				componentInChildren.text = upgrade.displayName;
			}
			_upgradeListElements.Add(val);
		}
		foreach (Inscription.Key inscription in Enum.GetValues(typeof(Inscription.Key)))
		{
			Button val2 = Object.Instantiate<Button>(_inscriptionElementPrefab, _gridContainer);
			Text componentInChildren2 = ((Component)val2).GetComponentInChildren<Text>();
			((UnityEvent)val2.onClick).AddListener((UnityAction)delegate
			{
				SetInscription(inscription);
			});
			if ((Object)(object)componentInChildren2 != (Object)null)
			{
				componentInChildren2.text = inscription.ToString();
			}
			_inscriptionListElements.Add(val2);
		}
		((UnityEvent<string>)(object)_inputField.onValueChanged).AddListener((UnityAction<string>)delegate
		{
			FilterGearList();
		});
		SetFilter(Filter.Weapon);
	}

	private void Update()
	{
		InputDevice activeDevice = InputManager.ActiveDevice;
		if (((OneAxisInputControl)activeDevice.LeftBumper).WasPressed || ((OneAxisInputControl)activeDevice.LeftTrigger).WasPressed)
		{
			if (_currentFilter == Filter.Weapon)
			{
				SetFilter(EnumValues<Filter>.Values.Last());
			}
			else
			{
				SetFilter(_currentFilter - 1);
			}
		}
		else if (((OneAxisInputControl)activeDevice.RightBumper).WasPressed || ((OneAxisInputControl)activeDevice.RightTrigger).WasPressed)
		{
			if (_currentFilter == EnumValues<Filter>.Values.Last())
			{
				SetFilter(EnumValues<Filter>.Values.First());
			}
			else
			{
				SetFilter(_currentFilter + 1);
			}
		}
	}

	private void SetFilter(Filter filter)
	{
		_currentFilter = filter;
		switch (filter)
		{
		case Filter.Weapon:
			FilterGearList(Gear.Type.Weapon);
			break;
		case Filter.Item:
			FilterGearList(Gear.Type.Item);
			break;
		case Filter.Essence:
			FilterGearList(Gear.Type.Quintessence);
			break;
		case Filter.Upgrade:
			FilterUpgrade();
			break;
		}
	}

	private void FilterGearList(Gear.Type? type = null)
	{
		string value = _inputField.text.Trim().ToUpperInvariant();
		foreach (Button upgradeListElement in _upgradeListElements)
		{
			((Component)upgradeListElement).gameObject.SetActive(false);
		}
		foreach (Button inscriptionListElement in _inscriptionListElements)
		{
			((Component)inscriptionListElement).gameObject.SetActive(type == Gear.Type.Item);
		}
		foreach (GearListElement gearListElement in _gearListElements)
		{
			bool flag = string.IsNullOrEmpty(value) || gearListElement.text.ToUpperInvariant().Contains(value) || gearListElement.gearReference.name.ToUpperInvariant().Contains(value);
			if (type.HasValue)
			{
				flag &= gearListElement.type == type;
			}
			((Component)gearListElement).gameObject.SetActive(flag);
		}
	}

	private void FilterUpgrade()
	{
		string value = _inputField.text.Trim().ToUpperInvariant();
		foreach (Button inscriptionListElement in _inscriptionListElements)
		{
			((Component)inscriptionListElement).gameObject.SetActive(false);
		}
		foreach (GearListElement gearListElement in _gearListElements)
		{
			((Component)gearListElement).gameObject.SetActive(false);
		}
		foreach (Button upgradeListElement in _upgradeListElements)
		{
			bool active = string.IsNullOrEmpty(value) || ((Object)upgradeListElement).name.ToUpperInvariant().Contains(value);
			((Component)upgradeListElement).gameObject.SetActive(active);
		}
	}

	private void SetInscription(Inscription.Key keyword)
	{
		selected.Clear();
		foreach (ItemReference item in GearResource.instance.items)
		{
			if (item.prefabKeyword1 == keyword || item.prefabKeyword2 == keyword)
			{
				selected.Add(item);
			}
		}
		foreach (GearListElement gearListElement in _gearListElements)
		{
			bool active = false;
			if (gearListElement.gearReference.type == Gear.Type.Item)
			{
				foreach (ItemReference item2 in selected)
				{
					if (gearListElement.gearReference.name.Equals(item2.name, StringComparison.OrdinalIgnoreCase))
					{
						active = true;
						break;
					}
				}
			}
			((Component)gearListElement).gameObject.SetActive(active);
		}
	}
}
