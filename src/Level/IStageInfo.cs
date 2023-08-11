using System;
using Characters.Gear.Synergy.Inscriptions;
using GameResources;
using Level.BlackMarket;
using Level.Npc.FieldNpcs;
using UnityEngine;

namespace Level;

public abstract class IStageInfo : ScriptableObject
{
	public MapReference[] maps;

	[SerializeField]
	private AudioClip _music;

	[Space]
	[SerializeField]
	private Sprite _loadingScreenBackground;

	[Space]
	[SerializeField]
	private float _healthMultiplier = 1f;

	[SerializeField]
	private float _adventurerHealthMultiplier = 1f;

	[SerializeField]
	private float _adventurerAttackDamageMultiplier = 1f;

	[SerializeField]
	private float _adventurerCastingBreakDamageMultiplier = 1f;

	[MinMaxSlider(1f, 99f)]
	[SerializeField]
	private Vector2Int _adventurerLevel = new Vector2Int(1, 99);

	[SerializeField]
	private Vector2Int _goldrewardAmount = new Vector2Int(90, 110);

	[SerializeField]
	private Vector2Int _bonerewardAmount = new Vector2Int(90, 110);

	[Space]
	[SerializeField]
	private RarityPossibilities _gearPossibilities;

	[SerializeField]
	private Level.BlackMarket.SettingsByStage _marketSettings;

	[SerializeField]
	private Level.Npc.FieldNpcs.SettingsByStage _fieldNpcSettings;

	[SerializeField]
	private CurrencyRangeByRarity _goldRangeByRarity;

	[SerializeField]
	private CurrencyRangeByRarity _darkQuartzRangeByRarity;

	[SerializeField]
	private CurrencyRangeByRarity _boneRangeByRarity;

	[SerializeField]
	private Treasure.StageInfo _treasureInfo;

	[NonSerialized]
	public int pathIndex = -1;

	[NonSerialized]
	public NodeIndex nodeIndex;

	public AudioClip music => _music;

	public Sprite loadingScreenBackground => _loadingScreenBackground;

	public float healthMultiplier => _healthMultiplier;

	public float adventurerHealthMultiplier => _adventurerHealthMultiplier;

	public float adventurerAttackDamageMultiplier => _adventurerAttackDamageMultiplier;

	public float adventurerCastingBreakDamageMultiplier => _adventurerCastingBreakDamageMultiplier;

	public Vector2Int adventurerLevel => _adventurerLevel;

	public Vector2Int goldrewardAmount => _goldrewardAmount;

	public Vector2Int bonerewardAmount => _bonerewardAmount;

	public RarityPossibilities gearPossibilities => _gearPossibilities;

	public Level.BlackMarket.SettingsByStage marketSettings => _marketSettings;

	public Level.Npc.FieldNpcs.SettingsByStage fieldNpcSettings => _fieldNpcSettings;

	public CurrencyRangeByRarity goldRangeByRarity => _goldRangeByRarity;

	public CurrencyRangeByRarity darkQuartzRangeByRarity => _darkQuartzRangeByRarity;

	public CurrencyRangeByRarity boneRangeByRarity => _boneRangeByRarity;

	public Treasure.StageInfo treasureInfo => _treasureInfo;

	public abstract (PathNode node1, PathNode node2) currentMapPath { get; }

	public abstract (PathNode node1, PathNode node2) nextMapPath { get; }

	public abstract ParallaxBackground background { get; }

	public abstract void Initialize();

	public abstract void Reset();

	public abstract PathNode Next(NodeIndex nodeIndex);

	public abstract void UpdateReferences();
}
