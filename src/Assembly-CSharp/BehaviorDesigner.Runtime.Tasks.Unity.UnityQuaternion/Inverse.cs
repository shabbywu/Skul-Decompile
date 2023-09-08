using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;

[TaskCategory("Unity/Quaternion")]
[TaskDescription("Stores the inverse of the specified quaternion.")]
public class Inverse : Action
{
	[Tooltip("The target quaternion")]
	public SharedQuaternion targetQuaternion;

	[RequiredField]
	[Tooltip("The stored quaternion")]
	public SharedQuaternion storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Quaternion>)storeResult).Value = Quaternion.Inverse(((SharedVariable<Quaternion>)targetQuaternion).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		targetQuaternion = (storeResult = Quaternion.identity);
	}
}
