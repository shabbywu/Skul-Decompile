using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;

[TaskDescription("Returns the time in second since the start of the game.")]
[TaskCategory("Unity/Time")]
public class GetTime : Action
{
	[Tooltip("The variable to store the result")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<float>)storeResult).Value = Time.time;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		storeResult = 0f;
	}
}
