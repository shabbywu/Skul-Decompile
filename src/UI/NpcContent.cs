using System;
using System.Collections.Generic;
using GameResources;
using InControl;
using UnityEngine;
using UserInput;

namespace UI;

public class NpcContent : MonoBehaviour
{
	[Serializable]
	public class TypeGameObjectArray : EnumArray<Type, GameObject>
	{
	}

	public enum Type
	{
		Witch,
		WitchCat,
		Druid,
		Orge,
		Fox
	}

	[SerializeField]
	private NpcConversation _npcConversation;

	[SerializeField]
	private TypeGameObjectArray _contents;

	[SerializeField]
	private GameObject _contentContainer;

	[SerializeField]
	private ContentSelector _contentSelector;

	private Type _type;

	private string _name;

	private string[] _chatScripts;

	private string _greeting;

	private string _content;

	private void OpenContent()
	{
		_npcConversation.body = _content;
		_npcConversation.skippable = false;
		_npcConversation.Type();
		((Component)_contentSelector).gameObject.SetActive(false);
		((EnumArray<Type, GameObject>)_contents)[_type].gameObject.SetActive(true);
	}

	private void Chat()
	{
		_npcConversation.Conversation(_chatScripts);
	}

	private void Update()
	{
		if (_contentContainer.activeSelf && ((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed)
		{
			Close();
		}
	}

	public void Open(Type type, string key)
	{
		_type = type;
		_name = Localization.GetLocalizedString(key + "/name");
		_chatScripts = ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays("npc/" + key + "/chat"));
		_greeting = Localization.GetLocalizedString(key + "/greeting");
		_content = Localization.GetLocalizedString(key + "/content");
		string localizedString = Localization.GetLocalizedString(key + "/contentLabel");
		_contentContainer.SetActive(true);
		_npcConversation.name = _name;
		_npcConversation.body = _greeting;
		_npcConversation.skippable = false;
		_npcConversation.Type();
		_npcConversation.OpenContentSelector(localizedString, OpenContent, Chat, Close);
	}

	public void Close()
	{
		_npcConversation.Done();
		_contentSelector.Close();
		((EnumArray<Type, GameObject>)_contents)[_type].gameObject.SetActive(false);
		_contentContainer.SetActive(false);
	}
}
