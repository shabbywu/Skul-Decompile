namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString;

[TaskDescription("Sets the variable string to the value string.")]
[TaskCategory("Unity/String")]
public class SetString : Action
{
	[Tooltip("The target string")]
	[RequiredField]
	public SharedString variable;

	[Tooltip("The value string")]
	public SharedString value;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<string>)variable).Value = ((SharedVariable<string>)value).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		variable = "";
		value = "";
	}
}
