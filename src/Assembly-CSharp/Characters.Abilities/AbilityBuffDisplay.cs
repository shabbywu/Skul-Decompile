using Level;
using Scenes;
using UI.GearPopup;
using UnityEngine;

namespace Characters.Abilities;

public class AbilityBuffDisplay : DroppedGear
{
	private AbilityBuff _dish;

	public void Initialize(AbilityBuff dish)
	{
		_dish = dish;
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)this).transform.position;
		Vector3 position2 = ((Component)character).transform.position;
		position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (0f - InteractiveObject._popupUIOffset.x));
		position.y += InteractiveObject._popupUIOffset.y;
		GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
		gearPopupCanvas.gearPopup.Set(_dish.displayName, _dish.description);
		gearPopupCanvas.gearPopup.SetInteractionLabel(this);
		gearPopupCanvas.Open(position);
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
