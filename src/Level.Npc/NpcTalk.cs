using System.Collections;
using System.Collections.Generic;
using Characters;
using GameResources;
using Scenes;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Npc;

public class NpcTalk : InteractiveObject
{
	[SerializeField]
	private string _displayNameKey;

	[SerializeField]
	private string _greetingKey;

	[SerializeField]
	private string _chatKey;

	[SerializeField]
	private UnityEvent _onChat;

	private NpcConversation _npcConversation;

	private string displayName => Localization.GetLocalizedString(_displayNameKey);

	private string greeting => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray(_greetingKey));

	private string[] chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays(_chatKey));

	private void Start()
	{
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
	}

	public override void InteractWith(Character character)
	{
		LetterBox.instance.Appear();
		_npcConversation.name = displayName;
		_npcConversation.portrait = null;
		_npcConversation.body = greeting;
		_npcConversation.skippable = false;
		_npcConversation.Type();
		_npcConversation.OpenChatSelector(Chat, Close);
	}

	private void Chat()
	{
		_npcConversation.skippable = true;
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			_onChat.Invoke();
			yield return _npcConversation.CConversation(chat);
			LetterBox.instance.Disappear();
		}
	}

	private void Close()
	{
		_npcConversation.visible = false;
		LetterBox.instance.Disappear();
	}
}
