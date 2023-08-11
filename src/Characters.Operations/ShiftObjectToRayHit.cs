using UnityEngine;

namespace Characters.Operations;

public class ShiftObjectToRayHit : CharacterOperation
{
	[SerializeField]
	private Transform _origin;

	[SerializeField]
	private Transform _object;

	[SerializeField]
	private float _offsetX = -1f;

	[SerializeField]
	private float _rayDistance;

	public override void Run(Character owner)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = ((owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
		RaycastHit2D val2 = Physics2D.Raycast(Vector2.op_Implicit(_origin.position), val, _rayDistance, LayerMask.op_Implicit(Layers.groundMask));
		if (RaycastHit2D.op_Implicit(val2))
		{
			_object.position = Vector2.op_Implicit(new Vector2(((RaycastHit2D)(ref val2)).point.x + _offsetX * val.x, ((RaycastHit2D)(ref val2)).point.y));
			return;
		}
		float num = val.x * _rayDistance;
		float num2 = val.y * _rayDistance;
		_object.position = Vector2.op_Implicit(new Vector2(_origin.position.x + num + _offsetX * val.x, _origin.position.y + num2));
	}
}
