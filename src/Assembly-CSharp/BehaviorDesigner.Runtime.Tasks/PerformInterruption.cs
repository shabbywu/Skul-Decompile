namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Perform the actual interruption. This will immediately stop the specified tasks from running and will return success or failure depending on the value of interrupt success.")]
[TaskIcon("{SkinColor}PerformInterruptionIcon.png")]
public class PerformInterruption : Action
{
	[Tooltip("The list of tasks to interrupt. Can be any number of tasks")]
	public Interrupt[] interruptTasks;

	[Tooltip("When we interrupt the task should we return a task status of success?")]
	public SharedBool interruptSuccess;

	public override TaskStatus OnUpdate()
	{
		for (int i = 0; i < interruptTasks.Length; i++)
		{
			interruptTasks[i].DoInterrupt((TaskStatus)((!((SharedVariable<bool>)interruptSuccess).Value) ? 1 : 2));
		}
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		interruptTasks = null;
		interruptSuccess = false;
	}
}
