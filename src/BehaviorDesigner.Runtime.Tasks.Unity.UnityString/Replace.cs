namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString;

[TaskDescription("Replaces a string with the new string")]
[TaskCategory("Unity/String")]
public class Replace : Action
{
	[Tooltip("The target string")]
	public SharedString targetString;

	[Tooltip("The old replace")]
	public SharedString oldString;

	[Tooltip("The new string")]
	public SharedString newString;

	[Tooltip("The stored result")]
	[RequiredField]
	public SharedString storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<string>)storeResult).Value = ((SharedVariable<string>)targetString).Value.Replace(((SharedVariable<string>)oldString).Value, ((SharedVariable<string>)newString).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetString = "";
		oldString = "";
		newString = "";
		storeResult = "";
	}
}
