using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
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

public sealed class DroppedRandomInscriptionItemBox : DroppedPurchasableReward
{
	private sealed class ExtraSeed : MonoBehaviour
	{
		public short value;
	}

	private const int _randomSeed = 716722307;

	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private SpriteRenderer _inscriptionDisplay;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onDeactivate;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onChangedKey;

	private Inscription.Key _key;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	private List<Inscription.Key> _keywords;

	private Random _random;

	public override string displayName => string.Format(Localization.GetLocalizedString(base._keyBase + "/name"), Inscription.GetName(_key));

	public override string description => string.Format(Localization.GetLocalizedString(base._keyBase + "/desc"), Inscription.GetName(_key));

	public string interaction => Localization.GetLocalizedString("label/interaction/search");

	private void Start()
	{
		ExtraSeed extraSeed = default(ExtraSeed);
		if (!((Component)Map.Instance).TryGetComponent<ExtraSeed>(ref extraSeed))
		{
			extraSeed = ((Component)Map.Instance).gameObject.AddComponent<ExtraSeed>();
		}
		extraSeed.value++;
		_onDeactivate.Initialize();
		_onChangedKey.Initialize();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 716722307 + (int)currentChapter.type * 16 + currentChapter.stageIndex + extraSeed.value);
		_keywords = Inscription.keys.ToList();
		_keywords.Remove(Inscription.Key.None);
		_keywords.Remove(Inscription.Key.SunAndMoon);
		_keywords.Remove(Inscription.Key.Omen);
		_keywords.Remove(Inscription.Key.Sin);
		LoadItemWithKey();
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged += HandleOnItemInstanceChanged;
	}

	private void HandleOnItemInstanceChanged()
	{
		if (!Singleton<Service>.Instance.gearManager.CanDrop(_key))
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			((MonoBehaviour)player).StartCoroutine(_onChangedKey.CRun(player));
			LoadItemWithKey();
		}
	}

	private void LoadItemWithKey()
	{
		Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
		do
		{
			_key = ExtensionMethods.Random<Inscription.Key>((IEnumerable<Inscription.Key>)_keywords, _random);
		}
		while (synergy.inscriptions[_key].isMaxStep);
		LoadItem();
	}

	private void LoadItem()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		do
		{
			Rarity rarity = _rarityPossibilities.Evaluate(_random);
			_itemToDrop = Singleton<Service>.Instance.gearManager.GetItemByKeyword(_random, rarity, _key);
		}
		while (_itemToDrop == null);
		_itemRequest = _itemToDrop.LoadAsync();
		_inscriptionDisplay.sprite = Inscription.GetActiveIcon(_key);
	}

	private void OnDestroy()
	{
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= HandleOnItemInstanceChanged;
		_itemRequest?.Release();
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

	public override void InteractWith(Character character)
	{
		base.InteractWith(character);
		if (base.price == 0)
		{
			((MonoBehaviour)this).StartCoroutine(CDelayedDrop());
			ClosePopup();
			Character player = Singleton<Service>.Instance.levelManager.player;
			((MonoBehaviour)player).StartCoroutine(_onDeactivate.CRun(player));
			Deactivate();
		}
		IEnumerator CDelayedDrop()
		{
			while (!_itemRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropItem(_itemRequest, _dropPoint.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, _dropPoint.position);
		}
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
