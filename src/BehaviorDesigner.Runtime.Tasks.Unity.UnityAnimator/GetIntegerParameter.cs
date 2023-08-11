using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;

[TaskDescription("Stores the integer parameter on an animator. Returns Success.")]
[TaskCategory("Unity/Animator")]
public class GetIntegerParameter : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The name of the parameter")]
	public SharedString paramaterName;

	[RequiredField]
	[Tooltip("The value of the integer parameter")]
	public SharedInt storeValue;

	private Animator animator;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			animator = defaultGameObject.GetComponent<Animator>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if ((Object)(object)animator == (Object)null)
		{
			Debug.LogWarning((object)"Animator is null");
			return (TaskStatus)1;
		}
		((SharedVariable<int>)storeValue).Value = animator.GetInteger(((SharedVariable<string>)paramaterName).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		paramaterName = "";
		storeValue = 0;
	}
}
