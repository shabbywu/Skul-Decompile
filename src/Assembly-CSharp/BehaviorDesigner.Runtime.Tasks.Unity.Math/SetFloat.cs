namespace BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskIcon("Assets/Behavior Designer/Icon/SetFloat.png")]
[TaskCategory("Unity/Math")]
[TaskDescription("Sets a float value")]
public class SetFloat : Action
{
	[Tooltip("The float value to set")]
	public SharedFloat floatValue;

	[Tooltip("The variable to store the result")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<float>)storeResult).Value = ((SharedVariable<float>)floatValue).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		floatValue = 0f;
		storeResult = 0f;
	}
}
