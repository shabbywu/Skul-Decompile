namespace BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskCategory("Unity/Math")]
[TaskDescription("Performs a comparison between two bools.")]
[TaskIcon("Assets/Behavior Designer/Icon/BoolComparison.png")]
public class BoolComparison : Conditional
{
	[Tooltip("The first bool")]
	public SharedBool bool1;

	[Tooltip("The second bool")]
	public SharedBool bool2;

	public override TaskStatus OnUpdate()
	{
		if (((SharedVariable<bool>)bool1).Value == ((SharedVariable<bool>)bool2).Value)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		bool1 = false;
		bool2 = false;
	}
}
