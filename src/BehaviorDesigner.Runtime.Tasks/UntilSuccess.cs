namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("The until success task will keep executing its child task until the child task returns success.")]
[TaskIcon("{SkinColor}UntilSuccessIcon.png")]
public class UntilSuccess : Decorator
{
	private TaskStatus executionStatus;

	public override bool CanExecute()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)executionStatus != 1)
		{
			return (int)executionStatus == 0;
		}
		return true;
	}

	public override void OnChildExecuted(TaskStatus childStatus)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		executionStatus = childStatus;
	}

	public override void OnEnd()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		executionStatus = (TaskStatus)0;
	}
}
