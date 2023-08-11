using System;
using System.Collections;
using Characters;
using Characters.Operations;
using Characters.Operations.Fx;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI.GearPopup;
using UnityEditor;
using UnityEngine;

namespace Level;

public sealed class DroppedRandomItem : DroppedPurchasableReward
{
	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private int priority;

	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _curve;

	[SerializeField]
	[Subcomponent(typeof(SpawnEffect))]
	private SpawnEffect _spawn;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onDeactivate;

	private const int _randomSeed = 2028506624;

	private Random _random;

	private Rarity _rarity;

	private Rarity _gearRarity;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	public string interaction => Localization.GetLocalizedString("label/interaction/search");

	protected override void Awake()
	{
		base.Awake();
		_onDeactivate.Initialize();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	private void Start()
	{
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged += Load;
		Load();
	}

	private void OnDestroy()
	{
		_itemRequest?.Release();
	}

	private void EvaluateGearRarity()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		_gearRarity = Settings.instance.containerPossibilities[_rarity].Evaluate(_random);
	}

	private void Load()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		_itemRequest?.Release();
		do
		{
			_rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(_random);
			EvaluateGearRarity();
			_itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _gearRarity);
		}
		while (_itemToDrop == null);
		_itemRequest = _itemToDrop.LoadAsync();
	}

	public override void InteractWith(Character character)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		base.InteractWith(character);
		if (base.price == 0)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, _dropPoint.position);
			_spawn.Run(character);
			((MonoBehaviour)this).StartCoroutine(CDrop());
			ClosePopup();
			Deactivate();
		}
	}

	private IEnumerator CDrop()
	{
		while (!_itemRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= Load;
		Singleton<Service>.Instance.levelManager.DropItem(_itemRequest, _dropPoint.position);
		Object.Destroy((Object)(object)((Component)this).gameObject);
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

	public void Initialize()
	{
	}
}
