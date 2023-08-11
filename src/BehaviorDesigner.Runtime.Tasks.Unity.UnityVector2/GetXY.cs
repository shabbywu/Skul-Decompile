using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;

[TaskCategory("Unity/Vector2")]
[TaskDescription("Stores the X and Y values of the Vector2.")]
public class GetXY : Action
{
	[Tooltip("The Vector2 to get the values of")]
	public SharedVector2 vector2Variable;

	[RequiredField]
	[Tooltip("The X value")]
	public SharedFloat storeX;

	[RequiredField]
	[Tooltip("The Y value")]
	public SharedFloat storeY;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<float>)storeX).Value = ((SharedVariable<Vector2>)vector2Variable).Value.x;
		((SharedVariable<float>)storeY).Value = ((SharedVariable<Vector2>)vector2Variable).Value.y;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		vector2Variable = Vector2.zero;
		storeX = (storeY = 0f);
	}
}
