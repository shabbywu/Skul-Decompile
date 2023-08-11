using System.Collections;
using System.Collections.Generic;
using Characters;
using Data;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace Level.Npc;

public class Druid : InteractiveObject
{
	private const NpcType _type = NpcType.Druid;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	private NpcLineText _lineText;

	private NpcConversation _npcConversation;

	public string displayName => Localization.GetLocalizedString($"npc/{NpcType.Druid}/name");

	public string greeting => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.Druid}/greeting"));

	public string[] chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays($"npc/{NpcType.Druid}/chat"));

	public string changeProphecyLabel => Localization.GetLocalizedString($"npc/{NpcType.Druid}/ChangeProphecy/label");

	public string[] changeProphecyNoMoney => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays($"npc/{NpcType.Druid}/ChangeProphecy/NoMoney"));

	protected override void Awake()
	{
		base.Awake();
		if (!GameData.Progress.GetRescued(NpcType.Druid))
		{
			((Component)this).gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
	}

	public override void InteractWith(Character character)
	{
		((Component)_lineText).gameObject.SetActive(false);
		_npcConversation.name = displayName;
		_npcConversation.portrait = _portrait;
		_npcConversation.skippable = true;
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			yield return LetterBox.instance.CAppear();
			_npcConversation.OpenChatSelector(Chat, Close);
			_npcConversation.body = greeting;
			yield return _npcConversation.CType();
		}
	}

	private void Chat()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			yield return _npcConversation.CConversation(chat);
			Close();
		}
	}

	private void Close()
	{
		_npcConversation.visible = false;
		LetterBox.instance.Disappear();
		((Component)_lineText).gameObject.SetActive(true);
	}
}
