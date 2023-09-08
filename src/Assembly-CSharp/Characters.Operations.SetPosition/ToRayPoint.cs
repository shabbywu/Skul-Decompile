using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public sealed class ToRayPoint : Policy
{
	[SerializeField]
	private Transform _origin;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private LayerMask _targetLayerMask;

	[SerializeField]
	private float _distance;

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _onPlatformWhenHitTerrain;

	private NonAllocCaster _nonAllocCaster;

	private NonAllocCaster _belowCaster;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	private void Awake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		_nonAllocCaster = new NonAllocCaster(1);
		((ContactFilter2D)(ref _nonAllocCaster.contactFilter)).SetLayerMask(_targetLayerMask);
		if (_onPlatform || _onPlatformWhenHitTerrain)
		{
			_belowCaster = new NonAllocCaster(1);
		}
	}

	public override Vector2 GetPosition()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.op_Implicit(((Component)_target).transform.position - ((Component)_origin).transform.position);
		float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
		if (num < 0f)
		{
			num += 360f;
		}
		if (((Component)_target).transform.position.x > ((Component)_origin).transform.position.x)
		{
			if (num > 90f && num < 270f)
			{
				val = Vector2.down;
			}
			else if (num > 0f && num < 90f)
			{
				val = Vector2.right;
			}
		}
		else if (num < 90f || num > 270f)
		{
			val = Vector2.down;
		}
		else if (num > 90f && num < 180f)
		{
			val = Vector2.left;
		}
		_nonAllocCaster.RayCast(Vector2.op_Implicit(((Component)_origin).transform.position), val, _distance);
		if (_nonAllocCaster.results.Count == 0)
		{
			return Vector2.op_Implicit(((Component)_target).transform.position);
		}
		RaycastHit2D val2 = _nonAllocCaster.results[0];
		Vector2 point = ((RaycastHit2D)(ref val2)).point;
		if (_onPlatform)
		{
			return PlatformUtils.GetProjectionPointToPlatform(point, Vector2.down, _belowCaster, LayerMask.op_Implicit(262144));
		}
		if (_onPlatformWhenHitTerrain)
		{
			val2 = _nonAllocCaster.results[0];
			if (((Component)((RaycastHit2D)(ref val2)).collider).gameObject.layer == 8)
			{
				return PlatformUtils.GetProjectionPointToPlatform(point, Vector2.down, _belowCaster, LayerMask.op_Implicit(262144));
			}
		}
		else if (point.y > _origin.position.y)
		{
			((Vector2)(ref point))._002Ector(point.x, _origin.position.y);
		}
		return point;
	}
}
