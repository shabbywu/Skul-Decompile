using UnityEngine;

namespace Characters.AI.Conditions;

public class CompareDistanceFromWall : Condition
{
	private enum Comparer
	{
		GreaterThan,
		LessThan
	}

	[SerializeField]
	private Comparer _compare;

	[SerializeField]
	private float _distanceFromWall;

	protected override bool Check(AIController controller)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = controller.character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return false;
		}
		Bounds bounds = lastStandingCollider.bounds;
		float num = ((((Component)controller.character).transform.position.x > ((Bounds)(ref bounds)).center.x) ? Mathf.Abs(((Bounds)(ref bounds)).max.x - ((Component)controller.character).transform.position.x) : Mathf.Abs(((Bounds)(ref bounds)).min.x - ((Component)controller.character).transform.position.x));
		return _compare switch
		{
			Comparer.GreaterThan => num >= _distanceFromWall, 
			Comparer.LessThan => num <= _distanceFromWall, 
			_ => false, 
		};
	}
}
