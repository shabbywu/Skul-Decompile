using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToPlayer : Policy
{
	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private bool _lastStandingCollider;

	[SerializeField]
	private float _belowRayLength = 100f;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		if (!_onPlatform)
		{
			return Vector2.op_Implicit(((Component)player).transform.position);
		}
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = player.movement.controller.collisionState.lastStandingCollider;
		}
		else if (!player.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayLength))
		{
			return Vector2.op_Implicit(((Component)player).transform.position);
		}
		float x = ((Component)player).transform.position.x;
		Bounds bounds = collider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		return new Vector2(x, y);
	}
}
