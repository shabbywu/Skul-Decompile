using UnityEngine;

namespace FX;

public class GearShadowCaster : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private Collider2D _collider;

	private FootShadowRenderer _shadowRenderer;

	private void Awake()
	{
		_shadowRenderer = new FootShadowRenderer(0, ((Component)this).transform);
		((Renderer)_shadowRenderer.spriteRenderer).sortingLayerID = ((Renderer)_renderer).sortingLayerID;
		((Renderer)_shadowRenderer.spriteRenderer).sortingOrder = ((Renderer)_renderer).sortingOrder - 10000;
	}

	private void LateUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _collider.bounds;
		((Bounds)(ref bounds)).size = Vector2.op_Implicit(new Vector2(0.75f, 0.5f));
		_shadowRenderer.SetBounds(bounds);
		_shadowRenderer.Update();
	}
}
