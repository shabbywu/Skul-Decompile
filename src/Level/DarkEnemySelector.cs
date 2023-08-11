using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities.Darks;
using Data;
using FX;
using Hardmode;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

[CreateAssetMenu]
public sealed class DarkEnemySelector : ScriptableObject
{
	private const int _randomSeed = 1177618293;

	private static DarkEnemySelector _instance;

	[SerializeField]
	private EffectInfo _dieEffect;

	[SerializeField]
	private EffectInfo[] _introEffects;

	[SerializeField]
	private EffectInfo[] _dieEffects;

	[SerializeField]
	private SoundInfo _introSound;

	[SerializeField]
	private SoundInfo _dieSound;

	[SerializeField]
	private CharacterHealthBar _healthBar;

	[SerializeField]
	private DarkAbilityConstructor[] _constructors;

	[SerializeField]
	private DarkAbilityConstructor _constructorForTest;

	private int _remainsInStage;

	private int _remainInMap;

	private Random _stageRandom;

	private Random _mapRandom;

	public static DarkEnemySelector instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<DarkEnemySelector>("HardmodeSetting/DarkEnemySelector");
			}
			return _instance;
		}
	}

	public EffectInfo[] introEffects => _introEffects;

	public EffectInfo[] dieEffects => _dieEffects;

	public EffectInfo dieEffect => _dieEffect;

	public CharacterHealthBar healthbar => _healthBar;

	public SoundInfo introSound => _introSound;

	public SoundInfo dieSound => _dieSound;

	public int SetTargetCountInStage()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_stageRandom = new Random(GameData.Save.instance.randomSeed + 1177618293 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16);
		int darkEnemyTotalCount = HardmodeLevelInfo.instance.GetDarkEnemyTotalCount(_stageRandom);
		return _remainsInStage = darkEnemyTotalCount;
	}

	public void SetTargetCountInMap()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_mapRandom = new Random(GameData.Save.instance.randomSeed + 1177618293 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_remainInMap = HardmodeLevelInfo.instance.GetDarkEnemyCountPerMap(_mapRandom);
	}

	public void ElectIn(ICollection<Character> candidates)
	{
		if (!Map.TestingTool.darkenemy || (Singleton<Service>.Instance.levelManager.currentChapter.type != 0 && (_remainInMap <= 0 || _remainsInStage <= 0)))
		{
			return;
		}
		if (candidates.Count <= 0)
		{
			Debug.LogError((object)$"검은적이 배치되어 있지 않습니다. 하드모드 : {GameData.HardmodeProgress.hardmode}, 레벨 {GameData.HardmodeProgress.hardmodeLevel}");
			return;
		}
		Character target = ExtensionMethods.Random<Character>(candidates.Where((Character candidate) => candidate.key != Key.Hound), _mapRandom);
		_constructors[Singleton<HardmodeManager>.Instance.currentLevel].Provide(target);
		_remainsInStage--;
		_remainInMap--;
	}
}
