using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour;

[TaskDescription("Enables/Disables the object. Returns Success.")]
[TaskCategory("Unity/Behaviour")]
public class SetEnabled : Action
{
	[Tooltip("The Behavior to use")]
	public SharedBehaviour specifiedObject;

	[Tooltip("The enabled/disabled state")]
	public SharedBool enabled;

	public override TaskStatus OnUpdate()
	{
		if (specifiedObject == null)
		{
			Debug.LogWarning((object)"SpecifiedObject is null");
			return (TaskStatus)1;
		}
		((SharedVariable<Behaviour>)specifiedObject).Value.enabled = ((SharedVariable<bool>)enabled).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		((SharedVariable<Behaviour>)specifiedObject).Value = null;
		enabled = false;
	}
}
