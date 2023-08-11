using System;
using Characters;
using UnityEngine;

namespace Utils;

[Serializable]
public class PositionInfo
{
	public enum Pivot
	{
		Center,
		TopLeft,
		Top,
		TopRight,
		Left,
		Right,
		BottomLeft,
		Bottom,
		BottomRight,
		Custom
	}

	private static readonly EnumArray<Pivot, Vector2> _pivotValues = new EnumArray<Pivot, Vector2>((Vector2[])(object)new Vector2[10]
	{
		new Vector2(0f, 0f),
		new Vector2(-0.5f, 0.5f),
		new Vector2(0f, 0.5f),
		new Vector2(0.5f, 0.5f),
		new Vector2(-0.5f, 0f),
		new Vector2(0f, 0.5f),
		new Vector2(-0.5f, -0.5f),
		new Vector2(0f, -0.5f),
		new Vector2(0.5f, -0.5f),
		new Vector2(0f, 0f)
	});

	[SerializeField]
	private Pivot _pivot;

	[SerializeField]
	[HideInInspector]
	private Vector2 _pivotValue;

	public Pivot pivot => _pivot;

	public Vector2 pivotValue => _pivotValue;

	public PositionInfo()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		_pivot = Pivot.Center;
		_pivotValue = Vector2.zero;
	}

	public PositionInfo(bool attach, bool layerOnly, int layerOrderOffset, Pivot pivot)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_pivot = pivot;
		_pivotValue = _pivotValues[pivot];
	}

	public void Attach(ITarget target, Transform obj)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)obj == (Object)null))
		{
			Bounds bounds = target.collider.bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			bounds = target.collider.bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			size.x *= pivotValue.x;
			size.y *= pivotValue.y;
			Vector3 position = center + size;
			obj.position = position;
		}
	}

	public void Attach(Character target, Transform obj)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)obj == (Object)null))
		{
			Bounds bounds = ((Collider2D)target.collider).bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			bounds = ((Collider2D)target.collider).bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			size.x *= pivotValue.x;
			size.y *= pivotValue.y;
			Vector3 position = center + size;
			obj.position = position;
		}
	}

	public Vector2 GetPosition(Character target)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)target.collider).bounds;
		Vector3 center = ((Bounds)(ref bounds)).center;
		bounds = ((Collider2D)target.collider).bounds;
		Vector3 size = ((Bounds)(ref bounds)).size;
		size.x *= pivotValue.x;
		size.y *= pivotValue.y;
		return Vector2.op_Implicit(center + size);
	}
}
