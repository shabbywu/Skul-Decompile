using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;

[TaskCategory("Unity/Vector2")]
[TaskDescription("Normalize the Vector2.")]
public class Normalize : Action
{
	[Tooltip("The Vector2 to normalize")]
	public SharedVector2 vector2Variable;

	[RequiredField]
	[Tooltip("The normalized resut")]
	public SharedVector2 storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		SharedVector2 sharedVector = storeResult;
		Vector2 value = ((SharedVariable<Vector2>)vector2Variable).Value;
		((SharedVariable<Vector2>)sharedVector).Value = ((Vector2)(ref value)).normalized;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		vector2Variable = Vector2.zero;
		storeResult = Vector2.zero;
	}
}
