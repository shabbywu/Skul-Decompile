using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToCharacterBased : Policy
{
	[SerializeField]
	private Character _target;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private CustomFloat _amount;

	[SerializeField]
	private float _belowRayDistance = 100f;

	[SerializeField]
	private bool _behind;

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _lastStandingCollider;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_target == (Object)null)
		{
			_target = owner;
		}
		if ((Object)(object)_collider == (Object)null)
		{
			_collider = (Collider2D)(object)owner.collider;
		}
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Vector2.op_Implicit(((Component)_target).transform.position);
		Clamp(ref result, _amount.value);
		if (!_onPlatform)
		{
			return result;
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = _target.movement.controller.collisionState.lastStandingCollider;
		}
		else
		{
			_target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayDistance);
		}
		float x = result.x;
		Bounds bounds = collider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		return new Vector2(x, y);
	}

	private void Clamp(ref Vector2 result, float amount)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = _target.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				_target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayDistance);
			}
		}
		else
		{
			_target.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayDistance);
		}
		Bounds bounds = collider.bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		bounds = _collider.bounds;
		float min = x + ((Bounds)(ref bounds)).extents.x;
		bounds = collider.bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		bounds = _collider.bounds;
		float max = x2 - ((Bounds)(ref bounds)).extents.x;
		if (_target.lookingDirection == Character.LookingDirection.Right)
		{
			result = ClampX(result, _behind ? (result.x - amount) : (result.x + amount), min, max);
		}
		else
		{
			result = ClampX(result, _behind ? (result.x + amount) : (result.x - amount), min, max);
		}
	}

	private Vector2 ClampX(Vector2 result, float x, float min, float max)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.05f;
		if (x <= min)
		{
			((Vector2)(ref result))._002Ector(min + num, result.y);
		}
		else if (x >= max)
		{
			((Vector2)(ref result))._002Ector(max - num, result.y);
		}
		else
		{
			((Vector2)(ref result))._002Ector(x, result.y);
		}
		return result;
	}
}
