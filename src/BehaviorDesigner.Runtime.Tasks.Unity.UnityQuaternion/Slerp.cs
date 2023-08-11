using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;

[TaskCategory("Unity/Quaternion")]
[TaskDescription("Spherically lerp between two quaternions.")]
public class Slerp : Action
{
	[Tooltip("The from rotation")]
	public SharedQuaternion fromQuaternion;

	[Tooltip("The to rotation")]
	public SharedQuaternion toQuaternion;

	[Tooltip("The amount to lerp")]
	public SharedFloat amount;

	[Tooltip("The stored result")]
	[RequiredField]
	public SharedQuaternion storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Quaternion>)storeResult).Value = Quaternion.Slerp(((SharedVariable<Quaternion>)fromQuaternion).Value, ((SharedVariable<Quaternion>)toQuaternion).Value, ((SharedVariable<float>)amount).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		fromQuaternion = (toQuaternion = (storeResult = Quaternion.identity));
		amount = 0f;
	}
}
