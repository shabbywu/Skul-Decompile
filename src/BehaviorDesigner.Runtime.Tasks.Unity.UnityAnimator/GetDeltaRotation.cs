using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;

[TaskCategory("Unity/Animator")]
[TaskDescription("Gets the avatar delta rotation for the last evaluated frame. Returns Success.")]
public class GetDeltaRotation : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The avatar delta rotation")]
	[RequiredField]
	public SharedQuaternion storeValue;

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
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)animator == (Object)null)
		{
			Debug.LogWarning((object)"Animator is null");
			return (TaskStatus)1;
		}
		((SharedVariable<Quaternion>)storeValue).Value = animator.deltaRotation;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (storeValue != null)
		{
			((SharedVariable<Quaternion>)storeValue).Value = Quaternion.identity;
		}
	}
}
