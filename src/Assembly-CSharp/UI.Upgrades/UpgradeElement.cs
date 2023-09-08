using System.Collections;
using Characters.Gear;
using Characters.Gear.Upgrades;
using Data;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Upgrades;

public sealed class UpgradeElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	[SerializeField]
	private Button _button;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _deactiveMask;

	[SerializeField]
	private Sprite _normalFindSprite;

	[SerializeField]
	private Sprite _curseFindSprite;

	[SerializeField]
	private Image _findFrame;

	[SerializeField]
	private GameObject _findEffect;

	[SerializeField]
	private GameObject _failEffect;

	[SerializeField]
	private SelectionSpriteSwapper _selectionSpriteSwapper;

	private UpgradeResource.Reference _reference;

	private Panel _panel;

	public Selectable selectable => (Selectable)(object)_button;

	public UpgradeResource.Reference reference => _reference;

	private void Start()
	{
		Singleton<UpgradeShop>.Instance.onChanged += HandleOnChanged;
	}

	private void OnDestroy()
	{
		Singleton<UpgradeShop>.Instance.onChanged -= HandleOnChanged;
	}

	private void HandleOnChanged()
	{
		UpdateSelectable(_reference);
	}

	public void Initialize(UpgradeResource.Reference reference, Panel panel)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		_panel = panel;
		_reference = reference;
		_icon.sprite = reference.icon;
		_deactiveMask.sprite = reference.icon;
		UnityAction val = new UnityAction(TryUpgrade);
		((UnityEventBase)_button.onClick).RemoveAllListeners();
		((UnityEvent)_button.onClick).AddListener(val);
		Button button = _button;
		Navigation navigation = default(Navigation);
		((Navigation)(ref navigation)).mode = (Mode)3;
		((Selectable)button).navigation = navigation;
		_findEffect.SetActive(false);
		UpdateSelectable(reference);
		string typeName = Gear.Type.Upgrade.ToString();
		if (!GameData.Gear.IsFounded(typeName, _reference.name))
		{
			_findFrame.sprite = ((reference.type == UpgradeObject.Type.Cursed) ? _curseFindSprite : _normalFindSprite);
			((Component)_findFrame).gameObject.SetActive(true);
			((MonoBehaviour)this).StartCoroutine(CActiveFindEffect());
		}
		HandleOnChanged();
		GameData.Gear.SetFounded(typeName, _reference.name, value: true);
	}

	private IEnumerator CActiveFindEffect()
	{
		_findEffect.SetActive(true);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.findSoundInfo, ((Component)_findFrame).gameObject.transform.position);
		yield return Chronometer.global.WaitForSeconds(1f);
		_findEffect.SetActive(false);
	}

	private void OnEnable()
	{
		_failEffect.SetActive(false);
	}

	public void OnSelect(BaseEventData eventData)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (selectable.interactable)
		{
			((Component)_findFrame).gameObject.SetActive(false);
			_panel.UpdateCurrentOption(this);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.moveSoundInfo, ((Component)_findFrame).gameObject.transform.position);
		}
	}

	public void UpdateObjectTo(UpgradeResource.Reference reference)
	{
		_reference = reference;
	}

	private void UpdateSelectable(UpgradeResource.Reference reference)
	{
		if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(reference))
		{
			selectable.interactable = false;
			selectable.targetGraphic.raycastTarget = false;
			_selectionSpriteSwapper.OnDeselect(null);
			((Behaviour)_selectionSpriteSwapper).enabled = false;
			((Component)_deactiveMask).gameObject.SetActive(true);
		}
		else
		{
			selectable.interactable = true;
			selectable.targetGraphic.raycastTarget = true;
			((Behaviour)_selectionSpriteSwapper).enabled = true;
			((Component)_deactiveMask).gameObject.SetActive(false);
		}
	}

	private void TryUpgrade()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (!Singleton<UpgradeShop>.Instance.TryUpgrade(_reference))
		{
			((MonoBehaviour)this).StartCoroutine(CEmitFailEffect());
			PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.failSoundInfo, ((Component)_findFrame).gameObject.transform.position);
			return;
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.buySoundInfo, ((Component)_findFrame).gameObject.transform.position);
		_panel.AppendToUpgradedList();
		_panel.UpdateCurrentOption(this);
		UpdateSelectable(_reference);
	}

	private IEnumerator CEmitFailEffect()
	{
		_failEffect.SetActive(true);
		yield return Chronometer.global.WaitForSeconds(0.3f);
		_failEffect.SetActive(false);
	}
}
