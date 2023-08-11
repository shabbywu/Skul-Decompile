using System.Collections;
using System.Collections.Generic;
using Characters;
using CutScenes;
using Data;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc;

public class Witch : InteractiveObject
{
	private const NpcType _type = NpcType.Witch;

	private readonly int _normalIdle = Animator.StringToHash("Idle_Human_Castle");

	private readonly int _hardmodeIdle = Animator.StringToHash("Idle_Human_Castle2");

	private readonly int _hardmodeEmptyIdle = Animator.StringToHash("Idle_NoHuman");

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	private Sprite _portraitCat;

	[SerializeField]
	private GameObject _body;

	private NpcConversation _npcConversation;

	[SerializeField]
	private SoundInfo _open;

	[SerializeField]
	private SoundInfo _close;

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private InteractiveObject _tutorialWitch;

	public string displayName => Localization.GetLocalizedString($"npc/{NpcType.Witch}/name");

	public string greeting => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.Witch}/greeting"));

	public string[] chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays($"npc/{NpcType.Witch}/chat"));

	public string masteriesScript => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.Witch}/Masteries"));

	public string masteriesLabel => Localization.GetLocalizedString($"npc/{NpcType.Witch}/Masteries/label");

	protected override void Awake()
	{
		base.Awake();
		if (GameData.Generic.normalEnding)
		{
			if (GameData.Progress.cutscene.GetData(CutScenes.Key.dwarfEngineer_First))
			{
				_animator.Play(_hardmodeIdle);
				return;
			}
			_animator.Play(_hardmodeEmptyIdle);
			((Component)_lineText).gameObject.SetActive(false);
			_tutorialWitch.Deactivate();
			Deactivate();
		}
		else
		{
			_animator.Play(_normalIdle);
		}
	}

	private void Start()
	{
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
	}

	private void OnDisable()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!Service.quitting)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_close, ((Component)this).transform.position);
			LetterBox.instance.visible = false;
		}
	}

	public override void InteractWith(Character character)
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			yield return LetterBox.instance.CAppear();
			_npcConversation.name = displayName;
			_npcConversation.portrait = _portrait;
			_npcConversation.body = greeting;
			_npcConversation.skippable = false;
			_npcConversation.Type();
			_npcConversation.OpenContentSelector(masteriesLabel, OpenContent, Chat, Close);
		}
	}

	private void OpenContent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_open, ((Component)this).transform.position);
		_npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.DarkQuartz);
		_npcConversation.witchContent.SetActive(true);
		_npcConversation.body = masteriesScript;
		_npcConversation.skippable = false;
		_npcConversation.Type();
	}

	private void Chat()
	{
		_npcConversation.skippable = true;
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			yield return _npcConversation.CConversation(chat);
			LetterBox.instance.Disappear();
		}
	}

	private void Close()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_close, ((Component)this).transform.position);
		_npcConversation.visible = false;
		LetterBox.instance.Disappear();
	}
}
