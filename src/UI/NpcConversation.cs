using System;
using System.Collections;
using Data;
using GameResources;
using InControl;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI;

public class NpcConversation : MonoBehaviour
{
	[SerializeField]
	private NpcName _name;

	[SerializeField]
	private NpcConversationBody _body;

	[SerializeField]
	private Image _enter;

	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private Image _portrait;

	[SerializeField]
	private GameObject _portraitContainer;

	[SerializeField]
	private ContentSelector _contentSelector;

	[SerializeField]
	private CurrencyBalanceDisplay _currencyBalanceDisplay;

	[SerializeField]
	private GameObject _witchContent;

	public string name
	{
		get
		{
			return _name.text;
		}
		set
		{
			_name.text = value;
		}
	}

	public string body
	{
		get
		{
			return _body.text;
		}
		set
		{
			_body.text = value;
		}
	}

	public Sprite portrait
	{
		get
		{
			return _portrait.sprite;
		}
		set
		{
			_portrait.sprite = value;
			_portraitContainer.SetActive((Object)(object)value != (Object)null);
		}
	}

	public bool visible
	{
		get
		{
			return _container.activeSelf;
		}
		set
		{
			if (_container.activeSelf != value)
			{
				if (!value)
				{
					((Component)_currencyBalanceDisplay).gameObject.SetActive(false);
					_contentSelector.Close();
					_witchContent.SetActive(false);
				}
				_container.SetActive(value);
			}
		}
	}

	public bool skippable
	{
		get
		{
			return _body.skippable;
		}
		set
		{
			_body.skippable = value;
		}
	}

	public GameObject witchContent => _witchContent;

	private void Awake()
	{
		((Behaviour)_enter).enabled = false;
	}

	private void OnDisable()
	{
		((Component)_currencyBalanceDisplay).gameObject.SetActive(false);
		_contentSelector.Close();
		_witchContent.SetActive(false);
	}

	public void OpenChatSelector(Action onChat, Action onCancel)
	{
		_contentSelector.Open(Localization.GetLocalizedString("npc/conversation/chat"), onChat, Localization.GetLocalizedString("npc/conversation/cancel"), onCancel);
	}

	public void OpenConfirmSelector(Action onYes, Action onNo)
	{
		_contentSelector.Open(Localization.GetLocalizedString("label/confirm/yes"), onYes, Localization.GetLocalizedString("label/confirm/no"), onNo);
	}

	public void OpenContentSelector(string contentLabel, Action onContent, Action onChat, Action onCancel)
	{
		_contentSelector.Open(contentLabel, onContent, Localization.GetLocalizedString("npc/conversation/chat"), onChat, Localization.GetLocalizedString("npc/conversation/cancel"), onCancel);
	}

	public void OpenContentSelector(string contentLabel, Action onContent, string cancelLabel, Action onCancel)
	{
		_contentSelector.Open(contentLabel, onContent, cancelLabel, onCancel);
	}

	public void OpenCurrencyBalancePanel(GameData.Currency.Type type)
	{
		((Component)_currencyBalanceDisplay).gameObject.SetActive(true);
		_currencyBalanceDisplay.SetType(type);
	}

	public void CloseCurrencyBalancePanel()
	{
		((Component)_currencyBalanceDisplay).gameObject.SetActive(false);
	}

	public void Talk(string nameKey, string textKey)
	{
		TalkRaw(Localization.GetLocalizedString(nameKey), Localization.GetLocalizedString(textKey));
	}

	public void TalkRaw(string name, string text)
	{
		_name.text = name;
		_body.text = text;
		visible = true;
	}

	public IEnumerator CTalk(string nameKey, string textKey)
	{
		portrait = null;
		skippable = true;
		name = Localization.GetLocalizedString(nameKey);
		body = Localization.GetLocalizedString(textKey);
		if (!string.IsNullOrWhiteSpace(body))
		{
			yield return CType();
			yield return CWaitInput();
		}
	}

	public IEnumerator CTalkRaw(string name, string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
			portrait = null;
			skippable = true;
			this.name = name;
			body = text;
			yield return CType();
			yield return CWaitInput();
		}
	}

	public void Conversation(params string[] texts)
	{
		((MonoBehaviour)this).StartCoroutine(CConversation(texts));
	}

	public IEnumerator CConversation(params string[] texts)
	{
		if (_body.typing)
		{
			body = string.Empty;
			_body.StopType();
			yield return null;
		}
		foreach (string text in texts)
		{
			body = text;
			yield return CType();
			yield return CWaitInput();
		}
		visible = false;
	}

	public void Type()
	{
		((MonoBehaviour)this).StartCoroutine(CType());
	}

	public IEnumerator CType()
	{
		visible = true;
		if (_body.typing)
		{
			_body.StopType();
			yield return null;
		}
		yield return _body.CType(this);
	}

	public IEnumerator CWaitInput()
	{
		((Behaviour)_enter).enabled = true;
		do
		{
			yield return null;
		}
		while (!((OneAxisInputControl)KeyMapper.Map.Attack).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Jump).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed);
		((Behaviour)_enter).enabled = false;
	}

	public void Done()
	{
		visible = false;
	}
}
