using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Weapons;
using Data;
using GameResources;
using InControl;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.GearPopup;

public class GearPopup : MonoBehaviour
{
	private const float _detailViewThreshold = 0.5f;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _frame;

	[SerializeField]
	private Sprite _frameWithKeywords;

	[SerializeField]
	[Space]
	private RectTransform _rectTransform;

	[SerializeField]
	[Space]
	private GameObject _interactionGuide;

	[SerializeField]
	private TMP_Text _interactionDescription;

	[Space]
	[SerializeField]
	private TMP_Text _name;

	[Space]
	[SerializeField]
	private GameObject _rarityAndCategory;

	[SerializeField]
	private TMP_Text _rarity;

	[SerializeField]
	private GameObject _cooldownIcon;

	[SerializeField]
	private TMP_Text _categoryOrCooldown;

	[SerializeField]
	[Space]
	private TMP_Text _description;

	[Space]
	[SerializeField]
	private GameObject[] _essenceObjects;

	[SerializeField]
	private TMP_Text _essenceActiveName;

	[SerializeField]
	private TMP_Text _essenceActiveDesc;

	[Space]
	[SerializeField]
	private GameObject _extraOptionContainer;

	[SerializeField]
	private Image _extraOption;

	[SerializeField]
	private TMP_Text _extraOptionText;

	[SerializeField]
	[Space]
	private GameObject _extraOptionContainer2;

	[SerializeField]
	private Image _extraOption1;

	[SerializeField]
	private TMP_Text _extraOption1Text;

	[SerializeField]
	private Image _extraOption2;

	[SerializeField]
	private TMP_Text _extraOption2Text;

	[SerializeField]
	private GameObject _viewDetailContainer;

	[SerializeField]
	[Space]
	private GearPopupSkill _skill;

	[SerializeField]
	private GearPopupSkill _skill1;

	[SerializeField]
	private GearPopupSkill _skill2;

	[Space]
	[SerializeField]
	private GearPopupKeywordDetail _keywordDetail1;

	[SerializeField]
	private GearPopupKeywordDetail _keywordDetail2;

	[Space]
	[SerializeField]
	private PressingButton _pressToDestroy;

	[SerializeField]
	[Space]
	private GameObject _detailContainer;

	private InteractiveObject _interactiveObject;

	private const string _omenKey = "synergy/key/Omen/name";

	public RectTransform rectTransform => _rectTransform;

	private static string _interactionLootLabel => Localization.GetLocalizedString("label/interaction/loot");

	private static string _interactionPurcaseLabel => Localization.GetLocalizedString("label/interaction/purchase");

	private void Update()
	{
		if ((Object)(object)_interactiveObject == (Object)null)
		{
			return;
		}
		ProcessDetailView();
		if (!((Object)(object)_pressToDestroy == (Object)null))
		{
			if (_interactiveObject.pressingPercent == 0f)
			{
				_pressToDestroy.StopPressing();
				return;
			}
			_pressToDestroy.PlayPressingSound();
			_pressToDestroy.SetPercent(_interactiveObject.pressingPercent);
		}
	}

	private void ProcessDetailView()
	{
		if (!_detailContainer.activeSelf && ((OneAxisInputControl)KeyMapper.Map.Down).Value > 0.5f)
		{
			_detailContainer.SetActive(true);
		}
		else if (_detailContainer.activeSelf && ((OneAxisInputControl)KeyMapper.Map.Down).Value < 0.5f)
		{
			_detailContainer.SetActive(false);
		}
	}

	private void OnDisable()
	{
		if (!((Object)(object)_pressToDestroy == (Object)null))
		{
			_pressToDestroy.StopPressing();
		}
	}

	private void SetBasic(Gear gear)
	{
		_interactiveObject = gear.dropped;
		_name.text = gear.displayName;
		if (gear.gearTag.HasFlag(Gear.Tag.Omen))
		{
			_rarity.text = Localization.GetLocalizedString("synergy/key/Omen/name");
		}
		else
		{
			_rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", gear.rarity));
		}
		_description.text = gear.description;
		_rarityAndCategory.gameObject.SetActive(true);
		SetInteractionLabel(gear.dropped);
		SetDestructible(gear);
	}

	private void SetDestructible(Gear gear)
	{
		if ((Object)(object)_pressToDestroy == (Object)null)
		{
			return;
		}
		((Component)_pressToDestroy).gameObject.SetActive(gear.destructible);
		if (!gear.destructible)
		{
			return;
		}
		((Component)_pressToDestroy).gameObject.SetActive(true);
		_pressToDestroy.description = Localization.GetLocalizedString("label/inventory/discardItem");
		GameData.Currency currency = GameData.Currency.currencies[gear.currencyTypeByDiscard];
		if (gear.currencyByDiscard > 0)
		{
			if (gear.type == Gear.Type.Quintessence)
			{
				_pressToDestroy.description = _pressToDestroy.description + "( " + Quintessence.currencySpriteKey + " " + $" <color=#{Quintessence.currencyTextColorCode}>{gear.currencyByDiscard}</color> )";
			}
			else
			{
				_pressToDestroy.description = _pressToDestroy.description + "( " + currency.spriteTMPKey + " " + $" <color=#{currency.colorCode}>{(int)((double)gear.currencyByDiscard * currency.multiplier.total)}</color> )";
			}
		}
	}

	private void DisableSetDestructible()
	{
		if (!((Object)(object)_pressToDestroy == (Object)null))
		{
			((Component)_pressToDestroy).gameObject.SetActive(false);
		}
	}

	private void SetEssenceActive(string name, string description)
	{
		GameObject[] essenceObjects = _essenceObjects;
		for (int i = 0; i < essenceObjects.Length; i++)
		{
			essenceObjects[i].SetActive(true);
		}
		_essenceActiveName.text = name;
		_essenceActiveDesc.text = description;
	}

	private void DisableEssenceActive()
	{
		GameObject[] essenceObjects = _essenceObjects;
		for (int i = 0; i < essenceObjects.Length; i++)
		{
			essenceObjects[i].SetActive(false);
		}
	}

	private void DisableExtraOptions()
	{
		_extraOptionContainer.SetActive(false);
		_extraOptionContainer2.SetActive(false);
		_viewDetailContainer.SetActive(false);
		((Component)_skill).gameObject.SetActive(false);
		((Component)_skill1).gameObject.SetActive(false);
		((Component)_skill2).gameObject.SetActive(false);
		((Component)_keywordDetail1).gameObject.SetActive(false);
		((Component)_keywordDetail2).gameObject.SetActive(false);
	}

	public void Set(Gear gear)
	{
		if (!(gear is Weapon weapon))
		{
			if (!(gear is Item item))
			{
				if (gear is Quintessence quintessence)
				{
					Set(quintessence);
				}
			}
			else
			{
				Set(item);
			}
		}
		else
		{
			Set(weapon);
		}
	}

	public void Set(Weapon weapon)
	{
		SetBasic(weapon);
		_cooldownIcon.SetActive(false);
		_categoryOrCooldown.text = weapon.categoryDisplayName;
		SetSkills(weapon);
		DisableEssenceActive();
	}

	private void SetSkills(Weapon weapon)
	{
		_viewDetailContainer.SetActive(true);
		((Component)_keywordDetail1).gameObject.SetActive(false);
		((Component)_keywordDetail2).gameObject.SetActive(false);
		SkillInfo skillInfo = weapon.currentSkills[0];
		Sprite icon = skillInfo.GetIcon();
		string displayName = skillInfo.displayName;
		_extraOption.sprite = icon;
		_extraOptionText.text = displayName;
		_extraOption1.sprite = icon;
		_extraOption1Text.text = displayName;
		if (weapon.currentSkills.Count < 2)
		{
			_skill.Set(skillInfo);
			((Component)_skill).gameObject.SetActive(true);
			((Component)_skill1).gameObject.SetActive(false);
			((Component)_skill2).gameObject.SetActive(false);
			_extraOptionContainer.SetActive(true);
			_extraOptionContainer2.SetActive(false);
		}
		else
		{
			_extraOptionContainer.SetActive(false);
			_extraOptionContainer2.SetActive(true);
			SkillInfo skillInfo2 = weapon.currentSkills[1];
			_extraOption2.sprite = skillInfo2.GetIcon();
			_extraOption2Text.text = skillInfo2.displayName;
			_skill1.Set(skillInfo);
			_skill2.Set(skillInfo2);
			((Component)_skill1).gameObject.SetActive(true);
			((Component)_skill2).gameObject.SetActive(true);
		}
	}

	public void Set(Item item)
	{
		SetBasic(item);
		_cooldownIcon.SetActive(false);
		_categoryOrCooldown.text = string.Empty;
		SetKeywords(item);
		DisableEssenceActive();
	}

	private void SetKeywords(Item item)
	{
		_viewDetailContainer.SetActive(true);
		_extraOptionContainer.SetActive(false);
		_extraOptionContainer2.SetActive(true);
		_extraOption1.sprite = Inscription.GetActiveIcon(item.keyword1);
		_extraOption1Text.text = Inscription.GetName(item.keyword1);
		_extraOption2.sprite = Inscription.GetActiveIcon(item.keyword2);
		_extraOption2Text.text = Inscription.GetName(item.keyword2);
		((Component)_skill).gameObject.SetActive(false);
		((Component)_skill1).gameObject.SetActive(false);
		((Component)_skill2).gameObject.SetActive(false);
		_keywordDetail1.Set(item.keyword1);
		_keywordDetail2.Set(item.keyword2);
		((Component)_keywordDetail1).gameObject.SetActive(true);
		((Component)_keywordDetail2).gameObject.SetActive(true);
	}

	public void Set(Quintessence quintessence)
	{
		SetBasic(quintessence);
		if (quintessence.cooldown.time == null)
		{
			_cooldownIcon.SetActive(false);
			_categoryOrCooldown.text = string.Empty;
		}
		else
		{
			_cooldownIcon.SetActive(true);
			TMP_Text categoryOrCooldown = _categoryOrCooldown;
			float cooldownTime = quintessence.cooldown.time.cooldownTime;
			categoryOrCooldown.text = cooldownTime.ToString();
		}
		SetEssenceActive(quintessence.activeName, quintessence.activeDescription);
		DisableExtraOptions();
	}

	public void Set(string name, string description, string rarity)
	{
		_name.text = name;
		_description.text = description;
		_rarityAndCategory.gameObject.SetActive(true);
		_rarity.text = rarity;
		_cooldownIcon.SetActive(false);
		_categoryOrCooldown.text = string.Empty;
		DisableEssenceActive();
		DisableExtraOptions();
		DisableSetDestructible();
	}

	public void Set(string name, string description)
	{
		Set(name, description, string.Empty);
	}

	public void Set(string name, string description, Rarity rarity)
	{
		Set(name, description, Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", rarity)));
	}

	public void SetInteractionLabel(string interactionLabel)
	{
		_interactiveObject = null;
		if ((Object)(object)_pressToDestroy != (Object)null)
		{
			((Component)_pressToDestroy).gameObject.SetActive(false);
		}
		if (!((Object)(object)_interactionDescription == (Object)null))
		{
			_interactionGuide.SetActive(true);
			_interactionDescription.text = interactionLabel;
		}
	}

	public void SetInteractionLabel(InteractiveObject interactiveObject, string interactionLabel, string pressingInteractionLabel)
	{
		_interactiveObject = interactiveObject;
		if (!((Object)(object)_interactionDescription == (Object)null))
		{
			_interactionDescription.text = interactionLabel;
			if (!((Object)(object)_pressToDestroy == (Object)null))
			{
				((Component)_pressToDestroy).gameObject.SetActive(true);
				_pressToDestroy.description = pressingInteractionLabel;
			}
		}
	}

	public void SetInteractionLabel(DroppedGear dropped)
	{
		if ((Object)(object)_interactionDescription == (Object)null)
		{
			return;
		}
		if ((Object)(object)dropped.gear != (Object)null && !dropped.gear.lootable)
		{
			_interactionGuide.SetActive(false);
			return;
		}
		_interactionGuide.SetActive(true);
		if (dropped.price > 0)
		{
			SetInteractionLabelAsPurchase(dropped.priceCurrency, dropped.price);
		}
		else
		{
			SetInteractionLabelAsLoot();
		}
	}

	public void SetInteractionLabelAsLoot()
	{
		_interactionDescription.text = _interactionLootLabel;
	}

	public void SetInteractionLabelAsPurchase(GameData.Currency.Type currencyType, int price)
	{
		GameData.Currency currency = GameData.Currency.currencies[currencyType];
		string spriteTMPKey = currency.spriteTMPKey;
		string arg = currency.colorCode;
		string format = _interactionPurcaseLabel;
		if (price == 0)
		{
			format = _interactionLootLabel;
		}
		if (!currency.Has(price))
		{
			arg = GameData.Currency.noMoneyColorCode;
		}
		string arg2 = $" {spriteTMPKey}  <color=#{arg}>{price}</color> ";
		_interactionDescription.text = string.Format(format, arg2);
	}

	public void SetInteractionLabelAsPurchase(string label, GameData.Currency.Type currencyType, int price)
	{
		GameData.Currency currency = GameData.Currency.currencies[currencyType];
		string spriteTMPKey = currency.spriteTMPKey;
		string arg = currency.colorCode;
		if (!currency.Has(price))
		{
			arg = GameData.Currency.noMoneyColorCode;
		}
		string arg2 = $" {spriteTMPKey}  <color=#{arg}>{price}</color> ";
		_interactionDescription.text = $"{label}({arg2})";
	}
}
