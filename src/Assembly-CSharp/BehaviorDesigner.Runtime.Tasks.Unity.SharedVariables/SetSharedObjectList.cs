using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;

[TaskDescription("Sets the SharedObjectList variable to the specified object. Returns Success.")]
[TaskCategory("Unity/SharedVariable")]
public class SetSharedObjectList : Action
{
	[Tooltip("The value to set the SharedObjectList to.")]
	public SharedObjectList targetValue;

	[RequiredField]
	[Tooltip("The SharedObjectList to set")]
	public SharedObjectList targetVariable;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<List<Object>>)targetVariable).Value = ((SharedVariable<List<Object>>)targetValue).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetValue = null;
		targetVariable = null;
	}
}
