using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI.GearPopup;
using UnityEngine;

namespace Level;

public sealed class CurrencyBag : InteractiveObject, IPurchasable
{
	private const string _prefix = "currencyBag";

	[SerializeField]
	private DropMovement _dropMovement;

	[SerializeField]
	private DroppedEffect _droppedEffect;

	[SerializeField]
	private Rarity _rarity;

	[SerializeField]
	private GameData.Currency.Type _type;

	[SerializeField]
	private int _amount;

	[SerializeField]
	private int _count;

	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private string _overrideNameKey;

	private string _keyBase
	{
		get
		{
			if (!string.IsNullOrEmpty(_overrideNameKey))
			{
				return "currencyBag/" + _overrideNameKey;
			}
			return "currencyBag/" + ((Object)this).name;
		}
	}

	public string displayName => Localization.GetLocalizedString(_keyBase + "/name");

	public string description => Localization.GetLocalizedString(_keyBase + "/desc");

	public int amount
	{
		get
		{
			return _amount;
		}
		set
		{
			_amount = value;
		}
	}

	public int count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
		}
	}

	public Rarity rarity => _rarity;

	public int price { get; set; }

	public GameData.Currency.Type priceCurrency { get; set; }

	public bool released { get; set; }

	public DropMovement dropMovement => _dropMovement;

	protected override void Awake()
	{
		base.Awake();
		_dropMovement.onGround += Activate;
	}

	public override void OnActivate()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		base.OnActivate();
		if ((int)rarity == 3 && (Object)(object)_droppedEffect != (Object)null)
		{
			_droppedEffect.SpawnLegendaryEffect();
		}
	}

	public override void OpenPopupBy(Character character)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)this).transform.position;
		Vector3 position2 = ((Component)character).transform.position;
		position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (0f - InteractiveObject._popupUIOffset.x));
		position.y += InteractiveObject._popupUIOffset.y;
		GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
		gearPopupCanvas.gearPopup.Set(displayName, description);
		string localizedString = Localization.GetLocalizedString("label/interaction/loot");
		string colorCode = GameData.Currency.currencies[_type].colorCode;
		gearPopupCanvas.gearPopup.SetInteractionLabel($"{localizedString} (<color=#{colorCode}>{_amount}</color>)");
		gearPopupCanvas.Open(position);
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
	}

	public override void InteractWith(Character character)
	{
		base.InteractWith(character);
		released = true;
		ClosePopup();
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	private void OnDestroy()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (released)
		{
			Singleton<Service>.Instance.levelManager.DropCurrency(_type, _amount, _count, ((Object)(object)_dropPoint != (Object)null) ? _dropPoint.position : ((Component)this).transform.position);
		}
	}
}
