using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;

[TaskCategory("Unity/Vector2")]
[TaskDescription("Stores the dot product of two Vector2 values.")]
public class Dot : Action
{
	[Tooltip("The left hand side of the dot product")]
	public SharedVector2 leftHandSide;

	[Tooltip("The right hand side of the dot product")]
	public SharedVector2 rightHandSide;

	[Tooltip("The dot product result")]
	[RequiredField]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<float>)storeResult).Value = Vector2.Dot(((SharedVariable<Vector2>)leftHandSide).Value, ((SharedVariable<Vector2>)rightHandSide).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		leftHandSide = Vector2.zero;
		rightHandSide = Vector2.zero;
		storeResult = 0f;
	}
}
