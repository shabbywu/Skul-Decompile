using System.Collections;
using Characters;
using Characters.Abilities;
using Data;
using FX;
using GameResources;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class HealthAuxiliaryEquipment : InteractiveObject
{
	[SerializeField]
	private SavableAbilityManager.Name _ability;

	[SerializeField]
	private GameObject[] _levelDisplay;

	[SerializeField]
	private EffectInfo _onLevelUpEffect;

	[SerializeField]
	private SoundInfo _onLevelUpSound;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private string _animatorTriggerKey;

	private NpcConversation _npcConversation;

	private int _currentIndex;

	private int _maxIndex;

	private int _cost;

	private float[] _statValues;

	private string displayName => Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", _ability, "name"));

	private string description => Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", _ability, "desc"));

	private string noMoney => Localization.GetLocalizedString("darktech/equipment/HealthAuxiliaryEquipment/noMoney");

	private string end => Localization.GetLocalizedString("darktech/equipment/HealthAuxiliaryEquipment/end");

	private GameData.Currency.Type _currencyType => GameData.Currency.Type.Bone;

	private GameData.Currency _currency => GameData.Currency.bone;

	private void Start()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		_currentIndex = (int)player.playerComponents.savableAbilityManager.GetStack(_ability);
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		switch (_ability)
		{
		case SavableAbilityManager.Name.HealthAuxiliaryDamage:
			_maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격.Length;
			if (_currentIndex >= _maxIndex)
			{
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격[_maxIndex - 1];
			}
			else
			{
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격[_currentIndex];
			}
			_statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프스텟;
			break;
		case SavableAbilityManager.Name.HealthAuxiliaryHealth:
			_maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격.Length;
			if (_currentIndex >= _maxIndex)
			{
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격[_maxIndex - 1];
			}
			else
			{
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격[_currentIndex];
			}
			_statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟;
			break;
		case SavableAbilityManager.Name.HealthAuxiliarySpeed:
			_maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격.Length;
			if (_currentIndex >= _maxIndex)
			{
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격[_maxIndex - 1];
			}
			else
			{
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격[_currentIndex];
			}
			_statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟;
			break;
		}
		for (int i = 0; i < _levelDisplay.Length; i++)
		{
			_levelDisplay[i].SetActive(false);
		}
		for (int j = 0; j < _currentIndex; j++)
		{
			_levelDisplay[j].SetActive(true);
		}
	}

	public override void InteractWith(Character character)
	{
		((MonoBehaviour)this).StartCoroutine(COpen());
	}

	private IEnumerator COpen()
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.name = displayName;
		if (_ability == SavableAbilityManager.Name.HealthAuxiliaryHealth)
		{
			_npcConversation.body = string.Format(description, displayName, _cost, _statValues[_currentIndex], Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟[_currentIndex]);
		}
		else
		{
			_npcConversation.body = string.Format(description, displayName, _cost, _statValues[_currentIndex] * 100f);
		}
		_npcConversation.skippable = true;
		_npcConversation.portrait = null;
		_npcConversation.Type();
		_npcConversation.OpenCurrencyBalancePanel(_currencyType);
		yield return _npcConversation.CType();
		_npcConversation.OpenConfirmSelector(OnSelectYes, Close);
		void OnSelectYes()
		{
			_npcConversation.CloseCurrencyBalancePanel();
			if (_currentIndex >= _maxIndex)
			{
				((MonoBehaviour)this).StartCoroutine(CFail(end));
			}
			else if (_currency.Consume(_cost))
			{
				GiveBuff();
				UpdateStack();
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CFail(noMoney));
			}
		}
	}

	private void UpdateStack()
	{
		if (_currentIndex < _maxIndex)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			_currentIndex = (int)player.playerComponents.savableAbilityManager.GetStack(_ability);
			_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			switch (_ability)
			{
			case SavableAbilityManager.Name.HealthAuxiliaryDamage:
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격[_currentIndex];
				_maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격.Length;
				_statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프스텟;
				break;
			case SavableAbilityManager.Name.HealthAuxiliaryHealth:
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격[_currentIndex];
				_maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격.Length;
				_statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟;
				break;
			case SavableAbilityManager.Name.HealthAuxiliarySpeed:
				_cost = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격[_currentIndex];
				_maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격.Length;
				_statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟;
				break;
			}
			for (int i = 0; i < _levelDisplay.Length; i++)
			{
				_levelDisplay[i].SetActive(false);
			}
			for (int j = 0; j < _currentIndex; j++)
			{
				_levelDisplay[j].SetActive(true);
			}
		}
	}

	private void GiveBuff()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		_onLevelUpEffect.Spawn(_levelDisplay[_currentIndex].transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_onLevelUpSound, _levelDisplay[_currentIndex].transform.position);
		_levelDisplay[_currentIndex].SetActive(true);
		_currentIndex++;
		if (_currentIndex >= _maxIndex)
		{
			ExtensionMethods.Set((Type)72);
		}
		Character player = Singleton<Service>.Instance.levelManager.player;
		player.playerComponents.savableAbilityManager.Apply(_ability, _currentIndex);
		if (_ability == SavableAbilityManager.Name.HealthAuxiliaryHealth)
		{
			player.health.Heal(Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프회복량[_currentIndex]);
		}
		_animator.SetTrigger(_animatorTriggerKey);
		Close();
	}

	private IEnumerator CFail(string body)
	{
		_npcConversation.skippable = true;
		_npcConversation.body = body;
		yield return _npcConversation.CType();
		yield return _npcConversation.CWaitInput();
		Close();
	}

	private void Close()
	{
		_npcConversation.visible = false;
		_npcConversation.CloseCurrencyBalancePanel();
		LetterBox.instance.Disappear();
	}
}
