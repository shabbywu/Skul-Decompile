using Characters.Player;
using UnityEngine;

namespace FX;

public class PlayerShadowCaster : MonoBehaviour
{
	private WeaponInventory _weaponInventory;

	private FootShadowRenderer _shadowRenderer;

	private Collider2D _collider;

	private float _customWidth;

	private void Awake()
	{
		_weaponInventory = ((Component)this).GetComponent<WeaponInventory>();
		_weaponInventory.onSwap += UpdateCustomWidth;
		_collider = ((Component)this).GetComponent<Collider2D>();
		_shadowRenderer = new FootShadowRenderer(1, ((Component)this).transform);
		((Renderer)_shadowRenderer.spriteRenderer).sortingLayerName = "Player";
		((Renderer)_shadowRenderer.spriteRenderer).sortingOrder = -10000;
	}

	private void UpdateCustomWidth()
	{
		_customWidth = _weaponInventory.polymorphOrCurrent.customWidth;
	}

	private void OnDestroy()
	{
		_weaponInventory.onSwap -= UpdateCustomWidth;
	}

	private void LateUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _collider.bounds;
		if (_customWidth > 0f)
		{
			Vector3 size = ((Bounds)(ref bounds)).size;
			size.x = _customWidth;
			((Bounds)(ref bounds)).size = size;
		}
		_shadowRenderer.SetBounds(bounds);
		_shadowRenderer.Update();
	}
}
