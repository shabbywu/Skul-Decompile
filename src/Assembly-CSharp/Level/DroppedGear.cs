using System;
using System.Linq;
using Characters;
using Characters.Gear;
using Characters.Gear.Items;
using Data;
using Scenes;
using Singletons;
using UnityEngine;

namespace Level;

public class DroppedGear : InteractiveObject
{
	[SerializeField]
	private DropMovement _dropMovement;

	[SerializeField]
	private DroppedEffect _droppedEffect;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[NonSerialized]
	public GameData.Currency.Type priceCurrency;

	[NonSerialized]
	public int price;

	[NonSerialized]
	public float additionalPopupUIOffsetY;

	private Vector3 _initialPosition;

	public override CharacterInteraction.InteractionType interactionType
	{
		get
		{
			if (!((Object)(object)gear == (Object)null) && gear.destructible)
			{
				return CharacterInteraction.InteractionType.Pressing;
			}
			return CharacterInteraction.InteractionType.Normal;
		}
	}

	public Gear gear { get; private set; }

	public SpriteRenderer spriteRenderer => _spriteRenderer;

	public DropMovement dropMovement => _dropMovement;

	public event Action<Character> onLoot;

	public event Action<Character> onDestroy;

	protected override void Awake()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		base.Awake();
		if ((Object)(object)_dropMovement == (Object)null)
		{
			Activate();
		}
		else
		{
			_dropMovement.onGround += OnGround;
		}
		_initialPosition = ((Component)this).transform.localPosition;
	}

	public void Initialize(Gear gear)
	{
		this.gear = gear;
	}

	private void OnGround()
	{
		Activate();
		if ((Object)(object)gear != (Object)null && (Object)(object)_droppedEffect != (Object)null)
		{
			if (gear.rarity == Rarity.Legendary)
			{
				_droppedEffect.SpawnLegendaryEffect();
			}
			else if (gear.gearTag.HasFlag(Gear.Tag.Omen))
			{
				_droppedEffect.SpawnOmenEffect();
			}
		}
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		base.OpenPopupBy(character);
		Vector3 position = ((Component)this).transform.position;
		Vector3 position2 = ((Component)character).transform.position;
		position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (0f - InteractiveObject._popupUIOffset.x));
		position.y += InteractiveObject._popupUIOffset.y + additionalPopupUIOffsetY;
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.gearPopup.Set(gear);
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Open(position);
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
	}

	public override void InteractWith(Character character)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)gear != (Object)null && !gear.lootable)
		{
			return;
		}
		GameData.Currency currency = GameData.Currency.currencies[priceCurrency];
		if (!currency.Has(price))
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactFailSound, ((Component)this).transform.position);
			return;
		}
		if (gear is Item fieldItem && !character.playerComponents.inventory.item.items.Any((Item i) => (Object)(object)i == (Object)null))
		{
			Scene<GameBase>.instance.uiManager.itemSelect.Open(fieldItem);
			return;
		}
		if (!currency.Consume(price))
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactFailSound, ((Component)this).transform.position);
			return;
		}
		ClosePopup();
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		price = 0;
		((Component)this).transform.localPosition = _initialPosition;
		if ((Object)(object)gear != (Object)null && gear.rarity == Rarity.Legendary && (Object)(object)_droppedEffect != (Object)null)
		{
			_droppedEffect.Despawn();
		}
		this.onLoot?.Invoke(character);
	}

	public override void InteractWithByPressing(Character character)
	{
		ClosePopup();
		if ((Object)(object)gear != (Object)null && gear.rarity == Rarity.Legendary && (Object)(object)_droppedEffect != (Object)null)
		{
			_droppedEffect.Despawn();
		}
		this.onDestroy?.Invoke(character);
		Object.Destroy((Object)(object)((Component)gear).gameObject);
	}
}
