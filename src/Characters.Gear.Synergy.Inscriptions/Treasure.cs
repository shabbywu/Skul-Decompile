using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.Movements;
using Characters.Operations;
using Data;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Treasure : InscriptionInstance
{
	[Serializable]
	private sealed class HasChest : Ability
	{
		public class Instance : AbilityInstance<HasChest>
		{
			public Instance(Character owner, HasChest ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
				owner.health.onDied += OnDied;
			}

			protected override void OnDetach()
			{
			}

			private void OnDied()
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				ability.treasure.Spawn(Vector2.op_Implicit(((Component)owner).transform.position));
				owner.health.onDied -= OnDied;
			}
		}

		[NonSerialized]
		public Treasure treasure;

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[Serializable]
	public sealed class StageInfo
	{
		[Header("2세트")]
		public CustomFloat goldAmount2Set;

		[Header("4세트")]
		public CustomFloat goldAmount4Set;

		[Range(0f, 100f)]
		[Header("상자 가중치")]
		public int goldChestWeight;

		[Range(0f, 100f)]
		public int currencyBagChestWeight;

		[Range(0f, 100f)]
		public int itemChechWeight;

		[Header("자원 가중치")]
		public CurrencyPossibilities currencyPossibilities;

		public RarityPossibilities currencyRarityPossibilities;

		public CurrencyRangeByRarity goldRangeByRarity;

		public CurrencyRangeByRarity darkQuartzRangeByRarity;

		public CurrencyRangeByRarity boneRangeByRarity;

		[Header("아이템 등급 가중치")]
		public RarityPossibilities itemRarityPossibilities;
	}

	[SerializeField]
	[Header("2세트 효과")]
	private TreasureChest _set2chest;

	[SerializeField]
	[Header("4세트 효과")]
	private TreasureChest _set4chest;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onSpawn;

	[SerializeField]
	private HasChest _hasChest;

	private const int _randomSeed = -612673708;

	private CoroutineReference _reference;

	protected override void Initialize()
	{
		_hasChest.treasure = this;
		_hasChest.Initialize();
		_onSpawn.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += OnMapChangedAndFadeIn;
	}

	private void OnMapChangedAndFadeIn(Map old, Map @new)
	{
		if (keyword.step >= 1 && (Map.Instance.type == Map.Type.Normal || Map.Instance.type == Map.Type.Special))
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			Random seed = new Random(GameData.Save.instance.randomSeed + -612673708 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
			AttachReward(seed);
		}
	}

	private void AttachReward(Random seed)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
		ExtensionMethods.PseudoShuffle<Character>((IList<Character>)allEnemies, seed);
		for (int i = 0; i < allEnemies.Count; i++)
		{
			Character character = allEnemies[i];
			if (character.type == Character.Type.TrashMob && !((Object)(object)character.movement == (Object)null) && character.movement.baseConfig.type != Movement.Config.Type.AcceleratingFlying && character.movement.baseConfig.type != Movement.Config.Type.Flying)
			{
				((CoroutineReference)(ref _reference)).Stop();
				_reference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CTryToAddAbility(character));
				break;
			}
		}
	}

	private IEnumerator CTryToAddAbility(Character enemy)
	{
		while (((Object)(object)enemy != (Object)null && (Object)(object)enemy.ability == (Object)null) || !((Component)enemy).gameObject.activeSelf)
		{
			yield return null;
		}
		if ((Object)(object)enemy != (Object)null)
		{
			enemy.ability.Add(_hasChest);
		}
	}

	public void Spawn(Vector2 point)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		TreasureChest treasureChest = null;
		if (keyword.step >= 1)
		{
			treasureChest = _set2chest;
		}
		if (keyword.step >= 2)
		{
			treasureChest = _set4chest;
		}
		if ((Object)(object)treasureChest != (Object)null)
		{
			_targetPoint.position = Vector2.op_Implicit(point);
			Object.Instantiate<TreasureChest>(treasureChest, Vector2.op_Implicit(point), Quaternion.identity, ((Component)Map.Instance).transform);
			((MonoBehaviour)this).StartCoroutine(_onSpawn.CRun(base.character));
		}
	}

	public override void Detach()
	{
		((CoroutineReference)(ref _reference)).Stop();
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= OnMapChangedAndFadeIn;
	}
}
