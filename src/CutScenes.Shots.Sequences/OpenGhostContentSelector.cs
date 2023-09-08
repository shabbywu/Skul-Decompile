using System.Collections;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots.Sequences;

public class OpenGhostContentSelector : Sequence
{
	[SerializeField]
	[Header("Body")]
	private string _nameKey;

	[SerializeField]
	private string _textKey;

	[SerializeField]
	private bool _skippable;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	[Header("Label")]
	private string _contentsLabelKey;

	[SerializeField]
	private string _cancelLabelKey = "label/confirm/no";

	[UnityEditor.Subcomponent(typeof(ShotInfo))]
	[SerializeField]
	private ShotInfo.Subcomponents _onContents;

	[UnityEditor.Subcomponent(typeof(ShotInfo))]
	[SerializeField]
	private ShotInfo.Subcomponents _onClose;

	public override IEnumerator CRun()
	{
		NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		npcConversation.portrait = _portrait;
		int hardmodeLevel = GameData.HardmodeProgress.hardmodeLevel;
		int num = (int)(Singleton<Service>.Instance.levelManager.currentChapter.type - 9);
		npcConversation.name = Localization.GetLocalizedString(_nameKey);
		npcConversation.body = Localization.GetLocalizedString($"{_textKey}/{hardmodeLevel}/{num}");
		npcConversation.skippable = _skippable;
		yield return npcConversation.CType();
		npcConversation.OpenContentSelector(Localization.GetLocalizedString(_contentsLabelKey), delegate
		{
			_onContents.Run(null, null);
		}, Localization.GetLocalizedString(_cancelLabelKey), delegate
		{
			_onClose.Run(null, null);
		});
	}
}
