using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;

[TaskDescription("Stores the mouse position.")]
[TaskCategory("Unity/Input")]
public class GetMousePosition : Action
{
	[RequiredField]
	[Tooltip("The stored result")]
	public SharedVector3 storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector3>)storeResult).Value = Input.mousePosition;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		storeResult = Vector3.zero;
	}
}
