using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

[TaskDescription("Returns Success if the GameObject is active in the hierarchy, otherwise Failure.")]
[TaskCategory("Unity/GameObject")]
public class ActiveSelf : Conditional
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	public override TaskStatus OnUpdate()
	{
		if (((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value).activeSelf)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		targetGameObject = null;
	}
}
