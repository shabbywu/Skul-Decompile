using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;

[TaskCategory("Unity/Vector3")]
[TaskDescription("Stores the up vector value.")]
public class GetUpVector : Action
{
	[RequiredField]
	[Tooltip("The stored result")]
	public SharedVector3 storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector3>)storeResult).Value = Vector3.up;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		storeResult = Vector3.zero;
	}
}
