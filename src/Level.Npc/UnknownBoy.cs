using System.Collections;
using System.Collections.Generic;
using Characters;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc;

public class UnknownBoy : InteractiveObject
{
	private enum QuestState
	{
		Wait,
		Accepted,
		Cleared,
		GaveReward
	}

	[SerializeField]
	private Transform _body;

	[SerializeField]
	[GetComponent]
	private Collider2D _collider;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private EnemyWave _startWave;

	[SerializeField]
	private Transform _rewardConversationPoint;

	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private GameObject _lineText;

	[SerializeField]
	private RarityPossibilities _essencePossibilities;

	[SerializeField]
	private EffectInfo _outEffectInfo;

	[SerializeField]
	private EffectInfo _inEffectInfo;

	private QuestState _questState;

	private NpcConversation _npcConversation;

	private EssenceRequest _essenceRequest;

	private string displayName => Localization.GetLocalizedString("npc/essence/unknownboy/name");

	private string[] questScripts => Localization.GetLocalizedStringArray("npc/essence/unknownboy/quest/0");

	private string[] rewardScripts => Localization.GetLocalizedStringArray("npc/essence/unknownboy/reward/0");

	private string[] chatScripts => ExtensionMethods.Random<string[]>((IEnumerable<string[]>)Localization.GetLocalizedStringArrays("npc/essence/unknownboy/chat"));

	protected override void Awake()
	{
		base.Awake();
		_npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		_npcConversation.name = displayName;
		_npcConversation.skippable = true;
		_npcConversation.portrait = null;
		_questState = QuestState.Wait;
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_essenceRequest = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(_essencePossibilities.Evaluate()).LoadAsync();
	}

	private void OnDestroy()
	{
		_essenceRequest?.Release();
	}

	public override void InteractWith(Character character)
	{
		if (_questState == QuestState.Cleared)
		{
			((MonoBehaviour)this).StartCoroutine(CRewardConversation());
		}
		else if (_questState == QuestState.Wait)
		{
			((MonoBehaviour)this).StartCoroutine(CAcceptQuest());
		}
		else if (_questState == QuestState.GaveReward)
		{
			((MonoBehaviour)this).StartCoroutine(CChat());
		}
	}

	private IEnumerator CAcceptQuest()
	{
		LetterBox.instance.Appear();
		yield return _npcConversation.CConversation(questScripts);
		LetterBox.instance.Disappear();
		_questState = QuestState.Accepted;
		_startWave.Spawn();
		((MonoBehaviour)this).StartCoroutine(CCheckWaveAllCleared());
	}

	private void OnDisable()
	{
		_npcConversation.portrait = null;
	}

	private IEnumerator CCheckWaveAllCleared()
	{
		EnemyWave[] waves = Map.Instance.waveContainer.enemyWaves;
		Transform player = ((Component)Singleton<Service>.Instance.levelManager.player).transform;
		((Behaviour)_collider).enabled = false;
		while (true)
		{
			if (((Component)this).transform.position.x < player.position.x)
			{
				_body.localScale = Vector2.op_Implicit(new Vector2(-1f, 1f));
			}
			else
			{
				_body.localScale = Vector2.op_Implicit(new Vector2(1f, 1f));
			}
			bool flag = true;
			EnemyWave[] array = waves;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].state != Wave.State.Cleared)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			yield return null;
		}
		_questState = QuestState.Cleared;
		_inEffectInfo.Spawn(((Component)this).transform.position);
		((Component)this).transform.position = ((Component)_rewardConversationPoint).transform.position;
		_outEffectInfo.Spawn(((Component)this).transform.position);
		_body.localScale = Vector3.one;
		((Behaviour)_collider).enabled = true;
		((Renderer)_spriteRenderer).sortingLayerName = "Enemy";
		_lineText.SetActive(false);
	}

	private IEnumerator CChat()
	{
		LetterBox.instance.Appear();
		yield return _npcConversation.CConversation(ExtensionMethods.Random<string>((IEnumerable<string>)chatScripts));
		LetterBox.instance.Disappear();
	}

	private IEnumerator CRewardConversation()
	{
		LetterBox.instance.Appear();
		yield return _npcConversation.CConversation(ExtensionMethods.Random<string>((IEnumerable<string>)rewardScripts));
		LetterBox.instance.Disappear();
		while (!_essenceRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.levelManager.DropQuintessence(_essenceRequest, _dropPosition.position);
		_questState = QuestState.GaveReward;
	}
}
