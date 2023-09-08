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

public class Ogre : InteractiveObject
{
	private enum Phase
	{
		Initial,
		Gave,
		ExtraGave
	}

	private const int _randomSeed = -1539017818;

	private const NpcType _type = NpcType.Ogre;

	[SerializeField]
	private int _extraItemDarkQuartzCost;

	[SerializeField]
	private RarityPossibilities _itemPossibilities;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	private Transform _dropPosition;

	private Phase _phase;

	private NpcConversation _npcConversation;

	private ItemRequest _itemToDrop;

	private ItemRequest _extraItemToDrop;

	private Random _random;

	public string displayName => Localization.GetLocalizedString($"npc/{NpcType.Ogre}/name");

	public string greeting => Localization.GetLocalizedStringArray($"npc/{NpcType.Ogre}/greeting").Random();

	public string[] chat => Localization.GetLocalizedStringArrays($"npc/{NpcType.Ogre}/chat").Random();

	public string[] giveItemScripts => Localization.GetLocalizedStringArrays($"npc/{NpcType.Ogre}/GiveItem").Random();

	public string giveExtraItem => string.Format(Localization.GetLocalizedStringArray($"npc/{NpcType.Ogre}/GiveExtraItem").Random(), _extraItemDarkQuartzCost);

	public string giveExtraItemLabel => Localization.GetLocalizedString($"npc/{NpcType.Ogre}/GiveExtraItem/label");

	public string[] giveExtraItemNoMoney => Localization.GetLocalizedStringArrays($"npc/{NpcType.Ogre}/GiveExtraItem/NoMoney").Random();

	protected override void Awake()
	{
		base.Awake();
		if (!GameData.Progress.GetRescued(NpcType.Ogre))
		{
			((Component)this).gameObject.SetActive(false);
		}
		_random = new Random(GameData.Save.instance.randomSeed + -1539017818);
	}

	private void Start()
	{
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		_itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _itemPossibilities.Evaluate(_random)).LoadAsync();
	}

	private void OnDisable()
	{
		if (!Service.quitting)
		{
			LetterBox.instance.visible = false;
			_itemToDrop?.Release();
			_extraItemToDrop?.Release();
		}
	}

	public override void InteractWith(Character character)
	{
		_npcConversation.name = displayName;
		_npcConversation.portrait = _portrait;
		switch (_phase)
		{
		case Phase.Initial:
			_phase = Phase.Gave;
			((MonoBehaviour)this).StartCoroutine(CGiveItem(character));
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
		_npcConversation.OpenContentSelector(giveExtraItemLabel, GetExtraItem, Chat, Close);
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

	private void GetExtraItem()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CDropExtraHead()
		{
			while (!_extraItemToDrop.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropItem(_extraItemToDrop, _dropPosition.position);
			LetterBox.instance.Disappear();
		}
		IEnumerator CNoMoneyAndClose()
		{
			_npcConversation.skippable = true;
			yield return _npcConversation.CConversation(giveExtraItemNoMoney);
			LetterBox.instance.Disappear();
		}
		IEnumerator CRun()
		{
			_npcConversation.skippable = true;
			_npcConversation.body = giveExtraItem;
			yield return _npcConversation.CType();
			_npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.DarkQuartz);
			_npcConversation.OpenConfirmSelector(OnYesSelected, Close);
		}
		void OnYesSelected()
		{
			if (GameData.Currency.darkQuartz.Consume(_extraItemDarkQuartzCost))
			{
				_phase = Phase.ExtraGave;
				((MonoBehaviour)this).StartCoroutine(CDropExtraHead());
				_npcConversation.visible = false;
				LetterBox.instance.Disappear();
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CNoMoneyAndClose());
			}
		}
	}

	private IEnumerator CGiveItem(Character character)
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.skippable = true;
		yield return _npcConversation.CConversation(giveItemScripts);
		LetterBox.instance.Disappear();
		while (!_itemToDrop.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.levelManager.DropItem(_itemToDrop, _dropPosition.position);
		_extraItemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _itemPossibilities.Evaluate(_random)).LoadAsync();
	}
}
