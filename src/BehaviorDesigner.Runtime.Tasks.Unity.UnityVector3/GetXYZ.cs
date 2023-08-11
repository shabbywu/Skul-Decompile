using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;

[TaskCategory("Unity/Vector3")]
[TaskDescription("Stores the X, Y, and Z values of the Vector3.")]
public class GetXYZ : Action
{
	[Tooltip("The Vector3 to get the values of")]
	public SharedVector3 vector3Variable;

	[Tooltip("The X value")]
	[RequiredField]
	public SharedFloat storeX;

	[Tooltip("The Y value")]
	[RequiredField]
	public SharedFloat storeY;

	[RequiredField]
	[Tooltip("The Z value")]
	public SharedFloat storeZ;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<float>)storeX).Value = ((SharedVariable<Vector3>)vector3Variable).Value.x;
		((SharedVariable<float>)storeY).Value = ((SharedVariable<Vector3>)vector3Variable).Value.y;
		((SharedVariable<float>)storeZ).Value = ((SharedVariable<Vector3>)vector3Variable).Value.z;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		vector3Variable = Vector3.zero;
		storeX = 0f;
		storeY = 0f;
		storeZ = 0f;
	}
}
