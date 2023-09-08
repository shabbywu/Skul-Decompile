using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;

[TaskCategory("Unity/Vector2")]
[TaskDescription("Sets the value of the Vector2.")]
public class SetValue : Action
{
	[Tooltip("The Vector2 to get the values of")]
	public SharedVector2 vector2Value;

	[Tooltip("The Vector2 to set the values of")]
	public SharedVector2 vector2Variable;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector2>)vector2Variable).Value = ((SharedVariable<Vector2>)vector2Value).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		vector2Value = Vector2.zero;
		vector2Variable = Vector2.zero;
	}
}
