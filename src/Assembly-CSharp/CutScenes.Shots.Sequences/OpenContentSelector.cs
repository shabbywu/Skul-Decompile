using System.Collections;
using GameResources;
using Scenes;
using UI;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots.Sequences;

public class OpenContentSelector : Sequence
{
	[SerializeField]
	[Header("Body")]
	private string _nameKey;

	[SerializeField]
	private string _textKey;

	[SerializeField]
	private Sprite _portrait;

	[SerializeField]
	[Header("Label")]
	private string _contentsLabelKey;

	[SerializeField]
	private string _cancelLabelKey = "label/confirm/no";

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(ShotInfo))]
	private ShotInfo.Subcomponents _onContents;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(ShotInfo))]
	private ShotInfo.Subcomponents _onClose;

	public override IEnumerator CRun()
	{
		NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		npcConversation.portrait = _portrait;
		npcConversation.name = Localization.GetLocalizedString(_nameKey);
		npcConversation.body = Localization.GetLocalizedString(_textKey);
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
