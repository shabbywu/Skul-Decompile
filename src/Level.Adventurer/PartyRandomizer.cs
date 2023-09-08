using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters;
using CutScenes;
using Data;
using Hardmode;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Level.Adventurer;

[Serializable]
public class PartyRandomizer
{
	[Serializable]
	private class AdventurerReference
	{
		[SerializeField]
		internal AssetReference _character;

		[SerializeField]
		internal AssetReference _supportingCharacter;
	}

	private const int _randomSeed = 1020464638;

	[SerializeField]
	private AdventurerReference[] _adventurerReferences;

	[SerializeField]
	private AssetReference _firstMeetingCharacterReference;

	[SerializeField]
	private AssetReference _tutorialSupportingHeroReference;

	[SerializeField]
	private AssetReference _tutorialWarriorReference;

	[SerializeField]
	private Transform[] _spawnPoints;

	[SerializeField]
	private Transform _supportingCharacterSpawnPoint;

	[SerializeField]
	private EnemyWave _enemyWave;

	private Random _random;

	private string _waveClearKey = "WaveClear";

	private string _patternKey = "Pattern";

	public void Initialize()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 1020464638 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_adventurerReferences.PseudoShuffle(_random);
	}

	private Character SpawnCharacter(AssetReference reference, Transform transform, Vector3 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		AsyncOperationHandle<GameObject> val = Addressables.LoadAssetAsync<GameObject>((object)reference);
		Character result = Object.Instantiate<Character>(val.WaitForCompletion().GetComponent<Character>(), position, Quaternion.identity, transform);
		Addressables.Release<GameObject>(val);
		return result;
	}

	public List<Character> SpawnCharacters()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		List<Character> list = new List<Character>();
		if (_adventurerReferences.Length < _spawnPoints.Length)
		{
			Debug.LogError((object)"캐릭터 배열의 크기가 작습니다");
			return list;
		}
		if (CanPlayHardmodeCutScene())
		{
			if (_spawnPoints.Length == 0)
			{
				Debug.LogError((object)"캐릭터 배열의 크기가 작습니다");
				return list;
			}
			Character item = SpawnCharacter(_tutorialWarriorReference, ((Component)_enemyWave).transform, _spawnPoints[0].position);
			list.Add(item);
		}
		else if (CanPlayNormalmodeCutScene())
		{
			if (_spawnPoints.Length == 0)
			{
				Debug.LogError((object)"캐릭터 배열의 크기가 작습니다");
				return list;
			}
			Character item2 = SpawnCharacter(_firstMeetingCharacterReference, ((Component)_enemyWave).transform, _spawnPoints[0].position);
			list.Add(item2);
		}
		else
		{
			for (int i = 0; i < _spawnPoints.Length; i++)
			{
				Character item3 = SpawnCharacter(_adventurerReferences[i]._character, ((Component)_enemyWave).transform, _spawnPoints[i].position);
				list.Add(item3);
			}
		}
		_enemyWave.Initialize();
		return list;
	}

	public Character SpawnSupportingCharacter()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		if (_adventurerReferences.Length <= _spawnPoints.Length)
		{
			Debug.LogError((object)"캐릭터 배열의 크기가 작습니다");
			return null;
		}
		if (CanPlayHardmodeCutScene())
		{
			Character character = SpawnCharacter(_tutorialSupportingHeroReference, ((Component)_enemyWave).transform, _supportingCharacterSpawnPoint.position);
			BehaviorDesignerCommunicator tutorialHeroCommunicator = ((Component)character).GetComponent<BehaviorDesignerCommunicator>();
			_enemyWave.onClear += delegate
			{
				tutorialHeroCommunicator.SetVariable<SharedBool>(_waveClearKey, (object)true);
			};
			return character;
		}
		AssetReference supportingCharacter = _adventurerReferences[_spawnPoints.Length]._supportingCharacter;
		if (supportingCharacter == null || (Object)(object)_supportingCharacterSpawnPoint == (Object)null)
		{
			return null;
		}
		Character character2 = SpawnCharacter(supportingCharacter, ((Component)_enemyWave).transform, _supportingCharacterSpawnPoint.position);
		BehaviorDesignerCommunicator communicator = ((Component)character2).GetComponent<BehaviorDesignerCommunicator>();
		_enemyWave.onClear += delegate
		{
			communicator.SetVariable<SharedBool>(_waveClearKey, (object)true);
		};
		int[] enumerable = new int[2] { 0, 1 };
		communicator.SetVariable<SharedInt>(_patternKey, (object)enumerable.Random(_random));
		return character2;
	}

	public bool CanPlayNormalmodeCutScene()
	{
		if (!Singleton<HardmodeManager>.Instance.hardmode && _firstMeetingCharacterReference != null && _firstMeetingCharacterReference.RuntimeKeyIsValid())
		{
			return !GameData.Progress.cutscene.GetData(CutScenes.Key.rookieHero);
		}
		return false;
	}

	public bool CanPlayHardmodeCutScene()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.supportingAdventurer_First) && _tutorialSupportingHeroReference != null && _tutorialWarriorReference != null && _tutorialSupportingHeroReference.RuntimeKeyIsValid())
		{
			return _tutorialWarriorReference.RuntimeKeyIsValid();
		}
		return false;
	}
}
