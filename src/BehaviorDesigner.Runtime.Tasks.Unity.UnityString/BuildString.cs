namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString;

[TaskDescription("Creates a string from multiple other strings.")]
[TaskCategory("Unity/String")]
public class BuildString : Action
{
	[Tooltip("The array of strings")]
	public SharedString[] source;

	[Tooltip("The stored result")]
	[RequiredField]
	public SharedString storeResult;

	public override TaskStatus OnUpdate()
	{
		for (int i = 0; i < source.Length; i++)
		{
			SharedString sharedString = storeResult;
			((SharedVariable<string>)sharedString).Value = ((SharedVariable<string>)sharedString).Value + (object)source[i];
		}
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		source = null;
		storeResult = null;
	}
}
