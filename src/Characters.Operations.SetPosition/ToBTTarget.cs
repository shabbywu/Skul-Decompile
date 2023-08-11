using BT;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBTTarget : Policy
{
	[SerializeField]
	private BehaviourTreeRunner _bt;

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _lastStandingCollider;

	private Vector2 _default => Vector2.op_Implicit(((Component)this).transform.position);

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		Character character = _bt.context.Get<Character>(BT.Key.Target);
		if ((Object)(object)character == (Object)null)
		{
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		if (!_onPlatform)
		{
			return Vector2.op_Implicit(((Component)character).transform.position);
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = character.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)collider == (Object)null)
			{
				character.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
				if ((Object)(object)collider == (Object)null)
				{
					return _default;
				}
			}
		}
		else
		{
			character.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
			if ((Object)(object)collider == (Object)null)
			{
				return _default;
			}
		}
		float x = ((Component)character).transform.position.x;
		Bounds bounds = collider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		return new Vector2(x, y);
	}
}
