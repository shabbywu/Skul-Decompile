using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour;

[TaskCategory("Unity/Behaviour")]
[TaskDescription("Stores the enabled state of the object. Returns Success.")]
public class GetEnabled : Action
{
	[Tooltip("The Behavior to use")]
	public SharedBehaviour specifiedObject;

	[Tooltip("The enabled/disabled state")]
	[RequiredField]
	public SharedBool storeValue;

	public override TaskStatus OnUpdate()
	{
		if (specifiedObject == null)
		{
			Debug.LogWarning((object)"SpecifiedObject is null");
			return (TaskStatus)1;
		}
		((SharedVariable<bool>)storeValue).Value = ((SharedVariable<Behaviour>)specifiedObject).Value.enabled;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		((SharedVariable<Behaviour>)specifiedObject).Value = null;
		storeValue = false;
	}
}
