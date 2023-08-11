using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public sealed class ClosestSideFromPlayer : Target
{
	[SerializeField]
	private bool _farthest;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		if ((Object)(object)player == (Object)null)
		{
			return character.lookingDirection;
		}
		Collider2D collider = player.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)collider == (Object)null && !player.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask))
		{
			return character.lookingDirection;
		}
		float x = ((Component)player).transform.position.x;
		Bounds bounds = collider.bounds;
		if (x > ((Bounds)(ref bounds)).center.x)
		{
			if (!_farthest)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}
		if (!_farthest)
		{
			return Character.LookingDirection.Left;
		}
		return Character.LookingDirection.Right;
	}
}
