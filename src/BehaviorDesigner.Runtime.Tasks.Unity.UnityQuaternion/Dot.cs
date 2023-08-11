using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;

[TaskDescription("Stores the dot product between two rotations.")]
[TaskCategory("Unity/Quaternion")]
public class Dot : Action
{
	[Tooltip("The first rotation")]
	public SharedQuaternion leftRotation;

	[Tooltip("The second rotation")]
	public SharedQuaternion rightRotation;

	[RequiredField]
	[Tooltip("The stored result")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<float>)storeResult).Value = Quaternion.Dot(((SharedVariable<Quaternion>)leftRotation).Value, ((SharedVariable<Quaternion>)rightRotation).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		leftRotation = (rightRotation = Quaternion.identity);
		storeResult = 0f;
	}
}
