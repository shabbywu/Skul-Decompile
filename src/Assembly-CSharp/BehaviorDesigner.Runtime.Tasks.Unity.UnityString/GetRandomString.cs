using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString;

[TaskCategory("Unity/String")]
[TaskDescription("Randomly selects a string from the array of strings.")]
public class GetRandomString : Action
{
	[Tooltip("The array of strings")]
	public SharedString[] source;

	[RequiredField]
	[Tooltip("The stored result")]
	public SharedString storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<string>)storeResult).Value = ((SharedVariable<string>)source[Random.Range(0, source.Length)]).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		source = null;
		storeResult = null;
	}
}
