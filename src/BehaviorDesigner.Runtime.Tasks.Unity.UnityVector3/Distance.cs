using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;

[TaskDescription("Returns the distance between two Vector3s.")]
[TaskCategory("Unity/Vector3")]
public class Distance : Action
{
	[Tooltip("The first Vector3")]
	public SharedVector3 firstVector3;

	[Tooltip("The second Vector3")]
	public SharedVector3 secondVector3;

	[RequiredField]
	[Tooltip("The distance")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<float>)storeResult).Value = Vector3.Distance(((SharedVariable<Vector3>)firstVector3).Value, ((SharedVariable<Vector3>)secondVector3).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		firstVector3 = Vector3.zero;
		secondVector3 = Vector3.zero;
		storeResult = 0f;
	}
}
