using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Collider;

[TaskDescription("Stores the enabled state of the collider. Returns Success.")]
[TaskCategory("Unity/Collider")]
public class GetEnabled : Action
{
	[Tooltip("The Collider to use")]
	public SharedCollider specifiedCollider;

	[RequiredField]
	[Tooltip("The enabled/disabled state")]
	public SharedBool storeValue;

	public override TaskStatus OnUpdate()
	{
		if (specifiedCollider == null)
		{
			Debug.LogWarning((object)"SpecifiedObject is null");
			return (TaskStatus)1;
		}
		((SharedVariable<bool>)storeValue).Value = ((Behaviour)((SharedVariable<Collider2D>)specifiedCollider).Value).enabled;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		((SharedVariable<Collider2D>)specifiedCollider).Value = null;
		storeValue = false;
	}
}
