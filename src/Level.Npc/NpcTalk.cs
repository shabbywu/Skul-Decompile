using System.Collections;
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

	private string greeting => Localization.GetLocalizedStringArray(_greetingKey).Random();

	private string[] chat => Localization.GetLocalizedStringArrays(_chatKey).Random();

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
