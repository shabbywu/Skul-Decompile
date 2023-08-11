namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("이 노드의 자식노드는 오직 한번만 실행, 두번째 실행부터는 실패를 반환")]
public class RunOnlyOnce : Decorator
{
	private TaskStatus executionStatus;

	public override bool CanExecute()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return (int)executionStatus == 0;
	}

	public override void OnChildExecuted(TaskStatus childStatus)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		executionStatus = childStatus;
	}

	public override TaskStatus OverrideStatus(TaskStatus status)
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
}
