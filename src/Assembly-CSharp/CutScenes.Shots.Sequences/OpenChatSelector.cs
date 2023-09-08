using System.Collections;
using GameResources;
using Scenes;
using UI;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots.Sequences;

public class OpenChatSelector : Sequence
{
	[SerializeField]
	private Sprite _portarit;

	[SerializeField]
	private string _nameKey;

	[SerializeField]
	private bool _randomText;

	[SerializeField]
	private string _textKey;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(ShotInfo))]
	private ShotInfo.Subcomponents _onChat;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(ShotInfo))]
	private ShotInfo.Subcomponents _onClose;

	public override IEnumerator CRun()
	{
		NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		npcConversation.OpenChatSelector(delegate
		{
			_onChat.Run(null, null);
		}, delegate
		{
			_onClose.Run(null, null);
		});
		npcConversation.portrait = _portarit;
		npcConversation.name = Localization.GetLocalizedString(_nameKey);
		if (_randomText)
		{
			npcConversation.body = Localization.GetLocalizedStringArray(_textKey).Random();
		}
		else
		{
			npcConversation.body = Localization.GetLocalizedString(_textKey);
		}
		yield return npcConversation.CType();
	}
}
