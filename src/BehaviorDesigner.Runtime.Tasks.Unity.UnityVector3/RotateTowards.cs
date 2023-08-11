using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;

[TaskCategory("Unity/Vector3")]
[TaskDescription("Rotate the current rotation to the target rotation.")]
public class RotateTowards : Action
{
	[Tooltip("The current rotation in euler angles")]
	public SharedVector3 currentRotation;

	[Tooltip("The target rotation in euler angles")]
	public SharedVector3 targetRotation;

	[Tooltip("The maximum delta of the degrees")]
	public SharedFloat maxDegreesDelta;

	[Tooltip("The maximum delta of the magnitude")]
	public SharedFloat maxMagnitudeDelta;

	[Tooltip("The rotation resut")]
	[RequiredField]
	public SharedVector3 storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector3>)storeResult).Value = Vector3.RotateTowards(((SharedVariable<Vector3>)currentRotation).Value, ((SharedVariable<Vector3>)targetRotation).Value, ((SharedVariable<float>)maxDegreesDelta).Value * ((float)System.Math.PI / 180f) * Time.deltaTime, ((SharedVariable<float>)maxMagnitudeDelta).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		currentRotation = Vector3.zero;
		targetRotation = Vector3.zero;
		storeResult = Vector3.zero;
		maxDegreesDelta = 0f;
		maxMagnitudeDelta = 0f;
	}
}
