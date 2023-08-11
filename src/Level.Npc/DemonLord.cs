using System.Collections;
using System.Collections.Generic;
using Characters;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace Level.Npc;

public class DemonLord : InteractiveObject
{
	private const NpcType _type = NpcType.DemonLord;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	private NpcLineText _lineText;

	private NpcConversation _npcConversation;

	public string displayName => Localization.GetLocalizedString($"npc/{NpcType.DemonLord}/name");

	public string greeting => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.DemonLord}/greeting"));

	public string[] chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays($"npc/{NpcType.DemonLord}/chat"));

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

	private void Start()
	{
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
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
