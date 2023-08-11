using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Data;
using GameResources;
using Housing;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc;

public class DeathKnight : InteractiveObject
{
	[Serializable]
	public class SpecialBuildText
	{
		[SerializeField]
		public string textKey;

		[SerializeField]
		public BuildLevel target;

		public string Text(int cost)
		{
			return string.Format(Localization.GetLocalizedString($"npc/{NpcType.DeathKnight}/build/Special/{textKey}"), cost);
		}
	}

	private const NpcType _type = NpcType.DeathKnight;

	private bool _afterBuild;

	private NpcConversation _npcConversation;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	private HousingBuilder _housingBuilder;

	[SerializeField]
	private SpecialBuildText[] _specialBuildTexts;

	public string displayName => Localization.GetLocalizedString($"npc/{NpcType.DeathKnight}/name");

	public string greeting => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.DeathKnight}/greeting"));

	public string[] chat => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays($"npc/{NpcType.DeathKnight}/chat"));

	public string buildLabel => Localization.GetLocalizedString($"npc/{NpcType.DeathKnight}/build/Label");

	public string buildSuccess => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.DeathKnight}/build/Success"));

	public string buildNoMoney => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.DeathKnight}/build/NoMoney"));

	public string[] buildNoLevel => Localization.GetLocalizedStringArray($"npc/{NpcType.DeathKnight}/build/NoLevel");

	public string buildAgain => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.DeathKnight}/build/Again"));

	public string BuildText(int cost)
	{
		return string.Format(ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray($"npc/{NpcType.DeathKnight}/build")), cost);
	}

	private string GetBuildText(BuildLevel buildLevel)
	{
		for (int i = 0; i < _specialBuildTexts.Length; i++)
		{
			if ((Object)(object)_specialBuildTexts[i].target == (Object)(object)buildLevel)
			{
				return _specialBuildTexts[i].Text(buildLevel.cost);
			}
		}
		return BuildText(buildLevel.cost);
	}

	protected override void Awake()
	{
		base.Awake();
		if (!GameData.Progress.GetRescued(NpcType.DeathKnight))
		{
			((Component)this).gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
	}

	private void OnDisable()
	{
		if (!Service.quitting)
		{
			LetterBox.instance.visible = false;
		}
	}

	public override void InteractWith(Character character)
	{
		_npcConversation.name = displayName;
		_npcConversation.portrait = _portrait;
		((MonoBehaviour)this).StartCoroutine(CBuild());
	}

	private IEnumerator CBuild()
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.body = (_afterBuild ? buildAgain : greeting);
		_npcConversation.skippable = false;
		_npcConversation.Type();
		_npcConversation.OpenContentSelector(buildLabel, Build, Chat, Close);
	}

	private void Chat()
	{
		_npcConversation.skippable = true;
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			if (!LetterBox.instance.visible)
			{
				yield return LetterBox.instance.CAppear();
			}
			yield return _npcConversation.CConversation(chat);
			LetterBox.instance.Disappear();
		}
	}

	private void Close()
	{
		_npcConversation.visible = false;
		LetterBox.instance.Disappear();
	}

	private void Build()
	{
		BuildLevel nextBuildLevel = _housingBuilder.GetLevelAfterPoint(GameData.Progress.housingPoint);
		((MonoBehaviour)this).StartCoroutine(CRun());
		IEnumerator CRun()
		{
			_npcConversation.skippable = true;
			if ((Object)(object)nextBuildLevel != (Object)null)
			{
				_npcConversation.body = GetBuildText(nextBuildLevel);
				yield return _npcConversation.CType();
				_npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.DarkQuartz);
				_npcConversation.OpenConfirmSelector(OnYesSelected, Close);
			}
			else
			{
				yield return _npcConversation.CConversation(buildNoLevel);
				LetterBox.instance.Disappear();
			}
		}
		void OnYesSelected()
		{
			string text;
			if (GameData.Currency.darkQuartz.Consume(nextBuildLevel.cost))
			{
				_afterBuild = true;
				GameData.Progress.housingPoint = nextBuildLevel.order;
				GameData.Progress.SaveAll();
				GameData.Currency.SaveAll();
				PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
				if ((Object)(object)nextBuildLevel.next == (Object)null)
				{
					ExtensionMethods.Set((Type)4);
				}
				text = buildSuccess;
			}
			else
			{
				text = buildNoMoney;
			}
			_npcConversation.CloseCurrencyBalancePanel();
			((MonoBehaviour)this).StartCoroutine(CYesAndClose());
			IEnumerator CYesAndClose()
			{
				_npcConversation.skippable = true;
				yield return _npcConversation.CConversation(text);
				LetterBox.instance.Disappear();
			}
		}
	}
}
