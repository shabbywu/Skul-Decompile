using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public class FlipInDistanceFromPlatform : Target
{
	[SerializeField]
	private CustomFloat _distance;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return character.lookingDirection;
		}
		Vector3 position = ((Component)character).transform.position;
		float num = position.x + _distance.value;
		Bounds bounds = lastStandingCollider.bounds;
		if (num > ((Bounds)(ref bounds)).max.x)
		{
			return Character.LookingDirection.Right;
		}
		float num2 = position.x - _distance.value;
		bounds = lastStandingCollider.bounds;
		if (num2 < ((Bounds)(ref bounds)).min.x)
		{
			return Character.LookingDirection.Left;
		}
		return character.lookingDirection;
	}
}
