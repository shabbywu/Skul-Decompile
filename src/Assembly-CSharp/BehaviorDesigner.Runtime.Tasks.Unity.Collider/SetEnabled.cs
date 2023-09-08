using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Collider;

[TaskCategory("Unity/Collider")]
[TaskDescription("Enables/Disables the collider. Returns Success.")]
public class SetEnabled : Action
{
	[Tooltip("The Behavior to use")]
	public SharedCollider specifiedCollider;

	[Tooltip("The enabled/disabled state")]
	public SharedBool enabled;

	public override TaskStatus OnUpdate()
	{
		if (specifiedCollider == null)
		{
			Debug.LogWarning((object)"SpecifiedCollider is null");
			return (TaskStatus)1;
		}
		((Behaviour)((SharedVariable<Collider2D>)specifiedCollider).Value).enabled = ((SharedVariable<bool>)enabled).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		((SharedVariable<Collider2D>)specifiedCollider).Value = null;
		enabled = false;
	}
}
