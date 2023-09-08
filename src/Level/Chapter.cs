using System;
using System.Collections;
using System.Globalization;
using Characters;
using Characters.Controllers;
using Data;
using FX;
using GameResources;
using Hardmode;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Level;

[CreateAssetMenu]
public class Chapter : ScriptableObject
{
	public enum Type
	{
		Test,
		Castle,
		Tutorial,
		Chapter1,
		Chapter2,
		Chapter3,
		Chapter4,
		Chapter5,
		HardmodeCastle,
		HardmodeChapter1,
		HardmodeChapter2,
		HardmodeChapter3,
		HardmodeChapter4,
		HardmodeChapter5
	}

	private Type _chapterType;

	public AssetReference[] stages;

	[SerializeField]
	[Space]
	private int _smallPotionPrice;

	[SerializeField]
	private int _mediumPotionPrice;

	[SerializeField]
	private int _largePotionPrice;

	[SerializeField]
	private float _adventurerGoldRewardMultiplier;

	[Space]
	[SerializeField]
	private int[] _collectorRefreshCosts;

	[Space]
	[SerializeField]
	private Sprite _gateWall;

	[SerializeField]
	private Sprite _gateTable;

	[SerializeField]
	private Sprite _gateChoiceTable;

	[SerializeField]
	private Gate _gatePrefab;

	private int _stageIndex;

	private AsyncOperationHandle<IStageInfo> _currentStageHandle;

	public string chapterName
	{
		get
		{
			Type type = this.type;
			if (Singleton<HardmodeManager>.Instance.hardmode && type >= Type.HardmodeChapter1)
			{
				type = this.type - 6;
			}
			return Localization.GetLocalizedString($"map/{type}");
		}
	}

	public string stageTag
	{
		get
		{
			Type type = this.type;
			if (Singleton<HardmodeManager>.Instance.hardmode && type >= Type.HardmodeChapter1)
			{
				type = this.type - 6;
			}
			return Localization.GetLocalizedString($"map/{type}/{_stageIndex}/tag");
		}
	}

	public string stageName
	{
		get
		{
			Type type = this.type;
			if (Singleton<HardmodeManager>.Instance.hardmode && type >= Type.HardmodeChapter1)
			{
				type = this.type - 6;
			}
			return Localization.GetLocalizedString($"map/{type}/{_stageIndex}");
		}
	}

	public int smallPotionPrice => _smallPotionPrice;

	public int mediumPotionPrice => _mediumPotionPrice;

	public int largePotionPrice => _largePotionPrice;

	public float adventurerGoldRewardMultiplier => _adventurerGoldRewardMultiplier;

	public int[] collectorRefreshCosts => _collectorRefreshCosts;

	public Sprite gateWall => _gateWall;

	public Sprite gateTable => _gateTable;

	public Sprite gateChoiceTable => _gateChoiceTable;

	public Gate gatePrefab => _gatePrefab;

	public Type type { get; private set; }

	public IStageInfo currentStage { get; private set; }

	public int stageIndex => _stageIndex;

	public Map map { get; private set; }

	public void Initialize(Type type)
	{
		this.type = type;
		for (int i = 0; i < stages.Length; i++)
		{
			if (stages[i] == null)
			{
				Debug.LogError((object)string.Format("[{0}] Stage is null : {1}, {2}, {3}", "Chapter", type, i, stages[i]));
			}
		}
		_stageIndex = -1;
		currentStage = null;
	}

	private void ChangeStage(int stageIndex)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (_stageIndex != stageIndex)
		{
			if ((Object)(object)currentStage != (Object)null)
			{
				Resources.UnloadAsset((Object)(object)currentStage);
			}
			if (_currentStageHandle.IsValid())
			{
				Addressables.Release<IStageInfo>(_currentStageHandle);
			}
			_stageIndex = stageIndex;
			_currentStageHandle = stages[stageIndex].LoadAssetAsync<IStageInfo>();
			currentStage = _currentStageHandle.WaitForCompletion();
			currentStage.Initialize();
		}
	}

	public void Enter()
	{
		ChangeStage(0);
		LoadStage();
	}

	public void Enter(int stageIndex, int pathIndex, int nodeIndex)
	{
		ChangeStage(stageIndex);
		LoadStage(pathIndex, nodeIndex);
	}

	public bool Next(NodeIndex nodeIndex)
	{
		PathNode pathNode = currentStage.Next(nodeIndex);
		if (pathNode == null || pathNode.reference == null)
		{
			PoolObject.DespawnAllOrphans();
			PoolObject.ClearUnused();
			if (NextStage())
			{
				return true;
			}
			return false;
		}
		ChangeMap(pathNode);
		return true;
	}

	public void Release()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Clear();
		if (_currentStageHandle.IsValid())
		{
			Addressables.Release<IStageInfo>(_currentStageHandle);
		}
	}

	public void Clear()
	{
		Singleton<EffectPool>.Instance.ClearNonAttached();
		PoolObject.DespawnAllOrphans();
		Singleton<Service>.Instance.levelManager.ClearDrops();
		if ((Object)(object)map != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)map).gameObject);
			map = null;
		}
	}

	public bool NextStage()
	{
		int num = _stageIndex + 1;
		if (num == stages.Length)
		{
			return false;
		}
		ChangeStage(num);
		LoadStage();
		return true;
	}

	private void LoadStage(int pathIndex = 0, int nodeIndex = 0)
	{
		Debug.Log((object)$"[Load Stage] {type}, stage {stageIndex} index {pathIndex}, nodeIndex {nodeIndex}");
		if ((Object)(object)currentStage.music == (Object)null)
		{
			PersistentSingleton<SoundManager>.Instance.FadeOutBackgroundMusic();
		}
		else
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(currentStage.music);
		}
		currentStage.Reset();
		currentStage.pathIndex = pathIndex;
		PathNode item = currentStage.currentMapPath.node1;
		PathNode item2 = currentStage.currentMapPath.node2;
		PathNode pathNode = item;
		bool num = item != null && item.reference != null;
		bool flag = item2 != null && item2.reference != null;
		if (!num || (nodeIndex == 1 && flag))
		{
			pathNode = item2;
		}
		if (!TryGetLoadingScreenData(out var loadingScreenData))
		{
			((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CChangeMap(pathNode));
		}
		else
		{
			((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CChangeMap(pathNode, loadingScreenData));
		}
	}

	private bool TryGetLoadingScreenData(out LoadingScreen.LoadingScreenData loadingScreenData)
	{
		if ((Object)(object)currentStage.loadingScreenBackground == (Object)null)
		{
			loadingScreenData = default(LoadingScreen.LoadingScreenData);
			return false;
		}
		string description;
		if (MMMaths.RandomBool())
		{
			if (!Localization.TryGetLocalizedStringArray($"loading/description/{type}/{_stageIndex}", out var strings))
			{
				strings = Localization.GetLocalizedStringArray("loading/description/common");
			}
			description = strings.Random();
		}
		else
		{
			description = Localization.GetLocalizedStringArray("loading/description/common").Random();
		}
		AnimationClip walkClip = null;
		Character player = Singleton<Service>.Instance.levelManager.player;
		if ((Object)(object)player != (Object)null)
		{
			walkClip = player.playerComponents.inventory.weapon.polymorphOrCurrent.characterAnimation.walkClip;
		}
		if (type == Type.Castle || (type == Type.Chapter1 && stageIndex == 0))
		{
			loadingScreenData = new LoadingScreen.LoadingScreenData(currentStage.loadingScreenBackground, walkClip, stageName, description);
		}
		else
		{
			string currentTime = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\ \\:\\ mm\\ \\:\\ ss", CultureInfo.InvariantCulture);
			string key = $"{type}/{_stageIndex}";
			bool bestTimeUpdated = GameData.Record.UpdateBestTime(key);
			string bestTime = new TimeSpan(0, 0, GameData.Record.GetBestTime(key)).ToString("hh\\ \\:\\ mm\\ \\:\\ ss", CultureInfo.InvariantCulture);
			loadingScreenData = new LoadingScreen.LoadingScreenData(currentStage.loadingScreenBackground, walkClip, stageName, description, currentTime, bestTime, bestTimeUpdated);
		}
		return true;
	}

	public void ChangeMap(PathNode pathNode)
	{
		((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CChangeMap(pathNode));
	}

	private IEnumerator CChangeMap(PathNode pathNode, LoadingScreen.LoadingScreenData? loadingScreenData = null)
	{
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		PlayerInput.blocked.Attach(this);
		if ((Object)(object)levelManager.player != (Object)null && levelManager.player.invulnerable != null)
		{
			levelManager.player.invulnerable.Attach(this);
		}
		if ((Object)(object)map == (Object)null)
		{
			Chronometer.global.AttachTimeScale(this, 0f);
			Singleton<Service>.Instance.fadeInOut.FadeOutImmediately();
		}
		else
		{
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			Chronometer.global.AttachTimeScale(this, 0f);
			Clear();
		}
		yield return Resources.UnloadUnusedAssets();
		if (SystemInfo.systemMemorySize <= 4096)
		{
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
			yield return Resources.UnloadUnusedAssets();
		}
		MapRequest newMapRequest = pathNode.reference.LoadAsync();
		do
		{
			yield return null;
		}
		while (!newMapRequest.isDone);
		if (pathNode.reference.darkEnemy)
		{
			DarkEnemySelector.instance.SetTargetCountInMap();
		}
		map = Object.Instantiate<Map>(newMapRequest.asset, Vector3.zero, Quaternion.identity);
		ReleaseAddressableHandleOnDestroy.Reserve(((Component)map).gameObject, newMapRequest.handle);
		if (!string.IsNullOrWhiteSpace(chapterName) && !string.IsNullOrWhiteSpace(stageName) && map.displayStageName)
		{
			Scene<GameBase>.instance.uiManager.stageName.Show(chapterName, stageTag, stageName);
		}
		map.darkEnemy = pathNode.reference.darkEnemy;
		map.SetReward(pathNode.reward);
		map.SetExits(currentStage.nextMapPath.node1, currentStage.nextMapPath.node2);
		levelManager.SpawnPlayerIfNotExist();
		ResetPlayerPosition();
		((MonoBehaviour)(object)levelManager).ExcuteInNextFrame(ResetPlayerPosition);
		GameBase instance = Scene<GameBase>.instance;
		Vector3 position = ((Component)instance.cameraController).transform.position;
		Vector3 val = -map.backgroundOrigin;
		val.z = position.z;
		instance.cameraController.Move(map.playerOrigin);
		instance.minimapCameraController.Move(map.playerOrigin);
		instance.SetBackground(((Object)(object)map.background == (Object)null) ? currentStage.background : map.background, map.playerOrigin.y - map.backgroundOrigin.y);
		GameData.Currency.SaveAll();
		GameData.Progress.SaveAll();
		GameData.HardmodeProgress.SaveAll();
		PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
		levelManager.InvokeOnMapChanged();
		Singleton<Service>.Instance.fadeInOut.HideLoadingIcon();
		yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
		Chronometer.global.DetachTimeScale(this);
		PlayerInput.blocked.Detach(this);
		((MonoBehaviour)levelManager.player).StartCoroutine(CDetachInvulnerableInSecond());
		levelManager.InvokeOnMapChangedAndFadeIn(map);
		IEnumerator CDetachInvulnerableInSecond()
		{
			yield return (object)new WaitForSeconds(1f);
			levelManager.player.invulnerable.Detach(this);
		}
		void ResetPlayerPosition()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			((Component)levelManager.player).transform.position = map.playerOrigin;
			levelManager.player.movement.controller.ResetBounds();
			Physics2D.SyncTransforms();
		}
	}

	public void EnterOutTrack(Map newMap)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		levelManager.ClearDrops();
		if ((Object)(object)map != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)map).gameObject);
		}
		map = Object.Instantiate<Map>(newMap, Vector3.zero, Quaternion.identity);
		((Component)levelManager.player).transform.position = map.playerOrigin;
		levelManager.player.movement.controller.ResetBounds();
		Physics2D.SyncTransforms();
		GameBase instance = Scene<GameBase>.instance;
		Vector3 position = ((Component)instance.cameraController).transform.position;
		Vector3 val = -map.backgroundOrigin;
		val.z = position.z;
		instance.cameraController.Move(map.playerOrigin);
		instance.minimapCameraController.Move(map.playerOrigin);
		instance.SetBackground(((Object)(object)map.background == (Object)null) ? currentStage.background : map.background, map.playerOrigin.y - map.backgroundOrigin.y);
	}
}
