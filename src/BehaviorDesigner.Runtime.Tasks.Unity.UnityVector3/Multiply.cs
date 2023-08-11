using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;

[TaskDescription("Multiply the Vector3 by a float.")]
[TaskCategory("Unity/Vector3")]
public class Multiply : Action
{
	[Tooltip("The Vector3 to multiply of")]
	public SharedVector3 vector3Variable;

	[Tooltip("The value to multiply the Vector3 of")]
	public SharedFloat multiplyBy;

	[Tooltip("The multiplication resut")]
	[RequiredField]
	public SharedVector3 storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector3>)storeResult).Value = ((SharedVariable<Vector3>)vector3Variable).Value * ((SharedVariable<float>)multiplyBy).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		vector3Variable = Vector3.zero;
		storeResult = Vector3.zero;
		multiplyBy = 0f;
	}
}
