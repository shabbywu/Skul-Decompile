using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug;

[TaskDescription("Log a variable value.")]
[TaskCategory("Unity/Debug")]
public class LogValue : Action
{
	[Tooltip("The variable to output")]
	public SharedGenericVariable variable;

	public override TaskStatus OnUpdate()
	{
		Debug.Log(((SharedVariable<GenericVariable>)(object)variable).Value.value.GetValue());
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		variable = null;
	}
}
