namespace BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskDescription("Is the float a positive value?")]
[TaskCategory("Unity/Math")]
public class IsFloatPositive : Conditional
{
	[Tooltip("The float to check if positive")]
	public SharedFloat floatVariable;

	public override TaskStatus OnUpdate()
	{
		if (((SharedVariable<float>)floatVariable).Value > 0f)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		floatVariable = 0f;
	}
}
