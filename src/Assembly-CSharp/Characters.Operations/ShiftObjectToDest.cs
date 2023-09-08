using UnityEngine;

namespace Characters.Operations;

public class ShiftObjectToDest : CharacterOperation
{
	[SerializeField]
	private Transform _destination;

	[SerializeField]
	private Transform _object;

	[SerializeField]
	private float _offsetY;

	[SerializeField]
	private float _offsetX;

	[SerializeField]
	private bool _fromPlatform;

	public override void Run(Character owner)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		float num = ((Component)_destination).transform.position.x + _offsetX;
		float y;
		if (_fromPlatform)
		{
			Bounds bounds = owner.movement.controller.collisionState.lastStandingCollider.bounds;
			y = ((Bounds)(ref bounds)).max.y;
		}
		else
		{
			y = ((Component)_destination).transform.position.y;
		}
		y += _offsetY;
		_object.position = Vector2.op_Implicit(new Vector2(num, y));
	}
}
