using System;
using System.Collections;
using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc;

public class Fox : InteractiveObject
{
	private enum Phase
	{
		Initial,
		Gave,
		ExtraGave
	}

	private const int _randomSeed = 1141070443;

	private const NpcType _type = NpcType.Fox;

	[SerializeField]
	private int _extraHeadDarkQuartzCost;

	[SerializeField]
	private RarityPossibilities _headPossibilities;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	private Transform _dropPosition;

	private Phase _phase;

	private NpcConversation _npcConversation;

	private WeaponRequest _weaponToDrop;

	private WeaponRequest _extraWeaponToDrop;

	private Random _random;

	public string displayName => Localization.GetLocalizedString($"npc/{NpcType.Fox}/name");

	public string greeting => Localization.GetLocalizedStringArray($"npc/{NpcType.Fox}/greeting").Random();

	public string[] chat => Localization.GetLocalizedStringArrays($"npc/{NpcType.Fox}/chat").Random();

	public string[] giveHeadScripts => Localization.GetLocalizedStringArrays($"npc/{NpcType.Fox}/GiveHead").Random();

	public string giveExtraHead => string.Format(Localization.GetLocalizedStringArray($"npc/{NpcType.Fox}/GiveExtraHead").Random(), _extraHeadDarkQuartzCost);

	public string giveExtraHeadLabel => Localization.GetLocalizedString($"npc/{NpcType.Fox}/GiveExtraHead/label");

	public string[] giveExtraHeadNoMoney => Localization.GetLocalizedStringArrays($"npc/{NpcType.Fox}/GiveExtraHead/NoMoney").Random();

	protected override void Awake()
	{
		base.Awake();
		if (!GameData.Progress.GetRescued(NpcType.Fox))
		{
			((Component)this).gameObject.SetActive(false);
		}
		_random = new Random(GameData.Save.instance.randomSeed + 1141070443);
	}

	private void Start()
	{
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		_weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(_random, _headPossibilities.Evaluate(_random)).LoadAsync();
	}

	private void OnDisable()
	{
		if (!Service.quitting)
		{
			LetterBox.instance.visible = false;
		}
	}

	private void OnDestroy()
	{
		_weaponToDrop?.Release();
		_extraWeaponToDrop?.Release();
	}

	public override void InteractWith(Character character)
	{
		_npcConversation.name = displayName;
		_npcConversation.portrait = _portrait;
		switch (_phase)
		{
		case Phase.Initial:
			_phase = Phase.Gave;
			((MonoBehaviour)this).StartCoroutine(CGiveHead(character));
			break;
		case Phase.Gave:
			((MonoBehaviour)this).StartCoroutine(CSelectContent());
			break;
		case Phase.ExtraGave:
			Chat();
			break;
		}
	}

	private IEnumerator CSelectContent()
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.body = greeting;
		_npcConversation.skippable = false;
		_npcConversation.Type();
		_npcConversation.OpenContentSelector(giveExtraHeadLabel, GetExtraHead, Chat, Close);
	}

	private void Chat()
	{
		_npcConversation.skippable = true;
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			if (!LetterBox.instance.visible)
			{
				yield return LetterBox.instance.CAppear();
			}
			yield return _npcConversation.CConversation(chat);
			LetterBox.instance.Disappear();
		}
	}

	private void Close()
	{
		_npcConversation.visible = false;
		LetterBox.instance.Disappear();
	}

	private void GetExtraHead()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CDropExtraHead()
		{
			while (!_extraWeaponToDrop.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropWeapon(_extraWeaponToDrop, _dropPosition.position);
			LetterBox.instance.Disappear();
		}
		IEnumerator CNoMoneyAndClose()
		{
			_npcConversation.skippable = true;
			yield return _npcConversation.CConversation(giveExtraHeadNoMoney);
			LetterBox.instance.Disappear();
		}
		IEnumerator CRun()
		{
			_npcConversation.skippable = true;
			_npcConversation.body = giveExtraHead;
			yield return _npcConversation.CType();
			_npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.DarkQuartz);
			_npcConversation.OpenConfirmSelector(OnYesSelected, Close);
		}
		void OnYesSelected()
		{
			if (GameData.Currency.darkQuartz.Consume(_extraHeadDarkQuartzCost))
			{
				_phase = Phase.ExtraGave;
				((MonoBehaviour)this).StartCoroutine(CDropExtraHead());
				_npcConversation.visible = false;
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CNoMoneyAndClose());
			}
		}
	}

	private IEnumerator CGiveHead(Character character)
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.skippable = true;
		yield return _npcConversation.CConversation(giveHeadScripts);
		LetterBox.instance.Disappear();
		while (!_weaponToDrop.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.levelManager.DropWeapon(_weaponToDrop, _dropPosition.position);
		_extraWeaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(_random, _headPossibilities.Evaluate(_random)).LoadAsync();
	}
}
