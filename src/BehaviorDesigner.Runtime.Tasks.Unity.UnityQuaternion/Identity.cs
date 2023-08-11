using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;

[TaskCategory("Unity/Quaternion")]
[TaskDescription("Stores the quaternion identity.")]
public class Identity : Action
{
	[RequiredField]
	[Tooltip("The identity")]
	public SharedQuaternion storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Quaternion>)storeResult).Value = Quaternion.identity;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		storeResult = Quaternion.identity;
	}
}
