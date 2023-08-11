using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Data;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Level;

public sealed class LevelManager : MonoBehaviour
{
	public delegate void OnMapChangedDelegate(Map old, Map @new);

	[SerializeField]
	private AbilityBuffList _abilityBuffList;

	private Map _oldMap;

	private List<PoolObject> _drops = new List<PoolObject>();

	private int _prevChapterIndex = -1;

	private int _currentChapterIndex = -1;

	private AsyncOperationHandle<Chapter> _currentChapterHandle;

	public int prevChapterIndex => _prevChapterIndex;

	public bool skulSpawnAnimaionEnable { get; set; } = true;


	public bool skulPortalUsed { get; set; }

	public Character player { get; private set; }

	public Chapter currentChapter { get; private set; }

	public bool clearing { get; private set; }

	public event Action onMapLoaded;

	public event Action onMapLoadedAndFadedIn;

	public event OnMapChangedDelegate onMapChangedAndFadedIn;

	public event Action onChapterLoaded;

	public event Action onActivateMapReward;

	public void ClearEvents()
	{
		this.onMapLoaded = null;
		this.onMapLoadedAndFadedIn = null;
		this.onMapChangedAndFadedIn = null;
		this.onChapterLoaded = null;
	}

	public void ClearDrops()
	{
		clearing = true;
		for (int num = _drops.Count - 1; num >= 0; num--)
		{
			_drops[num].Despawn();
		}
		_drops.Clear();
		clearing = false;
	}

	public void RegisterDrop(PoolObject poolObject)
	{
		_drops.Add(poolObject);
	}

	public void DeregisterDrop(PoolObject poolObject)
	{
		if (!clearing)
		{
			_drops.Remove(poolObject);
		}
	}

	public void DropGold(int amount, int count)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		DropGold(amount, count, ((Component)player).transform.position);
	}

	public void DropGold(int amount, int count, Vector3 position)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		DropCurrency(GameData.Currency.Type.Gold, amount, count, position);
	}

	public void DropGold(int amount, int count, Vector3 position, Vector2 force)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		DropCurrency(GameData.Currency.Type.Gold, amount, count, position, force);
	}

	public void DropDarkQuartz(int amount)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		DropDarkQuartz(amount, ((Component)player).transform.position);
	}

	public void DropDarkQuartz(int amount, Vector3 position)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		DropDarkQuartz(amount, Mathf.Min(amount, 20), position);
	}

	public void DropDarkQuartz(int amount, Vector3 position, Vector2 force)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		DropDarkQuartz(amount, Mathf.Min(amount, 20), position, force);
	}

	public void DropDarkQuartz(int amount, int count, Vector3 position)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		DropCurrency(GameData.Currency.Type.DarkQuartz, amount, count, position);
	}

	public void DropDarkQuartz(int amount, int count, Vector3 position, Vector2 force)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		DropCurrency(GameData.Currency.Type.DarkQuartz, amount, count, position, force);
	}

	public void DropBone(int amount, int count)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		DropBone(amount, count, ((Component)player).transform.position);
	}

	public void DropBone(int amount, int count, Vector3 position)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		DropCurrency(GameData.Currency.Type.Bone, amount, count, position);
	}

	public void DropBone(int amount, int count, Vector3 position, Vector2 force)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		DropCurrency(GameData.Currency.Type.Bone, amount, count, position, force);
	}

	public void DropCurrency(GameData.Currency.Type type, int amount, int count, Vector3 position)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (count == 0)
		{
			return;
		}
		int currencyAmount = amount / count;
		PoolObject currencyParticle = CommonResource.instance.GetCurrencyParticle(type);
		for (int i = 0; i < count; i++)
		{
			CurrencyParticle component = ((Component)currencyParticle.Spawn(position, true)).GetComponent<CurrencyParticle>();
			component.currencyType = type;
			component.currencyAmount = currencyAmount;
			if (i == 0)
			{
				component.currencyAmount += amount % count;
			}
		}
	}

	public void DropCurrencyBag(GameData.Currency.Type type, Rarity rarity, int amount, int count, Vector3 position)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (count != 0)
		{
			CurrencyBag currencyBag = CurrencyBagResource.instance.GetCurrencyBag(type, rarity);
			CurrencyBag currencyBag2 = Object.Instantiate<CurrencyBag>(currencyBag, position, Quaternion.identity, ((Component)Map.Instance).transform);
			((Object)currencyBag2).name = ((Object)currencyBag).name;
			currencyBag2.amount = amount;
			currencyBag2.count = count;
		}
	}

	public void DropCurrency(GameData.Currency.Type type, int amount, int count, Vector3 position, Vector2 force)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (count == 0)
		{
			return;
		}
		int currencyAmount = amount / count;
		PoolObject currencyParticle = CommonResource.instance.GetCurrencyParticle(type);
		for (int i = 0; i < count; i++)
		{
			CurrencyParticle component = ((Component)currencyParticle.Spawn(position, true)).GetComponent<CurrencyParticle>();
			component.currencyType = type;
			component.currencyAmount = currencyAmount;
			component.SetForce(force);
			if (i == 0)
			{
				component.currencyAmount += amount % count;
			}
		}
	}

	public Weapon DropWeapon(WeaponRequest gearRequest, Vector3 position)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Weapon weapon = DropWeapon(gearRequest.asset, position);
		ReleaseAddressableHandleOnDestroy.Reserve(((Component)weapon).gameObject, gearRequest.handle);
		gearRequest.SetReleaseReserved();
		return weapon;
	}

	public Weapon DropWeapon(Weapon weapon, Vector3 position)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)weapon == (Object)null)
		{
			return null;
		}
		Weapon weapon2 = Object.Instantiate<Weapon>(weapon, position, Quaternion.identity);
		((Object)weapon2).name = ((Object)weapon).name;
		((Component)weapon2).transform.parent = ((Component)Map.Instance).transform;
		weapon2.Initialize();
		return weapon2;
	}

	public Item DropItem(ItemRequest gearRequest, Vector3 position)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Item item = DropItem(gearRequest.asset, position);
		ReleaseAddressableHandleOnDestroy.Reserve(((Component)item).gameObject, gearRequest.handle);
		gearRequest.SetReleaseReserved();
		return item;
	}

	public Item DropItem(Item item, Vector3 position)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)item == (Object)null)
		{
			return null;
		}
		Item item2 = Object.Instantiate<Item>(item, position, Quaternion.identity);
		((Object)item2).name = ((Object)item).name;
		((Component)item2).transform.parent = ((Component)Map.Instance).transform;
		item2.Initialize();
		return item2;
	}

	public Quintessence DropQuintessence(EssenceRequest gearRequest, Vector3 position)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Quintessence quintessence = DropQuintessence(gearRequest.asset, position);
		ReleaseAddressableHandleOnDestroy.Reserve(((Component)quintessence).gameObject, gearRequest.handle);
		gearRequest.SetReleaseReserved();
		return quintessence;
	}

	public Quintessence DropQuintessence(Quintessence quintessence, Vector3 position)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)quintessence == (Object)null)
		{
			return null;
		}
		Quintessence quintessence2 = Object.Instantiate<Quintessence>(quintessence, position, Quaternion.identity);
		((Object)quintessence2).name = ((Object)quintessence).name;
		((Component)quintessence2).transform.parent = ((Component)Map.Instance).transform;
		quintessence2.Initialize();
		return quintessence2;
	}

	public Gear DropGear(GearRequest gearRequest, Vector3 position)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Gear gear = DropGear(gearRequest.asset, position);
		ReleaseAddressableHandleOnDestroy.Reserve(((Component)gear).gameObject, gearRequest.handle);
		gearRequest.SetReleaseReserved();
		return gear;
	}

	public Gear DropGear(Gear gear, Vector3 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Gear gear2 = Object.Instantiate<Gear>(gear, position, Quaternion.identity);
		((Object)gear2).name = ((Object)gear).name;
		((Component)gear2).transform.parent = ((Component)Map.Instance).transform;
		gear2.Initialize();
		return gear2;
	}

	public Potion DropPotion(Potion potion)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return DropPotion(potion, ((Component)player).transform.position);
	}

	public Potion DropPotion(Potion potion, Vector3 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Potion potion2 = Object.Instantiate<Potion>(potion, position, Quaternion.identity);
		((Object)potion2).name = ((Object)potion).name;
		((Component)potion2).transform.parent = ((Component)Map.Instance).transform;
		potion2.Initialize();
		return potion2;
	}

	private void ChangeChapter(int chapterIndex)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (_currentChapterIndex != chapterIndex)
		{
			if ((Object)(object)currentChapter != (Object)null)
			{
				currentChapter.Release();
				Resources.UnloadAsset((Object)(object)currentChapter);
			}
			if (_currentChapterHandle.IsValid())
			{
				Addressables.Release<Chapter>(_currentChapterHandle);
			}
			_prevChapterIndex = _currentChapterIndex;
			_currentChapterIndex = chapterIndex;
			AssetReference chapter = LevelResource.instance.GetChapter(chapterIndex);
			_currentChapterHandle = chapter.LoadAssetAsync<Chapter>();
			currentChapter = _currentChapterHandle.WaitForCompletion();
			currentChapter.Initialize((Chapter.Type)chapterIndex);
		}
	}

	public void Load(Chapter.Type chapter)
	{
		Load((int)chapter);
	}

	private void Load(int chapterIndex)
	{
		if (chapterIndex == 2)
		{
			GameData.Generic.tutorial.Start();
		}
		if (chapterIndex == 1 || chapterIndex == 2 || chapterIndex == 8)
		{
			GameData.Save.instance.ResetAll();
			GameData.Currency.gold.ResetNonpermaAll();
			GameData.Currency.bone.ResetNonpermaAll();
			GameData.Currency.heartQuartz.ResetNonpermaAll();
			GameData.Currency.darkQuartz.income = 0;
			GameData.HardmodeProgress.ResetNonpermaAll();
			GameData.Progress.ResetNonpermaAll();
			GameData.Buff.ResetAll();
		}
		GameData.HardmodeProgress.hardmode = chapterIndex >= 8;
		ExtensionMethods.Empty(((Component)this).transform);
		ChangeChapter(chapterIndex);
		if ((Object)(object)Scene<GameBase>.instance == (Object)null)
		{
			SceneManager.LoadSceneAsync("gameBase", (LoadSceneMode)0).completed += delegate
			{
				currentChapter.Enter();
			};
		}
		else
		{
			currentChapter.Enter();
		}
		this.onChapterLoaded?.Invoke();
		Save();
	}

	public void LoadGame()
	{
		if (GameData.Save.instance.hasSave)
		{
			LoadFromSave();
			return;
		}
		Chapter.Type chapter = (GameData.Generic.tutorial.isPlayed() ? Chapter.Type.Castle : Chapter.Type.Tutorial);
		Load(chapter);
	}

	public void LoadFromSave()
	{
		GameData.Save save = GameData.Save.instance;
		ChangeChapter(save.chapterIndex);
		if ((Object)(object)Scene<GameBase>.instance == (Object)null)
		{
			SceneManager.LoadSceneAsync("gameBase", (LoadSceneMode)0).completed += delegate
			{
				Load();
			};
		}
		else
		{
			Load();
		}
		IEnumerator CLoad()
		{
			while ((Object)(object)player == (Object)null)
			{
				yield return null;
			}
			player.playerComponents.inventory.LoadFromSave();
			yield return null;
			player.health.SetCurrentHealth(save.health);
			string[] array = save.abilityBuffs.Split(new char[1] { ',' });
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					(string name, int durationMaps, int stack) tuple = AbilityBuff.Parse(text);
					string item = tuple.name;
					int item2 = tuple.durationMaps;
					int item3 = tuple.stack;
					AbilityBuff abilityBuff = _abilityBuffList.Get(item);
					if (!((Object)(object)abilityBuff == (Object)null))
					{
						AbilityBuff abilityBuff2 = Object.Instantiate<AbilityBuff>(abilityBuff);
						((Object)abilityBuff2).name = item;
						abilityBuff2.Attach(player);
						abilityBuff2.remainMaps = item2;
						abilityBuff2.stack = item3;
					}
				}
			}
		}
		void Load()
		{
			currentChapter.Enter(save.stageIndex, save.pathIndex, save.nodeIndex);
			((MonoBehaviour)this).StartCoroutine(CLoad());
		}
	}

	private void Save()
	{
		Chapter.Type currentChapterIndex = (Chapter.Type)_currentChapterIndex;
		if (currentChapterIndex != Chapter.Type.Castle && currentChapterIndex != Chapter.Type.Tutorial && currentChapterIndex != 0 && !((Object)(object)player == (Object)null))
		{
			player.playerComponents.inventory.Save();
			GameData.Save instance = GameData.Save.instance;
			instance.hasSave = true;
			instance.health = (int)player.health.currentHealth;
			instance.chapterIndex = _currentChapterIndex;
			instance.stageIndex = currentChapter.stageIndex;
			instance.pathIndex = currentChapter.currentStage.pathIndex;
			instance.nodeIndex = (int)currentChapter.currentStage.nodeIndex;
			List<AbilityBuff> instancesByInstanceType = player.ability.GetInstancesByInstanceType<AbilityBuff>();
			instance.abilityBuffs = string.Join(",", instancesByInstanceType);
			instance.SaveAll();
			player.playerComponents.savableAbilityManager.SaveAll();
		}
	}

	private void Reset()
	{
		Singleton<EffectPool>.Instance.Clear();
		PoolObject.Clear();
		Scene<GameBase>.instance.poolObjectContainer.DespawnAll();
		Scene<GameBase>.instance.cameraController.shake.Clear();
		Singleton<Service>.Instance.controllerVibation.Stop();
		GameBase instance = Scene<GameBase>.instance;
		instance.gameFadeInOut.Deactivate();
		instance.cameraController.Zoom(1f);
		instance.cameraController.StopTrack();
		instance.uiManager.curseOfLightVignette.Hide();
		DestroyPlayer();
	}

	public void Unload()
	{
		Reset();
		ExtensionMethods.Empty(((Component)this).transform);
		currentChapter.Clear();
	}

	public void ResetGame()
	{
		_prevChapterIndex = _currentChapterIndex;
		Reset();
		if (GameData.Generic.tutorial.isPlayed())
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				Load(Chapter.Type.HardmodeCastle);
			}
			else
			{
				Load(Chapter.Type.Castle);
			}
		}
		else
		{
			Load(Chapter.Type.Tutorial);
		}
	}

	public void ResetGame(Chapter.Type chapter)
	{
		_prevChapterIndex = _currentChapterIndex;
		Reset();
		Load(chapter);
	}

	public void SpawnPlayerIfNotExist()
	{
		GameBase instance = Scene<GameBase>.instance;
		if ((Object)(object)player == (Object)null)
		{
			player = Object.Instantiate<Character>(CommonResource.instance.player, ((Component)instance).transform);
			instance.uiManager.headupDisplay.Initialize(player);
			instance.cameraController.StartTrack(((Component)player).transform);
			instance.minimapCameraController.StartTrack(((Component)player).transform);
		}
		Object.DontDestroyOnLoad((Object)(object)player);
	}

	public void LoadNextStage()
	{
		currentChapter.NextStage();
		Save();
	}

	public void LoadNextMap(NodeIndex nodeIndex = NodeIndex.Node1)
	{
		((MonoBehaviour)this).StartCoroutine(CLoadNextMap(nodeIndex));
	}

	private IEnumerator CLoadNextMap(NodeIndex nodeIndex)
	{
		Chapter chapter = currentChapter;
		_oldMap = chapter.map;
		Singleton<Service>.Instance.gearManager.DestroyDroppedInstaces();
		if (!chapter.Next(nodeIndex))
		{
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			chapter.Clear();
			Load(_currentChapterIndex + 1);
		}
		Save();
	}

	public void InvokeOnActivateMapReward()
	{
		this.onActivateMapReward?.Invoke();
	}

	public void InvokeOnMapChanged()
	{
		this.onMapLoaded?.Invoke();
	}

	public void InvokeOnMapChangedAndFadeIn(Map newMap)
	{
		this.onMapLoadedAndFadedIn?.Invoke();
		this.onMapChangedAndFadedIn?.Invoke(_oldMap, newMap);
	}

	public void EnterOutTrack(Map map, bool playerReset = false)
	{
		Chapter chapter = currentChapter;
		_oldMap = chapter.map;
		if (playerReset)
		{
			DestroyPlayer();
			SpawnPlayerIfNotExist();
		}
		chapter.EnterOutTrack(map);
	}

	public void DestroyPlayer()
	{
		if ((Object)(object)player != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)player).gameObject);
			player = null;
		}
	}
}
