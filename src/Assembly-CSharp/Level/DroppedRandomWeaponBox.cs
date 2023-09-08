using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Operations;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI.GearPopup;
using UnityEditor;
using UnityEngine;

namespace Level;

public sealed class DroppedRandomWeaponBox : DroppedPurchasableReward
{
	private const int _randomSeed = 716722307;

	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private Weapon.Category _category;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onDeactivate;

	private WeaponReference _weaponToDrop;

	private WeaponRequest _weaponRequest;

	private Random _random;

	private bool _spawned;

	public string interaction => Localization.GetLocalizedString("label/interaction/search");

	private void Start()
	{
		_onDeactivate.Initialize();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random((int)(GameData.Save.instance.randomSeed + 716722307 + (int)currentChapter.type * 16 + currentChapter.stageIndex + _category));
	}

	private void Load()
	{
		do
		{
			Rarity rarity = _rarityPossibilities.Evaluate(_random);
			_weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponByCategory(_random, rarity, _category);
		}
		while (_weaponToDrop == null);
		_weaponRequest = _weaponToDrop.LoadAsync();
	}

	private void OnDestroy()
	{
		_weaponRequest?.Release();
	}

	public override void OpenPopupBy(Character character)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		base.OpenPopupBy(character);
		Vector3 position = ((Component)this).transform.position;
		Vector3 position2 = ((Component)character).transform.position;
		position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (0f - InteractiveObject._popupUIOffset.x));
		position.y += InteractiveObject._popupUIOffset.y;
		GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
		gearPopupCanvas.gearPopup.Set(displayName, description);
		gearPopupCanvas.gearPopup.SetInteractionLabelAsPurchase(interaction, base.priceCurrency, base.price);
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Open(position);
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
	}

	public override void InteractWith(Character character)
	{
		base.InteractWith(character);
		if (base.price == 0)
		{
			((MonoBehaviour)character).StartCoroutine(CDelayedDrop(character));
			ClosePopup();
			Deactivate();
		}
	}

	private IEnumerator CDelayedDrop(Character character)
	{
		Load();
		while (!_weaponRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.levelManager.DropWeapon(_weaponRequest, _dropPoint.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, _dropPoint.position);
		_spawned = true;
		((MonoBehaviour)this).StartCoroutine(_onDeactivate.CRun(character));
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		((MonoBehaviour)Map.Instance).StartCoroutine(CDestroy());
	}

	private IEnumerator CDestroy()
	{
		while (!_spawned)
		{
			yield return null;
		}
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
