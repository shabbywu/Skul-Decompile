namespace BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskDescription("Sets an int value")]
[TaskCategory("Unity/Math")]
public class SetInt : Action
{
	[Tooltip("The int value to set")]
	public SharedInt intValue;

	[Tooltip("The variable to store the result")]
	public SharedInt storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<int>)storeResult).Value = ((SharedVariable<int>)intValue).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		intValue = 0;
		storeResult = 0;
	}
}
