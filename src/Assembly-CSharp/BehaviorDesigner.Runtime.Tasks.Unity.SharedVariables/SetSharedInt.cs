namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;

[TaskDescription("Sets the SharedInt variable to the specified object. Returns Success.")]
[TaskCategory("Unity/SharedVariable")]
public class SetSharedInt : Action
{
	[Tooltip("The value to set the SharedInt to")]
	public SharedInt targetValue;

	[Tooltip("The SharedInt to set")]
	[RequiredField]
	public SharedInt targetVariable;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<int>)targetVariable).Value = ((SharedVariable<int>)targetValue).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetValue = 0;
		targetVariable = 0;
	}
}
