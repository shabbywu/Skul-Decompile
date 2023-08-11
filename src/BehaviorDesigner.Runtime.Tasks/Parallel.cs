namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("하부 테스크가 실패를 반환하면 모든 테스크를 종료하고 실패를 반환함")]
[TaskIcon("{SkinColor}ParallelIcon.png")]
public class Parallel : Composite
{
	private int currentChildIndex;

	private TaskStatus[] executionStatus;

	public override void OnAwake()
	{
		executionStatus = (TaskStatus[])(object)new TaskStatus[((ParentTask)this).children.Count];
	}

	public override void OnChildStarted(int childIndex)
	{
		currentChildIndex++;
		executionStatus[childIndex] = (TaskStatus)3;
	}

	public override bool CanRunParallelChildren()
	{
		return true;
	}

	public override int CurrentChildIndex()
	{
		return currentChildIndex;
	}

	public override bool CanExecute()
	{
		return currentChildIndex < ((ParentTask)this).children.Count;
	}

	public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected I4, but got Unknown
		executionStatus[childIndex] = (TaskStatus)(int)childStatus;
	}

	public override TaskStatus OverrideStatus(TaskStatus status)
	{
		bool flag = true;
		for (int i = 0; i < executionStatus.Length; i++)
		{
			if ((int)executionStatus[i] == 3)
			{
				flag = false;
			}
			else if ((int)executionStatus[i] == 1)
			{
				return (TaskStatus)1;
			}
		}
		if (flag)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)3;
	}

	public override void OnConditionalAbort(int childIndex)
	{
		currentChildIndex = 0;
		for (int i = 0; i < executionStatus.Length; i++)
		{
			executionStatus[i] = (TaskStatus)0;
		}
	}

	public override void OnEnd()
	{
		for (int i = 0; i < executionStatus.Length; i++)
		{
			executionStatus[i] = (TaskStatus)0;
		}
		currentChildIndex = 0;
	}
}
