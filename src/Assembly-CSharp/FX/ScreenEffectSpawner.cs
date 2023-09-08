using Singletons;
using UnityEngine;

namespace FX;

public class ScreenEffectSpawner : Singleton<ScreenEffectSpawner>
{
	[SerializeField]
	private CameraController _cameraController;

	private float _cachedZoom;

	private void Update()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		float zoom = _cameraController.zoom;
		if (zoom != _cachedZoom)
		{
			_cachedZoom = zoom;
			((Component)this).transform.localScale = Vector3.one * zoom;
		}
	}

	public void Spawn(EffectInfo effect, Vector2 offset)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)this).transform.position;
		position.x += offset.x;
		position.y += offset.y;
		position.z = 0f;
		EffectPoolInstance effectPoolInstance = effect.Spawn(position);
		((Component)effectPoolInstance).transform.parent = ((Component)this).transform;
		Vector3 localScale = Vector3.one * effect.scale.value;
		float value = effect.scaleX.value;
		if (value > 0f)
		{
			localScale.x *= value;
		}
		float value2 = effect.scaleY.value;
		if (value2 > 0f)
		{
			localScale.y *= value2;
		}
		((Component)effectPoolInstance).transform.localScale = localScale;
	}
}
