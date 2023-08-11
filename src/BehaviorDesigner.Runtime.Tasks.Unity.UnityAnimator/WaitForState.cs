using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;

[TaskCategory("Unity/Animator")]
[TaskDescription("Waits for the Animator to reach the specified state.")]
public class WaitForState : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The name of the state")]
	public SharedString stateName;

	[Tooltip("The layer where the state is")]
	public SharedInt layer = -1;

	private Animator animator;

	private GameObject prevGameObject;

	private int stateHash;

	public override void OnAwake()
	{
		stateHash = Animator.StringToHash(((SharedVariable<string>)stateName).Value);
	}

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			animator = defaultGameObject.GetComponent<Animator>();
			prevGameObject = defaultGameObject;
			if (!animator.HasState(((SharedVariable<int>)layer).Value, stateHash))
			{
				Debug.LogError((object)("Error: The Animator does not have the state " + ((SharedVariable<string>)stateName).Value + " on layer " + ((SharedVariable<int>)layer).Value));
			}
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)animator == (Object)null)
		{
			Debug.LogWarning((object)"Animator is null");
			return (TaskStatus)1;
		}
		AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(((SharedVariable<int>)layer).Value);
		if (((AnimatorStateInfo)(ref currentAnimatorStateInfo)).shortNameHash != stateHash)
		{
			return (TaskStatus)3;
		}
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		stateName = "";
		layer = -1;
	}
}
