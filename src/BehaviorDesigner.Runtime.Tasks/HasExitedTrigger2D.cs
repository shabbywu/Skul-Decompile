using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Physics")]
[TaskDescription("Returns success when an object exits the 2D trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
public class HasExitedTrigger2D : Conditional
{
	[Tooltip("The tag of the GameObject to check for a trigger against")]
	public SharedString tag = "";

	[Tooltip("The object that exited the trigger")]
	public SharedGameObject otherGameObject;

	private bool exitedTrigger;

	public override TaskStatus OnUpdate()
	{
		if (exitedTrigger)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnEnd()
	{
		exitedTrigger = false;
	}

	public override void OnTriggerExit2D(Collider2D other)
	{
		if (string.IsNullOrEmpty(((SharedVariable<string>)tag).Value) || ((Component)other).gameObject.CompareTag(((SharedVariable<string>)tag).Value))
		{
			((SharedVariable<GameObject>)otherGameObject).Value = ((Component)other).gameObject;
			exitedTrigger = true;
		}
	}

	public override void OnReset()
	{
		tag = "";
		otherGameObject = null;
	}
}
