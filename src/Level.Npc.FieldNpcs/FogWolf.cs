using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Characters.Abilities.Savable;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public class FogWolf : FieldNpc
{
	private const int _randomSeed = 2028506624;

	[Header("Effects")]
	[Tooltip("버프 부여 후 이 시간 동안 대화창이 잠시 사라집니다.")]
	[SerializeField]
	private float _effectShowingDuration;

	[SerializeField]
	private EffectInfo _givingBuffEffect;

	[SerializeField]
	private SoundInfo _givingBuffSound;

	[SerializeField]
	private EffectInfo _takingBuffEffect;

	[SerializeField]
	private SoundInfo _takingBuffSound;

	[SerializeField]
	[Header("Buffs")]
	private string[] _floatingKeyArray = new string[5];

	private Random _random;

	protected override NpcType _type => NpcType.FogWolf;

	protected override void Start()
	{
		base.Start();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	protected override void Interact(Character character)
	{
		base.Interact(character);
		switch (_phase)
		{
		case Phase.Initial:
		case Phase.Greeted:
			((MonoBehaviour)this).StartCoroutine(CGiveBuff(character));
			break;
		case Phase.Gave:
			((MonoBehaviour)this).StartCoroutine(CChat());
			break;
		}
	}

	private IEnumerator CGiveBuff(Character character)
	{
		yield return LetterBox.instance.CAppear();
		yield return CGreeting();
		int buffIndex = _random.Next(0, FogWolfBuff.buffCount);
		character.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.FogWolfBuff, buffIndex);
		_phase = Phase.Gave;
		_npcConversation.skippable = true;
		_givingBuffEffect.Spawn(((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_givingBuffSound, ((Component)this).transform.position);
		NpcConversation npcConversation = _npcConversation;
		bool flag = false;
		npcConversation.visible = false;
		yield return flag;
		yield return (object)new WaitForSeconds(_effectShowingDuration);
		NpcConversation npcConversation2 = _npcConversation;
		flag = true;
		npcConversation2.visible = true;
		yield return flag;
		Bounds bounds = ((Collider2D)character.collider).bounds;
		float x = ((Bounds)(ref bounds)).center.x;
		bounds = ((Collider2D)character.collider).bounds;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(x, ((Bounds)(ref bounds)).max.y + 0.5f);
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(Localization.GetLocalizedString(_floatingKeyArray[buffIndex]), Vector2.op_Implicit(val));
		_takingBuffEffect.Spawn(((Component)character).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_takingBuffSound, ((Component)character).transform.position);
		yield return _npcConversation.CConversation(base._confirmed[buffIndex]);
		LetterBox.instance.Disappear();
	}
}
