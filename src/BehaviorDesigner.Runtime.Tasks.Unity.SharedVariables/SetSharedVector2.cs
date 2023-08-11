using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;

[TaskDescription("Sets the SharedVector2 variable to the specified object. Returns Success.")]
[TaskCategory("Unity/SharedVariable")]
public class SetSharedVector2 : Action
{
	[Tooltip("The value to set the SharedVector2 to")]
	public SharedVector2 targetValue;

	[Tooltip("The SharedVector2 to set")]
	[RequiredField]
	public SharedVector2 targetVariable;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector2>)targetVariable).Value = ((SharedVariable<Vector2>)targetValue).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		targetValue = Vector2.zero;
		targetVariable = Vector2.zero;
	}
}
