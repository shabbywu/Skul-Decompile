using System.Collections;
using Characters.Gear.Upgrades;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Upgrades;

public sealed class UpgradedElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	[SerializeField]
	private Button _button;

	[SerializeField]
	private Image _riskFrame;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Level _level;

	[SerializeField]
	private GameObject _failEffect;

	[SerializeField]
	[Header("이펙트")]
	private Animator[] _effects;

	private UpgradeResource.Reference _reference;

	private Panel _panel;

	private CoroutineReference _ceffectReference;

	private const float _effectLength = 1.04f;

	public Selectable selectable => (Selectable)(object)_button;

	public UpgradeResource.Reference reference => _reference;

	public void OnSelect(BaseEventData eventData)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (_reference != null)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.moveSoundInfo, ((Component)this).gameObject.transform.position);
			_panel.UpdateCurrentOption(this);
		}
	}

	private void OnEnable()
	{
		_failEffect.SetActive(false);
	}

	private void OnDisable()
	{
		Animator[] effects = _effects;
		for (int i = 0; i < effects.Length; i++)
		{
			((Component)effects[i]).gameObject.SetActive(false);
		}
	}

	public void Set(Panel panel, UpgradeResource.Reference reference, bool effect = false)
	{
		_panel = panel;
		_reference = reference;
		if (_reference == null)
		{
			SetEmpty();
			return;
		}
		SetElement();
		if (effect)
		{
			_ceffectReference.Stop();
			_ceffectReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CActiveGetEffect());
		}
	}

	private IEnumerator CActiveGetEffect()
	{
		Animator[] effects = _effects;
		foreach (Animator obj in effects)
		{
			((Component)obj).gameObject.SetActive(false);
			((Behaviour)obj).enabled = true;
			obj.Play(0, 0, 0f);
			((Behaviour)obj).enabled = false;
			((Component)obj).gameObject.SetActive(true);
		}
		float remainTime = 1.04f;
		while (remainTime > 0f)
		{
			yield return null;
			float deltaTime = Chronometer.global.deltaTime;
			effects = _effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].Update(deltaTime);
			}
			remainTime -= deltaTime;
		}
		effects = _effects;
		for (int i = 0; i < effects.Length; i++)
		{
			((Component)effects[i]).gameObject.SetActive(false);
		}
	}

	private void SetEmpty()
	{
		((Behaviour)_riskFrame).enabled = false;
		((Behaviour)_icon).enabled = false;
		_level.Set(0, 0, risky: false);
		((Component)this).gameObject.SetActive(false);
	}

	private void SetElement()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_riskFrame).enabled = _reference.type == UpgradeObject.Type.Cursed;
		((Behaviour)_icon).enabled = true;
		_icon.sprite = _reference.icon;
		int maxLevel = _reference.maxLevel;
		int currentLevel = _reference.GetCurrentLevel();
		_level.Set(currentLevel, maxLevel, _reference.type == UpgradeObject.Type.Cursed);
		UnityAction val = new UnityAction(TryUpgrade);
		((UnityEventBase)_button.onClick).RemoveAllListeners();
		((UnityEvent)_button.onClick).AddListener(val);
		Button button = _button;
		Navigation navigation = default(Navigation);
		((Navigation)(ref navigation)).mode = (Mode)3;
		((Selectable)button).navigation = navigation;
		((Component)this).gameObject.SetActive(true);
	}

	private void TryUpgrade()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(_reference))
		{
			if (!Singleton<UpgradeShop>.Instance.TryLevelUp(_reference))
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.failSoundInfo, ((Component)this).gameObject.transform.position);
				((MonoBehaviour)this).StartCoroutine(CEmitFailEffect());
				return;
			}
			_panel.UpdateUpgradedList();
		}
		else
		{
			if (!Singleton<UpgradeShop>.Instance.TryUpgrade(_reference))
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.failSoundInfo, ((Component)this).gameObject.transform.position);
				((MonoBehaviour)this).StartCoroutine(CEmitFailEffect());
				return;
			}
			_panel.AppendToUpgradedList();
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_panel.upgradableContainer.upgradeSoundInfo, ((Component)this).gameObject.transform.position);
		_panel.UpdateCurrentOption(this);
	}

	public void PlayFailEffect()
	{
		((MonoBehaviour)this).StartCoroutine(CEmitFailEffect());
	}

	private IEnumerator CEmitFailEffect()
	{
		_failEffect.SetActive(true);
		yield return Chronometer.global.WaitForSeconds(0.3f);
		_failEffect.SetActive(false);
	}
}
