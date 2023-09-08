using UnityEngine;

namespace FX;

public class EnemyShadowCaster : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	[Information("0이면 콜라이더 크기 따라감", InformationAttribute.InformationType.Info, false)]
	private float _customWidth;

	private FootShadowRenderer _shadowRenderer;

	private void Awake()
	{
		if ((Object)(object)_collider == (Object)null)
		{
			_collider = ((Component)this).GetComponent<Collider2D>();
		}
		_shadowRenderer = new FootShadowRenderer(0, ((Component)this).transform);
		((Renderer)_shadowRenderer.spriteRenderer).sortingLayerID = ((Renderer)_renderer).sortingLayerID;
		((Renderer)_shadowRenderer.spriteRenderer).sortingOrder = ((Renderer)_renderer).sortingOrder - 10000;
	}

	private void LateUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3 position = ((Component)_shadowRenderer.spriteRenderer).transform.position;
		position.x = ((Bounds)(ref bounds)).center.x;
		((Component)_shadowRenderer.spriteRenderer).transform.position = position;
	}
}
