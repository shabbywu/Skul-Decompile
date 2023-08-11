using Characters.Operations.SetPosition;
using UnityEngine;

namespace BT;

public class SetPositionTo : Node
{
	[SerializeField]
	private Transform _object;

	[SerializeField]
	private TargetInfo _targetInfo;

	protected override NodeState UpdateDeltatime(Context context)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_object.position = Vector2.op_Implicit(_targetInfo.GetPosition());
		return NodeState.Success;
	}
}
