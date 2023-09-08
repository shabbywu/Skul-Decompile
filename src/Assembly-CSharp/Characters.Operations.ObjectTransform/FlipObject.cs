using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class FlipObject : Operation
{
	[SerializeField]
	private Transform _object;

	[SerializeField]
	private bool _flipX;

	[SerializeField]
	private bool _flipY;

	public override void Run()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		float num = (_flipX ? (0f - _object.localScale.x) : _object.localScale.x);
		float num2 = (_flipY ? (0f - _object.localScale.y) : _object.localScale.y);
		_object.localScale = Vector2.op_Implicit(new Vector2(num, num2));
	}
}
