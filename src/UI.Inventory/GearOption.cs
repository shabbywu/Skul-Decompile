using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public class GearOption : MonoBehaviour
{
	[SerializeField]
	private WeaponOption _weaponOption;

	[SerializeField]
	private ItemOption _itemOption;

	[SerializeField]
	private QuintessenceOption _essenceOption;

	[SerializeField]
	private UpgradeOption _upgradeOption;

	[SerializeField]
	[Space]
	private Image _thumnailIcon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _rarity;

	[SerializeField]
	[Space]
	private PressingButton _itemDiscardKey;

	[SerializeField]
	private TMP_Text _itemDiscardText;

	[SerializeField]
	private GameObject _skillSwapKey;

	public void Clear()
	{
		((Behaviour)_thumnailIcon).enabled = false;
		_name.text = string.Empty;
		_rarity.text = string.Empty;
		((Component)_itemDiscardKey).gameObject.SetActive(false);
		_skillSwapKey.SetActive(false);
		((Component)_weaponOption).gameObject.SetActive(false);
		((Component)_itemOption).gameObject.SetActive(false);
		((Component)_essenceOption).gameObject.SetActive(false);
		((Component)_upgradeOption).gameObject.SetActive(false);
	}

	public void Set(Weapon weapon)
	{
		Clear();
		((Component)_weaponOption).gameObject.SetActive(true);
		_weaponOption.Set(weapon);
	}

	public void Set(Item item)
	{
		Clear();
		((Component)_itemOption).gameObject.SetActive(true);
		_itemOption.Set(item);
	}

	public void Set(Quintessence essence)
	{
		Clear();
		((Component)_essenceOption).gameObject.SetActive(true);
		_essenceOption.Set(essence);
	}

	public void Set(UpgradeObject item)
	{
		Clear();
		((Component)_upgradeOption).gameObject.SetActive(true);
		_upgradeOption.Set(item);
	}
}
