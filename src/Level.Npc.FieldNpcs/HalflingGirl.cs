using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Data;
using FX;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Npc.FieldNpcs;

public class HalflingGirl : FieldNpc
{
	private const int _randomSeed = 338118185;

	protected static readonly int _castingHash = Animator.StringToHash("Casting");

	[SerializeField]
	private NpcType _npcType;

	[SerializeField]
	[Space]
	private float _givingAnimatonDuration1;

	[SerializeField]
	private float _givingAnimatonDuration2;

	[SerializeField]
	[Header("Herb")]
	[FormerlySerializedAs("_foodList")]
	private AbilityBuffList _abilityBuffList;

	[SerializeField]
	[FormerlySerializedAs("_foodPossibilities")]
	private RarityPossibilities _abilityPossiblities;

	[Header("Drop")]
	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private EffectInfo _dropEffect;

	[SerializeField]
	private SoundInfo _dropSound;

	private Random _random;

	protected override NpcType _type => _npcType;

	protected override void Start()
	{
		base.Start();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 338118185 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	protected override void Interact(Character character)
	{
		base.Interact(character);
		switch (_phase)
		{
		case Phase.Initial:
		case Phase.Greeted:
			((MonoBehaviour)this).StartCoroutine(CGiveFood());
			break;
		case Phase.Gave:
			((MonoBehaviour)this).StartCoroutine(CChat());
			break;
		}
	}

	private IEnumerator CGiveFood()
	{
		yield return LetterBox.instance.CAppear();
		yield return CGreeting();
		_npcConversation.skippable = true;
		_npcConversation.visible = false;
		_animator.Play(_castingHash, 0, 0f);
		_dropEffect.Spawn(_dropPosition.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dropSound, _dropPosition.position);
		yield return (object)new WaitForSeconds(_givingAnimatonDuration1);
		PlaceFood();
		_phase = Phase.Gave;
		yield return (object)new WaitForSeconds(_givingAnimatonDuration2);
		_animator.Play(FieldNpc._idleHash, 0, 0f);
		_npcConversation.visible = true;
		yield return _npcConversation.CConversation(base._confirmed);
		LetterBox.instance.Disappear();
	}

	private void PlaceFood()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Rarity rarity = _abilityPossiblities.Evaluate();
		AbilityBuff abilityBuff = _abilityBuffList.Take(_random, rarity);
		AbilityBuff abilityBuff2 = Object.Instantiate<AbilityBuff>(abilityBuff, _dropPosition);
		((Object)abilityBuff2).name = ((Object)abilityBuff).name;
		abilityBuff2.price = 0;
		abilityBuff2.Initialize();
	}
}
