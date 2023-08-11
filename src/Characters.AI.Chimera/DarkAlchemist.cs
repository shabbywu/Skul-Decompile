using System.Collections;
using Characters.Actions;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Characters.AI.Chimera;

public class DarkAlchemist : MonoBehaviour
{
	private enum TextType
	{
		Idle_01,
		Idle_02,
		Idle_03,
		Idle_04,
		Idle_05,
		Laugh_01,
		Crazy_01,
		CrazyLaugh_01,
		Wait_01,
		Disappoint_01,
		Disappoint_02,
		Attack_Fire_01,
		Attack_Fire_02,
		Attack_Second2_01
	}

	private enum ActionType
	{
		Laugh,
		Crazy,
		CrazyLaugh,
		Wait,
		Back_LoopX,
		Disappoint,
		Stand_LoopX,
		Attack_Fire,
		Attack_Second_LoopX,
		Attack_Second2,
		Dead
	}

	private static readonly string _nameKey = "CutScene/name/DarkLabChief";

	private static readonly string _textKey = "CutScene/Ch3BossIntro/DarkLabChief/0";

	[SerializeField]
	private Chapter3Script _script;

	[SerializeField]
	private Character _charcter;

	[SerializeField]
	private ParticleEffectInfo _particleEffectInfo;

	[SerializeField]
	private Action[] _actions;

	private string[] _texts;

	private void Start()
	{
		_texts = Localization.GetLocalizedStringArray(_textKey);
		_charcter.health.onDied += delegate
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			_particleEffectInfo.Emit(Vector2.op_Implicit(((Component)_charcter).transform.position), ((Collider2D)_charcter.collider).bounds, (Vector2.up + Vector2.left) * 4f);
		};
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			LetterBox.instance.Disappear();
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			if (!((Object)(object)Scene<GameBase>.instance.cameraController == (Object)null) && !((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
			{
				Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
			}
		}
	}

	private IEnumerator CSmallTalk()
	{
		NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		npcConversation.portrait = null;
		npcConversation.skippable = true;
		npcConversation.name = Localization.GetLocalizedString(_nameKey);
		yield return Chronometer.global.WaitForSeconds(3f);
		yield return npcConversation.CConversation(_texts[0], _texts[1], _texts[2], _texts[3], _texts[4]);
		_actions[0]?.TryStart();
		yield return npcConversation.CConversation(_texts[5]);
		_actions[1]?.TryStart();
		yield return npcConversation.CConversation(_texts[6]);
		_actions[2]?.TryStart();
		yield return npcConversation.CConversation(_texts[7]);
		_actions[3]?.TryStart();
		yield return npcConversation.CConversation(_texts[8]);
		Scene<GameBase>.instance.uiManager.npcConversation.Done();
		_actions[4]?.TryStart();
		while (_actions[4].running)
		{
			yield return null;
		}
		_actions[5]?.TryStart();
		yield return npcConversation.CConversation(_texts[9]);
		yield return npcConversation.CConversation(_texts[10]);
		Scene<GameBase>.instance.uiManager.npcConversation.Done();
		_actions[6]?.TryStart();
		while (_actions[6].running)
		{
			yield return null;
		}
		_actions[7]?.TryStart();
		yield return npcConversation.CConversation(_texts[11]);
		yield return npcConversation.CConversation(_texts[12]);
		Scene<GameBase>.instance.uiManager.npcConversation.Done();
		_actions[8]?.TryStart();
		while (_actions[8].running)
		{
			yield return null;
		}
		_actions[9]?.TryStart();
		yield return npcConversation.CConversation(_texts[13]);
		_actions[10]?.TryStart();
		Deactivate();
	}

	public void Activate()
	{
		((MonoBehaviour)this).StartCoroutine(CSmallTalk());
	}

	private IEnumerator CActivate()
	{
		yield return Scene<GameBase>.instance.uiManager.letterBox.CAppear();
		((MonoBehaviour)this).StartCoroutine(CSmallTalk());
	}

	private void Deactivate()
	{
		Scene<GameBase>.instance.uiManager.npcConversation.Done();
		((MonoBehaviour)this).StartCoroutine(CDeactivate());
	}

	private IEnumerator CDeactivate()
	{
		yield return Scene<GameBase>.instance.uiManager.letterBox.CDisappear();
		_script.EndIntro();
	}
}
