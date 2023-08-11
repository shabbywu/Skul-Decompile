using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;

[TaskCategory("Unity/Vector3")]
[TaskDescription("Sets the value of the Vector3.")]
public class SetValue : Action
{
	[Tooltip("The Vector3 to get the values of")]
	public SharedVector3 vector3Value;

	[Tooltip("The Vector3 to set the values of")]
	public SharedVector3 vector3Variable;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector3>)vector3Variable).Value = ((SharedVariable<Vector3>)vector3Value).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		vector3Value = Vector3.zero;
		vector3Variable = Vector3.zero;
	}
}
