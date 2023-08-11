using System.Collections;
using FX;
using InControl;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UserInput;

namespace UI;

public class NpcConversationBody : Dialogue
{
	private const float _lettersPerSecond = 25f;

	private const float _intervalPerLetter = 0.04f;

	[SerializeField]
	private TextMeshProUGUI _textMeshPro;

	[SerializeField]
	private float _typeSpeed = 1f;

	[SerializeField]
	private SoundInfo _typeSoundInfo;

	[SerializeField]
	private SoundInfo _systemTypeSoundInfo;

	public bool skippable { get; set; }

	public bool typing { get; private set; }

	public string text
	{
		get
		{
			return ((TMP_Text)_textMeshPro).text;
		}
		set
		{
			((TMP_Text)_textMeshPro).text = value;
		}
	}

	public override bool closeWithPauseKey => false;

	protected override void OnEnable()
	{
		Dialogue.opened.Add(this);
	}

	protected override void OnDisable()
	{
		Dialogue.opened.Remove(this);
	}

	public IEnumerator CType()
	{
		typing = true;
		((TMP_Text)_textMeshPro).ForceMeshUpdate(false, false);
		int visibleCharacterCount = ((TMP_Text)_textMeshPro).textInfo.characterCount;
		((TMP_Text)_textMeshPro).maxVisibleCharacters = 0;
		float interval = 0.04f * (1f / _typeSpeed);
		for (int index = 0; index < visibleCharacterCount; index++)
		{
			if (!typing)
			{
				yield break;
			}
			if (((TMP_Text)_textMeshPro).text[index] != ' ')
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(_systemTypeSoundInfo, ((Component)Singleton<Service>.Instance.levelManager.player).transform.position);
			}
			TextMeshProUGUI textMeshPro = _textMeshPro;
			int maxVisibleCharacters = ((TMP_Text)textMeshPro).maxVisibleCharacters;
			((TMP_Text)textMeshPro).maxVisibleCharacters = maxVisibleCharacters + 1;
			((TMP_Text)_textMeshPro).havePropertiesChanged = false;
			float time = 0f;
			while (time < interval)
			{
				if (!typing)
				{
					yield break;
				}
				yield return null;
				time += Time.unscaledDeltaTime;
				if (skippable && (((OneAxisInputControl)KeyMapper.Map.Attack).WasPressed || ((OneAxisInputControl)KeyMapper.Map.Jump).WasPressed || ((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed || ((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed))
				{
					goto end_IL_0197;
				}
			}
			continue;
			end_IL_0197:
			break;
		}
		((TMP_Text)_textMeshPro).maxVisibleCharacters = visibleCharacterCount;
		typing = false;
	}

	public IEnumerator CType(NpcConversation conversation)
	{
		typing = true;
		((TMP_Text)_textMeshPro).ForceMeshUpdate(false, false);
		int visibleCharacterCount = ((TMP_Text)_textMeshPro).textInfo.characterCount;
		((TMP_Text)_textMeshPro).maxVisibleCharacters = 0;
		float interval = 0.04f * (1f / _typeSpeed);
		for (int index = 0; index < visibleCharacterCount; index++)
		{
			if (!typing)
			{
				yield break;
			}
			if (!conversation.visible)
			{
				break;
			}
			if (((TMP_Text)_textMeshPro).text[index] != ' ')
			{
				if ((Object)(object)Singleton<Service>.Instance.levelManager.player != (Object)null)
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(_typeSoundInfo, ((Component)Singleton<Service>.Instance.levelManager.player).transform.position);
				}
				else
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(_typeSoundInfo, ((Component)Camera.main).transform.position);
				}
			}
			TextMeshProUGUI textMeshPro = _textMeshPro;
			int maxVisibleCharacters = ((TMP_Text)textMeshPro).maxVisibleCharacters;
			((TMP_Text)textMeshPro).maxVisibleCharacters = maxVisibleCharacters + 1;
			((TMP_Text)_textMeshPro).havePropertiesChanged = false;
			float time = 0f;
			while (time < interval)
			{
				if (!typing)
				{
					yield break;
				}
				yield return null;
				time += Time.unscaledDeltaTime;
				if (skippable && (((OneAxisInputControl)KeyMapper.Map.Attack).WasPressed || ((OneAxisInputControl)KeyMapper.Map.Jump).WasPressed || ((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed || ((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed))
				{
					goto end_IL_01e0;
				}
			}
			continue;
			end_IL_01e0:
			break;
		}
		((TMP_Text)_textMeshPro).maxVisibleCharacters = visibleCharacterCount;
		typing = false;
	}

	public void StopType()
	{
		((TMP_Text)_textMeshPro).maxVisibleCharacters = 0;
		((TMP_Text)_textMeshPro).havePropertiesChanged = false;
		typing = false;
	}
}
