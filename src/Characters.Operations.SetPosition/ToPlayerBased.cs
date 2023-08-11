using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToPlayerBased : Policy
{
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

	[SerializeField]
	private LayerMask _platformLayerMask = Layers.footholdMask;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_collider == (Object)null)
		{
			_collider = (Collider2D)(object)owner.collider;
		}
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Vector2 result = Vector2.op_Implicit(((Component)player).transform.position);
		Clamp(ref result, _amount.value);
		if (!_onPlatform)
		{
			return result;
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = player.movement.controller.collisionState.lastStandingCollider;
		}
		else
		{
			player.movement.TryGetClosestBelowCollider(out collider, _platformLayerMask, _belowRayDistance);
		}
		float x = result.x;
		Bounds bounds = collider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		return new Vector2(x, y);
	}

	private void Clamp(ref Vector2 result, float amount)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = player.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				player.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayDistance);
			}
		}
		else
		{
			player.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayDistance);
		}
		Bounds bounds = collider.bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		bounds = _collider.bounds;
		float min = x + ((Bounds)(ref bounds)).extents.x;
		bounds = collider.bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		bounds = _collider.bounds;
		float max = x2 - ((Bounds)(ref bounds)).extents.x;
		if (player.lookingDirection == Character.LookingDirection.Right)
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
