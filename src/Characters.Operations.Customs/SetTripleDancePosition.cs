using Characters.AI;
using Level;
using UnityEngine;

namespace Characters.Operations.Customs;

public class SetTripleDancePosition : CharacterOperation
{
	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private Transform _object;

	[SerializeField]
	private float _minDistanceFromSide;

	[SerializeField]
	private float _offsetY;

	[SerializeField]
	private float _offsetX;

	public override void Run(Character owner)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Character target = _controller.target;
		if (!((Object)(object)target == (Object)null))
		{
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			float x = ((Component)target).transform.position.x;
			x += _offsetX;
			Evaluate(ref x);
			float y = ((Bounds)(ref bounds)).max.y;
			y += _offsetY;
			_object.position = Vector2.op_Implicit(new Vector2(x, y));
		}
	}

	private void Evaluate(ref float x)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = Map.Instance.bounds;
		float num = ((Bounds)(ref bounds)).max.x - _minDistanceFromSide;
		float num2 = ((Bounds)(ref bounds)).min.x + _minDistanceFromSide;
		if (num < x)
		{
			x = num;
		}
		if (num2 > x)
		{
			x = num2;
		}
	}
}
