using System.Collections;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Data;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameResourceLoader
{
	private AsyncOperationHandle<LocalizationStringResource> _localizationStringHandle;

	private AsyncOperationHandle<GearResource> _gearHandle;

	private AsyncOperationHandle<LevelResource> _levelHandle;

	private AsyncOperationHandle<CommonResource> _commonHandle;

	private AsyncOperationHandle<MaterialResource> _materialHandle;

	private AsyncOperationHandle<HUDResource> _hudHandle;

	private WeaponRequest _weaponRequest1;

	private WeaponRequest _weaponRequest2;

	private EssenceRequest _essenceRequest;

	private ItemRequest[] _itemRequests;

	public static GameResourceLoader instance { get; private set; }

	public static void Load()
	{
		instance = new GameResourceLoader();
		instance.LoadInternal();
	}

	private void LoadInternal()
	{
		PreloadStrings();
	}

	public void WaitForStrings()
	{
		_localizationStringHandle.WaitForCompletion();
	}

	public void WaitForCompletion()
	{
		PreloadStrings();
		PreloadLevel();
		PreloadGear();
		PreloadCommon();
		PreloadMaterial();
		PreloadHUD();
		_localizationStringHandle.WaitForCompletion();
		_gearHandle.WaitForCompletion();
		_levelHandle.WaitForCompletion();
		_commonHandle.WaitForCompletion();
		_materialHandle.WaitForCompletion();
		_hudHandle.WaitForCompletion();
		Singleton<Service>.Instance.gearManager.Initialize();
	}

	public void Clear()
	{
		instance = null;
	}

	private void PreloadStrings()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!_localizationStringHandle.IsValid())
		{
			_localizationStringHandle = Addressables.LoadAssetAsync<LocalizationStringResource>((object)"LocalizationStringResource");
			_localizationStringHandle.Completed += OnStringLoaded;
		}
	}

	private void OnStringLoaded(AsyncOperationHandle<LocalizationStringResource> handle)
	{
		handle.Result.Initialize();
		PreloadLevel();
		PreloadGear();
		PreloadCommon();
		PreloadMaterial();
		PreloadHUD();
	}

	private void PreloadLevel()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!_levelHandle.IsValid())
		{
			_levelHandle = Addressables.LoadAssetAsync<LevelResource>((object)"LevelResource");
			_levelHandle.Completed += OnLevelLoaded;
		}
	}

	private void OnLevelLoaded(AsyncOperationHandle<LevelResource> handle)
	{
		handle.Result.Initialize();
	}

	private void PreloadGear()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!_gearHandle.IsValid())
		{
			_gearHandle = Addressables.LoadAssetAsync<GearResource>((object)"GearResource");
			_gearHandle.Completed += OnGearLoaded;
		}
	}

	private void OnGearLoaded(AsyncOperationHandle<GearResource> handle)
	{
		handle.Result.Initialize();
		PreloadSavedGear();
	}

	private void PreloadCommon()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!_commonHandle.IsValid())
		{
			_commonHandle = Addressables.LoadAssetAsync<CommonResource>((object)"CommonResource");
			_commonHandle.Completed += OnResourceLoaded;
		}
	}

	private void OnResourceLoaded(AsyncOperationHandle<CommonResource> handle)
	{
		handle.Result.Initialize();
	}

	private void PreloadMaterial()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!_materialHandle.IsValid())
		{
			_materialHandle = Addressables.LoadAssetAsync<MaterialResource>((object)"MaterialResource");
			_materialHandle.Completed += OnMaterialsLoaded;
		}
	}

	private void OnMaterialsLoaded(AsyncOperationHandle<MaterialResource> handle)
	{
		handle.Result.Initialize();
	}

	private void PreloadHUD()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!_hudHandle.IsValid())
		{
			_hudHandle = Addressables.LoadAssetAsync<HUDResource>((object)"HUDResource");
			_hudHandle.Completed += OnHUDResourceLoaded;
		}
	}

	private void OnHUDResourceLoaded(AsyncOperationHandle<HUDResource> handle)
	{
		handle.Result.Initialize();
	}

	public void PreloadSavedGear()
	{
		((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CPreloadSavedGear());
	}

	private IEnumerator CPreloadSavedGear()
	{
		GameData.Save save = GameData.Save.instance;
		while (!save.initilaized)
		{
			yield return null;
		}
		if (!save.hasSave)
		{
			yield break;
		}
		_gearHandle.WaitForCompletion();
		_itemRequests = new ItemRequest[save.items.length];
		GearResource gearResource = GearResource.instance;
		if (gearResource.TryGetWeaponReferenceByName(save.nextWeapon, out var reference))
		{
			_weaponRequest2 = reference.LoadAsync();
		}
		if (gearResource.TryGetWeaponReferenceByName(save.currentWeapon, out var reference2))
		{
			_weaponRequest1 = reference2.LoadAsync();
		}
		if (gearResource.TryGetEssenceReferenceByName(save.essence, out var reference3))
		{
			_essenceRequest = reference3.LoadAsync();
		}
		for (int i = 0; i < save.items.length; i++)
		{
			if (gearResource.TryGetItemReferenceByName(save.items[i], out var reference4))
			{
				_itemRequests[i] = reference4.LoadAsync();
			}
		}
	}

	public Weapon TakeWeapon1()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_weaponRequest1.WaitForCompletion();
		Weapon result = Singleton<Service>.Instance.levelManager.DropWeapon(_weaponRequest1, Vector3.zero);
		_weaponRequest1 = null;
		return result;
	}

	public Weapon TakeWeapon2()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (_weaponRequest2 == null)
		{
			return null;
		}
		_weaponRequest2.WaitForCompletion();
		Weapon result = Singleton<Service>.Instance.levelManager.DropWeapon(_weaponRequest2, Vector3.zero);
		_weaponRequest2 = null;
		return result;
	}

	public Quintessence TakeEssence()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (_essenceRequest == null)
		{
			return null;
		}
		_essenceRequest.WaitForCompletion();
		Quintessence result = Singleton<Service>.Instance.levelManager.DropQuintessence(_essenceRequest, Vector3.zero);
		_essenceRequest = null;
		return result;
	}

	public Item TakeItem(int index)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		ref ItemRequest reference = ref _itemRequests[index];
		if (reference == null)
		{
			return null;
		}
		reference.WaitForCompletion();
		Item result = Singleton<Service>.Instance.levelManager.DropItem(reference, Vector3.zero);
		reference = null;
		return result;
	}
}
