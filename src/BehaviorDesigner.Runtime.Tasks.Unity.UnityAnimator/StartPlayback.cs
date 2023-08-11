using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;

[TaskCategory("Unity/Animator")]
[TaskDescription("Sets the animator in playback mode.")]
public class StartPlayback : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

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
		animator.StartPlayback();
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
	}
}
