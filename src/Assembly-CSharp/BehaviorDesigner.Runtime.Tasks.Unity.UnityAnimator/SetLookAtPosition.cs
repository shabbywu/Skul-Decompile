using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;

[TaskCategory("Unity/Animator")]
[TaskDescription("Sets the look at position. Returns Success.")]
public class SetLookAtPosition : Action
{
	[Tooltip("The position to lookAt")]
	public SharedVector3 position;

	private Animator animator;

	private bool positionSet;

	public override void OnStart()
	{
		animator = ((Task)this).GetComponent<Animator>();
		positionSet = false;
	}

	public override TaskStatus OnUpdate()
	{
		if ((Object)(object)animator == (Object)null)
		{
			Debug.LogWarning((object)"Animator is null");
			return (TaskStatus)1;
		}
		if (positionSet)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)3;
	}

	public override void OnAnimatorIK()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)animator == (Object)null))
		{
			animator.SetLookAtPosition(((SharedVariable<Vector3>)position).Value);
			positionSet = true;
		}
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		position = Vector3.zero;
	}
}
