namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;

[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
[TaskCategory("Unity/SharedVariable")]
public class CompareSharedString : Conditional
{
	[Tooltip("The first variable to compare")]
	public SharedString variable;

	[Tooltip("The variable to compare to")]
	public SharedString compareTo;

	public override TaskStatus OnUpdate()
	{
		if (((SharedVariable<string>)variable).Value.Equals(((SharedVariable<string>)compareTo).Value))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		variable = "";
		compareTo = "";
	}
}
