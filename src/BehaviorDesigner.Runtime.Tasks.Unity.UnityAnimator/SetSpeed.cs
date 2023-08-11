using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;

[TaskCategory("Unity/Animator")]
[TaskDescription("Sets the playback speed of the Animator. 1 is normal playback speed. Returns Success.")]
public class SetSpeed : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The playback speed of the Animator")]
	public SharedFloat speed;

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
		animator.speed = ((SharedVariable<float>)speed).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		speed = 0f;
	}
}
