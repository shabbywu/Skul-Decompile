using System.Collections;
using Characters;
using Characters.Actions;
using Characters.Controllers;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace CutScenes;

public class BlackMarCat : MonoBehaviour
{
	private enum ActionType
	{
		Appearance,
		Walk,
		Freeze,
		Amazing,
		AmazingFreeze,
		Idle,
		Bye
	}

	private enum TextType
	{
		Idle_01,
		Idle_02,
		Idle_03,
		Idle_04,
		Idle_05,
		Idle_06,
		Idle_07,
		Idle_08,
		Idle_09,
		Idle_10,
		Idle_11
	}

	private static readonly string _nameKey = "CutScene/name/StrangeCat";

	private static readonly string _textKey = "CutScene/BlackMarketIntro/StrangeCat/0";

	[SerializeField]
	private Character _character;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Transform _talkPoint;

	[SerializeField]
	private Transform _endPoint;

	[SerializeField]
	private Action[] _actions;

	private string[] _texts;

	private void Start()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (GameData.Progress.cutscene.GetData(Key.strangeCat))
		{
			((Component)this).transform.position = _endPoint.position;
			((Renderer)_spriteRenderer).enabled = true;
			return;
		}
		_texts = Localization.GetLocalizedStringArray(_textKey);
		if (_texts == null || _texts.Length < 10)
		{
			((Component)this).transform.position = _endPoint.position;
			((Renderer)_spriteRenderer).enabled = true;
		}
		else
		{
			Activate();
		}
	}

	private IEnumerator CSmallTalk()
	{
		NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		_character.ForceToLookAt(Character.LookingDirection.Left);
		_actions[0].TryStart();
		((Renderer)_spriteRenderer).enabled = true;
		while (_actions[0].running)
		{
			yield return null;
		}
		yield return MoveTo(_character, _talkPoint.position);
		_actions[2].TryStart();
		while (_actions[2].running)
		{
			yield return null;
		}
		_actions[3].TryStart();
		while (_actions[3].running)
		{
			yield return null;
		}
		_actions[4].TryStart();
		while (_actions[4].running)
		{
			yield return null;
		}
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[0]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[1]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[2]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[3]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[4]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[5]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[6]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[7]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[8]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[9]);
		yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(_nameKey), _texts[10]);
		_actions[6].TryStart();
		while (_actions[6].running)
		{
			yield return null;
		}
		npcConversation.Done();
		yield return MoveTo(_character, _endPoint.position);
		_character.ForceToLookAt(Character.LookingDirection.Right);
		Deactivate();
	}

	private void OnDestroy()
	{
		Scene<GameBase>.instance.uiManager.npcConversation.Done();
		PlayerInput.blocked.Detach((object)this);
		Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
		Singleton<Service>.Instance.levelManager.player.movement.blocked.Detach((object)this);
	}

	public void Activate()
	{
		PlayerInput.blocked.Attach((object)this);
		Scene<GameBase>.instance.uiManager.headupDisplay.visible = false;
		Character player = Singleton<Service>.Instance.levelManager.player;
		player.ForceToLookAt(Character.LookingDirection.Right);
		player.movement.blocked.Attach((object)this);
		((MonoBehaviour)this).StartCoroutine(CActivate());
	}

	private IEnumerator CActivate()
	{
		yield return Scene<GameBase>.instance.uiManager.letterBox.CAppear();
		((MonoBehaviour)this).StartCoroutine(CSmallTalk());
	}

	private void Deactivate()
	{
		PlayerInput.blocked.Detach((object)this);
		Scene<GameBase>.instance.uiManager.npcConversation.Done();
		((MonoBehaviour)this).StartCoroutine(CDeactivate());
		GameData.Progress.cutscene.SetData(Key.strangeCat, value: true);
	}

	private IEnumerator CDeactivate()
	{
		yield return Scene<GameBase>.instance.uiManager.letterBox.CDisappear();
		PlayerInput.blocked.Detach((object)this);
		Singleton<Service>.Instance.levelManager.player.movement.blocked.Detach((object)this);
		Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
	}

	private IEnumerator MoveTo(Character owner, Vector3 destination)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		while (true)
		{
			float num = destination.x - ((Component)owner).transform.position.x;
			if (!(Mathf.Abs(num) < 0.5f))
			{
				Vector2 move = ((num > 0f) ? Vector2.right : Vector2.left);
				owner.movement.move = move;
				yield return null;
				continue;
			}
			break;
		}
	}
}
