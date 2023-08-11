using UnityEngine;

namespace Characters.Operations.SetPosition;

public sealed class ToLinear : Policy
{
	[Range(0f, 1f)]
	[SerializeField]
	private float _value;

	[SerializeField]
	private Transform _from;

	[SerializeField]
	private Transform _to;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Lerp(Vector2.op_Implicit(_from.position), Vector2.op_Implicit(_to.position), _value);
	}
}
