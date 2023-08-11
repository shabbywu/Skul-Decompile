using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI.GearPopup;
using UnityEngine;

namespace Level;

public sealed class PurchasableCurrency : DroppedPurchasableReward
{
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

	private bool _released;

	public override void OpenPopupBy(Character character)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)this).transform.position;
		Vector3 position2 = ((Component)character).transform.position;
		position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (0f - InteractiveObject._popupUIOffset.x));
		position.y += InteractiveObject._popupUIOffset.y;
		GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
		gearPopupCanvas.gearPopup.Set(displayName, description);
		Localization.GetLocalizedString("label/interaction/loot");
		_ = GameData.Currency.currencies[_type].colorCode;
		gearPopupCanvas.gearPopup.SetInteractionLabelAsPurchase(base.priceCurrency, base.price);
		gearPopupCanvas.Open(position);
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
	}

	public override void InteractWith(Character character)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		base.InteractWith(character);
		if (base.price == 0)
		{
			Singleton<Service>.Instance.levelManager.DropCurrency(_type, _amount, _count, ((Object)(object)_dropPoint != (Object)null) ? _dropPoint.position : ((Component)this).transform.position);
			_released = true;
			ClosePopup();
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}

	private void OnDestroy()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (!_released && Map.Instance.type == Map.Type.Normal)
		{
			Singleton<Service>.Instance.levelManager.DropCurrency(_type, _amount, _count, ((Object)(object)_dropPoint != (Object)null) ? _dropPoint.position : ((Component)this).transform.position);
		}
	}
}
