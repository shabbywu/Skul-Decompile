using System;
using Scenes;
using UI;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
	private enum HorizontalAlign
	{
		Center,
		Left,
		Right
	}

	private enum VerticalAlign
	{
		Center,
		Bottom,
		Top
	}

	[GetComponent]
	[SerializeField]
	private BoxCollider2D _zone;

	[SerializeField]
	private HorizontalAlign _horizontalAlign;

	[SerializeField]
	private VerticalAlign _verticalAlign = VerticalAlign.Bottom;

	[SerializeField]
	private bool _hasCeil;

	[NonSerialized]
	public Bounds bounds;

	public bool hasCeil
	{
		get
		{
			return _hasCeil;
		}
		set
		{
			_hasCeil = value;
		}
	}

	private void Awake()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_zone != (Object)null)
		{
			bounds = ((Collider2D)_zone).bounds;
			Object.Destroy((Object)(object)_zone);
		}
	}

	public Vector3 GetClampedPosition(Camera camera, Vector3 position)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		float orthographicSize = camera.orthographicSize;
		UIManager uiManager = Scene<GameBase>.instance.uiManager;
		Vector2 sizeDelta = uiManager.rectTransform.sizeDelta;
		Vector2 sizeDelta2 = uiManager.content.sizeDelta;
		float num = sizeDelta2.x / sizeDelta2.y;
		float num2 = orthographicSize * 2f * sizeDelta2.y / sizeDelta.y;
		float num3 = num2 * num;
		Vector3 max = ((Bounds)(ref bounds)).max;
		max.x -= num3 * 0.5f;
		if (_hasCeil)
		{
			max.y -= num2 * 0.5f;
		}
		else
		{
			max.y = float.PositiveInfinity;
		}
		Vector3 min = ((Bounds)(ref bounds)).min;
		min.x += num3 * 0.5f;
		min.y += num2 * 0.5f;
		float z = position.z;
		position = Vector3.Max(Vector3.Min(position, max), min);
		position.z = z;
		if (((Bounds)(ref bounds)).size.x < num3)
		{
			switch (_horizontalAlign)
			{
			case HorizontalAlign.Center:
				position.x = ((Bounds)(ref bounds)).center.x;
				break;
			case HorizontalAlign.Left:
				position.x = min.x;
				break;
			case HorizontalAlign.Right:
				position.x = max.x;
				break;
			}
		}
		if (((Bounds)(ref bounds)).size.y < num2)
		{
			switch (_verticalAlign)
			{
			case VerticalAlign.Center:
				position.y = ((Bounds)(ref bounds)).center.y;
				break;
			case VerticalAlign.Bottom:
				position.y = min.y;
				break;
			case VerticalAlign.Top:
				position.y = max.y;
				break;
			}
		}
		return position;
	}

	public void ClampPosition(Camera camera)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		((Component)camera).transform.position = GetClampedPosition(camera, ((Component)camera).transform.position);
	}
}
