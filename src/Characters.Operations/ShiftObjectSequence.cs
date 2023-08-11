using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class ShiftObjectSequence : CharacterOperation
{
	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private Transform _object;

	[SerializeField]
	private Transform _origin;

	[SerializeField]
	private int _index;

	[SerializeField]
	private float _offsetY;

	[SerializeField]
	private float _offsetX;

	[SerializeField]
	private float _deltaY;

	[SerializeField]
	private float _deltaX;

	[SerializeField]
	private bool _fromPlatform;

	public override void Run(Character owner)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Character target = _controller.target;
		if (!((Object)(object)target == (Object)null))
		{
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			float num = _origin.position.x + _offsetX;
			float num2 = (_fromPlatform ? ((Bounds)(ref bounds)).max.y : _origin.position.y);
			num2 += _offsetY;
			_object.position = Vector2.op_Implicit(new Vector2(num, num2));
		}
	}
}
