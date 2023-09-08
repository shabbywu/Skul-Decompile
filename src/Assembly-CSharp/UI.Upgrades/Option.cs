using Characters.Gear.Upgrades;
using Characters.Player;
using GameResources;
using InControl;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.Upgrades;

public sealed class Option : MonoBehaviour
{
	[SerializeField]
	private Image _riskPanel;

	[SerializeField]
	private Image _riskFrame;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _type;

	[SerializeField]
	private Level _level;

	[SerializeField]
	private TMP_Text _description;

	[SerializeField]
	private TMP_Text _cost;

	[SerializeField]
	private Confirm _removeConfirm;

	[Header("Bottom")]
	[SerializeField]
	private GameObject _select;

	[SerializeField]
	private TMP_Text _selectText;

	[SerializeField]
	private Sprite _removeNormal;

	[SerializeField]
	private Sprite _removeRisky;

	[SerializeField]
	private Image _remove;

	[SerializeField]
	private TMP_Text _removeText;

	private UpgradedElement _currentUpgradedElement;

	private UpgradeElement _currentElement;

	private UpgradeResource.Reference _currentReference;

	private Panel _panel;

	private Color _priceColor;

	private string _noMoneyColorCode;

	private const string originalNameColorCode = "#8B36F3";

	private const string originalDescColorCode = "#764CC5";

	private const string riskNameColorCode = "#D6385E";

	private const string riskDescColorCode = "#992555";

	public void Initialize(Panel panel)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_panel = panel;
		_noMoneyColorCode = ColorUtility.ToHtmlStringRGB(Color.red);
		Singleton<UpgradeShop>.Instance.onUpgraded += HandleOnLevelUp;
		Singleton<UpgradeShop>.Instance.onLevelUp += HandleOnLevelUp;
	}

	private void HandleOnLevelUp(UpgradeResource.Reference reference)
	{
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		int num = upgrade.IndexOf(reference);
		if (num >= 0)
		{
			UpgradeObject upgradeObject = upgrade.upgrades[num];
			_level.LevelUp(upgradeObject.level);
		}
	}

	public void Set(UpgradedElement element)
	{
		_currentUpgradedElement = element;
		_currentReference = _currentUpgradedElement.reference;
		UpdateData();
	}

	public void Set(UpgradeElement element)
	{
		_currentElement = element;
		_currentReference = _currentElement.reference;
		UpdateData();
	}

	public void UpdateData()
	{
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		if (_currentReference == null)
		{
			_icon.sprite = null;
			_name.text = string.Empty;
			_type.text = string.Empty;
			return;
		}
		_icon.sprite = _currentReference.icon;
		_name.text = _currentReference.displayName;
		if (_currentReference.type == UpgradeObject.Type.Cursed)
		{
			_type.text = UpgradeResource.Reference.curseText;
		}
		else
		{
			_type.text = string.Empty;
		}
		SetFrame();
		int maxLevel = _currentReference.maxLevel;
		int currentLevel = _currentReference.GetCurrentLevel();
		_level.Set(currentLevel, maxLevel, _currentReference.type == UpgradeObject.Type.Cursed, flick: true);
		_description.text = _currentReference.GetNextLevelDescription();
		bool flag = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.IndexOf(_currentReference) != -1;
		string text = (Singleton<UpgradeShop>.Instance.saleCurrency.Has(_currentReference.price) ? Singleton<UpgradeShop>.Instance.saleCurrency.colorCode : _noMoneyColorCode);
		_select.SetActive(currentLevel < maxLevel);
		string localizedString = Localization.GetLocalizedString("label/get");
		_selectText.text = $"{localizedString} ({Singleton<UpgradeShop>.Instance.saleCurrency.spriteTMPKey} <color=#{text}>{_currentReference.price}</color>)";
		((Component)_remove).gameObject.SetActive(flag && _currentReference.removable);
		_remove.sprite = ((_currentReference.type == UpgradeObject.Type.Cursed) ? _removeRisky : _removeNormal);
		if (flag && _currentReference.removable)
		{
			int removeCost = Singleton<UpgradeShop>.Instance.GetRemoveCost(_currentReference.type);
			string text2 = (Singleton<UpgradeShop>.Instance.removeCurrency.Has(removeCost) ? ColorUtility.ToHtmlStringRGB(Color.yellow) : ColorUtility.ToHtmlStringRGB(Color.red));
			string localizedString2 = Localization.GetLocalizedString("label/interaction/destroy");
			_removeText.text = $"{localizedString2} ({Singleton<UpgradeShop>.Instance.removeCurrency.spriteTMPKey} <color=#{text2}>{removeCost}</color>)";
		}
	}

	private void SetFrame()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		Image riskFrame = _riskFrame;
		bool enabled = (((Behaviour)_riskPanel).enabled = _currentReference.type == UpgradeObject.Type.Cursed);
		((Behaviour)riskFrame).enabled = enabled;
		if (_currentReference.type == UpgradeObject.Type.Cursed)
		{
			Color color = default(Color);
			ColorUtility.TryParseHtmlString("#D6385E", ref color);
			Color color2 = default(Color);
			ColorUtility.TryParseHtmlString("#992555", ref color2);
			((Graphic)_description).color = color2;
			((Graphic)_name).color = color;
		}
		else
		{
			Color color3 = default(Color);
			ColorUtility.TryParseHtmlString("#8B36F3", ref color3);
			Color color4 = default(Color);
			ColorUtility.TryParseHtmlString("#764CC5", ref color4);
			((Graphic)_description).color = color4;
			((Graphic)_name).color = color3;
		}
	}

	private void OpenRemoveConfirmText()
	{
		if (!Singleton<UpgradeShop>.Instance.CheckRemovable(_currentReference))
		{
			if ((Object)(object)_currentUpgradedElement != (Object)null)
			{
				_currentUpgradedElement.PlayFailEffect();
			}
			return;
		}
		UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
		int num = upgrade.IndexOf(_currentReference);
		if (num != -1)
		{
			UpgradeObject upgradeObject = upgrade.upgrades[num];
			string text = string.Format(Localization.GetLocalizedString("label/upgrade/destroy"), Singleton<UpgradeShop>.Instance.GetRemoveCost(_currentReference.type), upgradeObject.returnCost);
			_removeConfirm.Open(text, RemoveUpgrade);
		}
	}

	private void RemoveUpgrade()
	{
		int index = 0;
		if (!Singleton<UpgradeShop>.Instance.TryRemoveUpgradeObjet(_currentReference, ref index) && (Object)(object)_currentUpgradedElement != (Object)null)
		{
			_currentUpgradedElement.PlayFailEffect();
		}
		UpgradeElement defaultFocusTarget = _panel.upgradableContainer.GetDefaultFocusTarget();
		_panel.UpdateUpgradedList();
		_panel.SetFocusOnRemoved(index);
		_currentElement = defaultFocusTarget;
		_currentReference = _currentElement.reference;
		UpdateData();
	}

	private void Update()
	{
		if (_currentReference != null && ((OneAxisInputControl)KeyMapper.Map.UiInteraction1).WasPressed && _currentReference != null && _currentReference.removable && Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(_currentReference))
		{
			OpenRemoveConfirmText();
		}
	}
}
