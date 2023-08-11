using Characters;
using Characters.Operations.Fx;
using FX.SpriteEffects;
using Scenes;
using Singletons;
using UI.GearPopup;
using UnityEditor;
using UnityEngine;

namespace Level;

public sealed class PurchasablePotion : DroppedPurchasableReward
{
	[SerializeField]
	private int _healAmount;

	[SerializeField]
	private int priority;

	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private Health.HealthGiverType _giverType = Health.HealthGiverType.Potion;

	[SerializeField]
	[Subcomponent(typeof(SpawnEffect))]
	private SpawnEffect _spawn;

	public override void InteractWith(Character character)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		base.InteractWith(character);
		if (base.price == 0)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
			_spawn.Run(character);
			character.spriteEffectStack.Add(new EasedColorBlend(priority, _startColor, _endColor, _curve));
			character.health.Heal(new Health.HealInfo(_giverType, _healAmount));
			ClosePopup();
			Object.Destroy((Object)(object)((Component)this).gameObject);
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
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		base.OpenPopupBy(character);
		Vector3 position = ((Component)this).transform.position;
		Vector3 position2 = ((Component)character).transform.position;
		position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (0f - InteractiveObject._popupUIOffset.x));
		position.y += InteractiveObject._popupUIOffset.y;
		GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
		gearPopupCanvas.gearPopup.Set(displayName, description);
		gearPopupCanvas.gearPopup.SetInteractionLabelAsPurchase(base.priceCurrency, base.price);
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
