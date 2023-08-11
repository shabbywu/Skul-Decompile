using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToPlatformPoint : Policy
{
	private enum Point
	{
		Left,
		Center,
		Right
	}

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private LayerMask _platformLayer = Layers.footholdMask;

	[SerializeField]
	private bool _lastStandingCollider = true;

	[SerializeField]
	private bool _colliderInterpolation = true;

	[SerializeField]
	private Point _point;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = owner;
		}
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = _owner.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				_owner.movement.TryGetClosestBelowCollider(out collider, _platformLayer);
			}
		}
		else
		{
			_owner.movement.TryGetClosestBelowCollider(out collider, _platformLayer);
		}
		Bounds bounds = collider.bounds;
		Vector2 result = default(Vector2);
		switch (_point)
		{
		case Point.Left:
			((Vector2)(ref result))._002Ector(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.y);
			if (_colliderInterpolation)
			{
				((Vector2)(ref result))._002Ector(ClampX(_owner, ((Bounds)(ref bounds)).min.x, bounds), ((Bounds)(ref bounds)).max.y);
			}
			return result;
		case Point.Center:
			((Vector2)(ref result))._002Ector(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y);
			if (_colliderInterpolation)
			{
				((Vector2)(ref result))._002Ector(ClampX(_owner, ((Bounds)(ref bounds)).center.x, bounds), ((Bounds)(ref bounds)).max.y);
			}
			return result;
		case Point.Right:
			((Vector2)(ref result))._002Ector(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).max.y);
			if (_colliderInterpolation)
			{
				((Vector2)(ref result))._002Ector(ClampX(_owner, ((Bounds)(ref bounds)).max.x, bounds), ((Bounds)(ref bounds)).max.y);
			}
			return result;
		default:
			return Vector2.op_Implicit(((Component)_owner).transform.position);
		}
	}

	private float ClampX(Character owner, float x, Bounds platform)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (x <= ((Bounds)(ref platform)).min.x + owner.collider.size.x)
		{
			x = ((Bounds)(ref platform)).min.x + owner.collider.size.x;
		}
		else if (x >= ((Bounds)(ref platform)).max.x - owner.collider.size.x)
		{
			x = ((Bounds)(ref platform)).max.x - owner.collider.size.x;
		}
		return x;
	}
}
