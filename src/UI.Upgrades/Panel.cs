using Data;
using InControl;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInput;

namespace UI.Upgrades;

public sealed class Panel : Dialogue
{
	[SerializeField]
	private UpgradedContainer _upgradedList;

	[SerializeField]
	private UpgradableContainer _upgradableList;

	[SerializeField]
	private Option _current;

	[Header("Left Bottom")]
	[SerializeField]
	private TMP_Text _currency;

	[SerializeField]
	private TMP_Text _gold;

	private int _currencyCache;

	private int _goldCache;

	public UpgradableContainer upgradableContainer => _upgradableList;

	public override bool closeWithPauseKey => true;

	private void Awake()
	{
		_upgradableList.Initialize(this);
		_current.Initialize(this);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		UpdateAll();
		Selectable selectable = _upgradableList.GetDefaultFocusTarget().selectable;
		EventSystem.current.SetSelectedGameObject(((Component)selectable).gameObject);
		selectable.Select();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		LetterBox.instance.Disappear();
	}

	protected override void Update()
	{
		base.Update();
		if (_currencyCache != GameData.Currency.heartQuartz.balance)
		{
			_currency.text = GameData.Currency.heartQuartz.balance.ToString();
			_currencyCache = GameData.Currency.heartQuartz.balance;
		}
		if (_goldCache != GameData.Currency.gold.balance)
		{
			_gold.text = GameData.Currency.gold.balance.ToString();
			_goldCache = GameData.Currency.gold.balance;
		}
		if (((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed && (Object)(object)Dialogue.GetCurrent() == (Object)(object)this)
		{
			Close();
		}
	}

	private void UpdateAll()
	{
		UpdateUpgradedList();
		_upgradableList.UpdateElements();
	}

	public void UpdateUpgradedList()
	{
		_upgradedList.Set(this);
	}

	public void AppendToUpgradedList()
	{
		_upgradedList.Append(this);
	}

	public void UpdateCurrentOption(UpgradeElement element)
	{
		_current.Set(element);
	}

	public void UpdateCurrentOption(UpgradedElement element)
	{
		_current.Set(element);
	}

	public void SetFocusOnRemoved(int removedIndex)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		UpgradedElement focusElementOnRemoved = _upgradedList.GetFocusElementOnRemoved(removedIndex);
		if ((Object)(object)focusElementOnRemoved == (Object)null)
		{
			SetFocusToDefault();
		}
		else
		{
			Focus(focusElementOnRemoved.selectable);
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(upgradableContainer.clearSoundInfo, ((Component)this).gameObject.transform.position);
	}

	public void SetFocusToDefault()
	{
		_defaultFocus = _upgradableList.GetDefaultFocusTarget().selectable;
		Focus();
	}
}
