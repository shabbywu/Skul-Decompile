using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.AI;
using Characters.Player;
using CutScenes;
using Data;
using GameResources;
using Hardmode;
using Runnables;
using Scenes;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc;

public class Arachne : InteractiveObject
{
	private enum Phase
	{
		First,
		Awakened
	}

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private ReviveScarecrowOnDie _scarecrow;

	[SerializeField]
	private Runnable _awakeningForRare;

	[SerializeField]
	private Runnable _awakeningForUnique;

	[SerializeField]
	private Runnable _awakeningForLegendary;

	[SerializeField]
	private Runnable _tutorial;

	private readonly string normalName = "arachne";

	private readonly string hardName = "arachneHardmode";

	[SerializeField]
	private CutScenes.Key _key = CutScenes.Key.arachne;

	private Phase _phase;

	private NpcConversation _npcConversation;

	private WeaponInventory _weaponInventory;

	private string arachne
	{
		get
		{
			if (!Singleton<HardmodeManager>.Instance.hardmode)
			{
				return normalName;
			}
			return hardName;
		}
	}

	private string displayName => Localization.GetLocalizedString("npc/" + arachne + "/name");

	private string greeting => Localization.GetLocalizedString("npc/" + arachne + "/greeting");

	private string awakenLabel => Localization.GetLocalizedString("npc/" + arachne + "/awaken/label");

	private string notExistsNextGrade => Localization.GetLocalizedString("npc/" + arachne + "/awaken/notExistsNextGrade");

	private string awaken => Localization.GetLocalizedString("npc/" + arachne + "/awaken");

	private string noMoney => Localization.GetLocalizedString("npc/" + arachne + "/awaken/noMoney");

	private string[] skulAwaken => Localization.GetLocalizedStringArray("npc/" + arachne + "/awaken/skul");

	private string[] tutorial => Localization.GetLocalizedStringArray("npc/" + arachne + "/tutorial");

	private string[] chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays("npc/" + arachne + "/chat"));

	private string askAwaken(int cost)
	{
		return string.Format(Localization.GetLocalizedString("npc/" + arachne + "/awaken/ask"), cost);
	}

	private string askAwakenAgain(int cost)
	{
		return string.Format(Localization.GetLocalizedString("npc/" + arachne + "/awaken/askAgain"), cost);
	}

	protected override void Awake()
	{
		base.Awake();
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
	}

	public override void InteractWith(Character character)
	{
		character.CancelAction();
		_weaponInventory = character.playerComponents.inventory.weapon;
		((Component)_lineText).gameObject.SetActive(false);
		_npcConversation.name = displayName;
		_npcConversation.skippable = true;
		_npcConversation.portrait = null;
		if (!GameData.Progress.cutscene.GetData(_key))
		{
			_tutorial.Run();
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CGreeting());
		}
	}

	public void SpawnScarecrowWave()
	{
		if (!((Object)(object)_scarecrow == (Object)null) && !((Component)_scarecrow).gameObject.activeSelf)
		{
			((Component)_scarecrow).gameObject.SetActive(true);
		}
	}

	private void SimpleConversationAndClose(params string[] texts)
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			yield return null;
			_npcConversation.skippable = true;
			yield return _npcConversation.CConversation(texts);
			Close();
		}
	}

	private IEnumerator CGreeting()
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.body = greeting;
		_npcConversation.skippable = false;
		_npcConversation.Type();
		_npcConversation.OpenContentSelector(awakenLabel, OnSelectAwaken, OnSelectChat, Close);
		IEnumerator CNotExistsNextGrade()
		{
			_npcConversation.skippable = true;
			_npcConversation.CloseCurrencyBalancePanel();
			yield return _npcConversation.CConversation(notExistsNextGrade);
			Close();
		}
		void OnSelectAwaken()
		{
			if (((Object)_weaponInventory.current).name.Equals("skul", StringComparison.OrdinalIgnoreCase))
			{
				SimpleConversationAndClose(skulAwaken);
			}
			else if (_weaponInventory.current.upgradable)
			{
				((MonoBehaviour)this).StartCoroutine(CAskAwaken());
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CNotExistsNextGrade());
			}
		}
		void OnSelectChat()
		{
			SimpleConversationAndClose(chat);
		}
	}

	private IEnumerator CAskAwaken()
	{
		int cost = Settings.instance.bonesToUpgrade[_weaponInventory.current.rarity];
		_npcConversation.skippable = true;
		_npcConversation.body = ((_phase == Phase.First) ? askAwaken(cost) : askAwakenAgain(cost));
		_npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.Bone);
		yield return _npcConversation.CType();
		_npcConversation.OpenConfirmSelector(OnSelectYes, Close);
		void OnSelectYes()
		{
			_npcConversation.CloseCurrencyBalancePanel();
			if (GameData.Currency.bone.Consume(cost))
			{
				((MonoBehaviour)this).StartCoroutine(CAwaken());
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CNoMoney());
			}
		}
	}

	private IEnumerator CAwaken()
	{
		_npcConversation.skippable = true;
		_npcConversation.body = awaken;
		yield return _npcConversation.CType();
		yield return _npcConversation.CWaitInput();
		_npcConversation.visible = false;
		_phase = Phase.Awakened;
		Rarity rarity = _weaponInventory.current.nextLevelReference.rarity;
		switch ((int)rarity)
		{
		case 0:
		case 1:
			_awakeningForRare.Run();
			break;
		case 2:
			_awakeningForUnique.Run();
			break;
		case 3:
			_awakeningForLegendary.Run();
			break;
		}
	}

	public IEnumerator CUpgrade()
	{
		yield return _weaponInventory.CUpgradeCurrentWeapon();
	}

	private IEnumerator CNoMoney()
	{
		_npcConversation.skippable = true;
		_npcConversation.body = noMoney;
		yield return _npcConversation.CType();
		yield return _npcConversation.CWaitInput();
		Close();
	}

	private void Close()
	{
		_npcConversation.visible = false;
		_npcConversation.CloseCurrencyBalancePanel();
		LetterBox.instance.Disappear();
		((Component)_lineText).gameObject.SetActive(true);
	}
}
