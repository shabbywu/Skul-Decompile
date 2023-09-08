using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}CooldownIcon.png")]
[TaskDescription("Waits the specified duration after the child has completed before returning the child's status of success or failure.")]
public class Cooldown : Decorator
{
	public SharedFloat duration = 2f;

	private TaskStatus executionStatus;

	private float cooldownTime = -1f;

	public override bool CanExecute()
	{
		if (cooldownTime == -1f)
		{
			return true;
		}
		return cooldownTime + ((SharedVariable<float>)duration).Value > Time.time;
	}

	public override int CurrentChildIndex()
	{
		if (cooldownTime == -1f)
		{
			return 0;
		}
		return -1;
	}

	public override void OnChildExecuted(TaskStatus childStatus)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		executionStatus = childStatus;
		if ((int)executionStatus == 1 || (int)executionStatus == 2)
		{
			cooldownTime = Time.time;
		}
	}

	public override TaskStatus OverrideStatus()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if (!((ParentTask)this).CanExecute())
		{
			return (TaskStatus)3;
		}
		return executionStatus;
	}

	public override TaskStatus OverrideStatus(TaskStatus status)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if ((int)status == 3)
		{
			return status;
		}
		return executionStatus;
	}

	public override void OnEnd()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		executionStatus = (TaskStatus)0;
		cooldownTime = -1f;
	}
}
