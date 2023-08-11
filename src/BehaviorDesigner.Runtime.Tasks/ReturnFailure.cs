namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}ReturnFailureIcon.png")]
[TaskDescription("The return failure task will always return failure except when the child task is running.")]
public class ReturnFailure : Decorator
{
	private TaskStatus executionStatus;

	public override bool CanExecute()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		if ((int)executionStatus != 0)
		{
			return (int)executionStatus == 3;
		}
		return true;
	}

	public override void OnChildExecuted(TaskStatus childStatus)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		executionStatus = childStatus;
	}

	public override TaskStatus Decorate(TaskStatus status)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if ((int)status == 2)
		{
			return (TaskStatus)1;
		}
		return status;
	}

	public override void OnEnd()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		executionStatus = (TaskStatus)0;
	}
}
