using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;

[TaskCategory("Unity/Vector3")]
[TaskDescription("Stores the dot product of two Vector3 values.")]
public class Dot : Action
{
	[Tooltip("The left hand side of the dot product")]
	public SharedVector3 leftHandSide;

	[Tooltip("The right hand side of the dot product")]
	public SharedVector3 rightHandSide;

	[RequiredField]
	[Tooltip("The dot product result")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<float>)storeResult).Value = Vector3.Dot(((SharedVariable<Vector3>)leftHandSide).Value, ((SharedVariable<Vector3>)rightHandSide).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		leftHandSide = Vector3.zero;
		rightHandSide = Vector3.zero;
		storeResult = 0f;
	}
}
