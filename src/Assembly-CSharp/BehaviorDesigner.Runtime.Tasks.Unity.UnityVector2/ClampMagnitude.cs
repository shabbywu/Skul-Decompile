using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;

[TaskCategory("Unity/Vector2")]
[TaskDescription("Clamps the magnitude of the Vector2.")]
public class ClampMagnitude : Action
{
	[Tooltip("The Vector2 to clamp the magnitude of")]
	public SharedVector2 vector2Variable;

	[Tooltip("The max length of the magnitude")]
	public SharedFloat maxLength;

	[Tooltip("The clamp magnitude resut")]
	[RequiredField]
	public SharedVector2 storeResult;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Vector2>)storeResult).Value = Vector2.ClampMagnitude(((SharedVariable<Vector2>)vector2Variable).Value, ((SharedVariable<float>)maxLength).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		vector2Variable = Vector2.zero;
		storeResult = Vector2.zero;
		maxLength = 0f;
	}
}
