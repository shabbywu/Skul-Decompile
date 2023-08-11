using System.Collections;
using System.Collections.Generic;
using Characters;
using CutScenes;
using Data;
using GameResources;
using InControl;
using Runnables;
using Scenes;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace Level.Npc;

public class Dwarf : InteractiveObject
{
	private const NpcType _type = NpcType.Dwarf;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private bool _readyForLevelChangeTutorial;

	[SerializeField]
	private Runnable _levelChangeTutorial;

	[SerializeField]
	private bool _levelTextActive = true;

	[SerializeField]
	private TMP_Text _levelText;

	[SerializeField]
	private CustomAudioSource _levelChangeAudio;

	[SerializeField]
	private GameObject _activeLevelFrame;

	[SerializeField]
	private GameObject _uiLevelChange;

	private readonly int _putQuartz = Animator.StringToHash("Put_DarkQuartz");

	private readonly int _Idle_2 = Animator.StringToHash("Idle_2");

	private NpcConversation _npcConversation;

	private int _tryLevel;

	private const string _activeColorCode = "#7311BC";

	private const string _deactiveColorCode = "#020005";

	private const float _levelAnimationDuration = 0.3f;

	private CoroutineReference _levelAnimationCoroutineReference;

	private Color _activeColor;

	private Color _deactiveColor;

	public string displayName => Localization.GetLocalizedString($"npc/{NpcType.Dwarf}/name");

	public string greeting => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.Dwarf}/greeting"));

	public string[] chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays($"npc/{NpcType.Dwarf}/chat"));

	public int tryLevel => _tryLevel;

	public override void InteractWith(Character character)
	{
		if (_readyForLevelChangeTutorial)
		{
			_levelChangeTutorial.Run();
			_readyForLevelChangeTutorial = false;
			return;
		}
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
		_tryLevel = GameData.HardmodeProgress.hardmodeLevel;
		ColorUtility.TryParseHtmlString("#7311BC", ref _activeColor);
		ColorUtility.TryParseHtmlString("#020005", ref _deactiveColor);
		UpdateHardmodeLevelText();
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

	public void SetReadyForLevelChangeTutorial()
	{
		_readyForLevelChangeTutorial = true;
	}

	public void SetNotReadyForLevelChangeTutorial()
	{
		_readyForLevelChangeTutorial = false;
	}

	private void Update()
	{
		if (base.popupVisible && GameData.Progress.cutscene.GetData(CutScenes.Key.darkMirror_FirstClear) && GameData.HardmodeProgress.clearedLevel >= 0)
		{
			if (((OneAxisInputControl)KeyMapper.Map.Up).WasPressed)
			{
				LevelUp();
			}
			else if (((OneAxisInputControl)KeyMapper.Map.Down).WasPressed)
			{
				LevelDown();
			}
		}
	}

	private void LevelUp()
	{
		if (_tryLevel == GameData.HardmodeProgress.maxLevel)
		{
			_tryLevel = GameData.HardmodeProgress.maxLevel;
			GameData.HardmodeProgress.hardmodeLevel = _tryLevel;
			UpdateHardmodeLevelText();
			return;
		}
		_tryLevel++;
		if (_tryLevel <= GameData.HardmodeProgress.clearedLevel + 1)
		{
			GameData.HardmodeProgress.hardmodeLevel = _tryLevel;
		}
		UpdateHardmodeLevelText();
	}

	private void LevelDown()
	{
		if (_tryLevel != 0)
		{
			_tryLevel--;
			if (_tryLevel <= GameData.HardmodeProgress.clearedLevel + 1)
			{
				GameData.HardmodeProgress.hardmodeLevel = _tryLevel;
			}
			UpdateHardmodeLevelText();
		}
	}

	private void UpdateHardmodeLevelText()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (_readyForLevelChangeTutorial)
		{
			_levelChangeTutorial.Run();
			_readyForLevelChangeTutorial = false;
		}
		else if (_levelTextActive)
		{
			((CoroutineReference)(ref _levelAnimationCoroutineReference)).Stop();
			_levelAnimationCoroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CUpdateAnimation());
			_activeLevelFrame.SetActive(_tryLevel == GameData.HardmodeProgress.clearedLevel + 1);
			((Graphic)_levelText).color = ((_tryLevel <= GameData.HardmodeProgress.clearedLevel + 1) ? _activeColor : _deactiveColor);
			_levelText.text = _tryLevel.ToString();
		}
	}

	private IEnumerator CUpdateAnimation()
	{
		((Component)_levelText).gameObject.SetActive(false);
		_animator.Play(_putQuartz);
		_levelChangeAudio.Play();
		yield return Chronometer.global.WaitForSeconds(0.3f);
		_levelChangeAudio.Stop();
		_animator.Play(_Idle_2);
		((Component)_levelText).gameObject.SetActive(true);
	}

	public void DeactivateLevelText()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		_levelTextActive = false;
		((Component)_levelText).gameObject.SetActive(false);
		_activeLevelFrame.gameObject.SetActive(false);
	}

	public void ActivateLevelText()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StopAllCoroutines();
		_levelTextActive = true;
		((Component)_levelText).gameObject.SetActive(true);
		_tryLevel = GameData.HardmodeProgress.hardmodeLevel;
		_activeLevelFrame.SetActive(_tryLevel == GameData.HardmodeProgress.clearedLevel + 1);
		((Graphic)_levelText).color = ((_tryLevel <= GameData.HardmodeProgress.clearedLevel + 1) ? _activeColor : _deactiveColor);
		_levelText.text = _tryLevel.ToString();
	}

	public void SetLevelText(int _level)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StopAllCoroutines();
		_tryLevel = _level;
		((Graphic)_levelText).color = ((_tryLevel <= GameData.HardmodeProgress.clearedLevel + 1) ? _activeColor : _deactiveColor);
		_levelText.text = _tryLevel.ToString();
	}

	public void OnDisable()
	{
		DeactivateLevelText();
	}

	public void DetachLevelChangeUI()
	{
		_uiObjects = (GameObject[])(object)new GameObject[0];
	}

	public void AttachLevelChangeUI()
	{
		_uiObjects = (GameObject[])(object)new GameObject[1];
		_uiObjects[0] = _uiLevelChange;
	}
}
