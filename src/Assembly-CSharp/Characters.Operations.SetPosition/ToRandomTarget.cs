using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToRandomTarget : Policy
{
	[SerializeField]
	private Collider2D _findRange;

	[SerializeField]
	private Collider2D _ownerCollider;

	[SerializeField]
	private CustomFloat _amount;

	[SerializeField]
	private float _belowRayDistance = 100f;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private bool _behind;

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _lastStandingCollider;

	private Vector2 _default => Vector2.op_Implicit(((Component)this).transform.position);

	public override Vector2 GetPosition(Character owner)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_ownerCollider == (Object)null)
		{
			_ownerCollider = (Collider2D)(object)owner.collider;
		}
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		Character randomTarget = TargetFinder.GetRandomTarget(_findRange, _targetLayer.Evaluate(((Component)_ownerCollider).gameObject));
		if ((Object)(object)randomTarget == (Object)null)
		{
			Debug.Log((object)"Target is null");
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		Vector2 result = Vector2.op_Implicit(((Component)randomTarget).transform.position);
		Clamp(ref result, _amount.value);
		if (!_onPlatform)
		{
			return result;
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = randomTarget.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				randomTarget.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
				if ((Object)(object)collider == (Object)null)
				{
					return _default;
				}
			}
		}
		else
		{
			randomTarget.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
			if ((Object)(object)collider == (Object)null)
			{
				return _default;
			}
		}
		float x = ((Component)randomTarget).transform.position.x;
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
		bounds = _ownerCollider.bounds;
		float min = x + ((Bounds)(ref bounds)).size.x;
		bounds = collider.bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		bounds = _ownerCollider.bounds;
		float max = x2 - ((Bounds)(ref bounds)).size.x;
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
