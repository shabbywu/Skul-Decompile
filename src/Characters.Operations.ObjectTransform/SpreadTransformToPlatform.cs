using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class SpreadTransformToPlatform : CharacterOperation
{
	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private float _belowRayDistance = 100f;

	[SerializeField]
	private bool _lastStandingCollider;

	public override void Run(Character owner)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		Collider2D collider;
		if (_lastStandingCollider)
		{
			collider = owner.movement.controller.collisionState.lastStandingCollider;
		}
		else
		{
			owner.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask, _belowRayDistance);
		}
		if (!((Object)(object)collider == (Object)null))
		{
			Bounds bounds = collider.bounds;
			float x = ((Bounds)(ref bounds)).size.x;
			Transform transform = ((Component)_target).transform;
			bounds = collider.bounds;
			float x2 = ((Bounds)(ref bounds)).center.x;
			bounds = collider.bounds;
			transform.position = Vector2.op_Implicit(new Vector2(x2, ((Bounds)(ref bounds)).max.y));
			((Component)_target).transform.localScale = Vector2.op_Implicit(new Vector2(x, ((Component)_target).transform.localScale.y));
		}
	}
}
