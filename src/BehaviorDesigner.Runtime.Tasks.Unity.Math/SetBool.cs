namespace BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskCategory("Unity/Math")]
[TaskDescription("Sets a bool value")]
[TaskIcon("Assets/Behavior Designer/Icon/SetBool.png")]
public class SetBool : Action
{
	[Tooltip("The bool value to set")]
	public SharedBool boolValue;

	[Tooltip("The variable to store the result")]
	public SharedBool storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<bool>)storeResult).Value = ((SharedVariable<bool>)boolValue).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		boolValue = false;
	}
}
