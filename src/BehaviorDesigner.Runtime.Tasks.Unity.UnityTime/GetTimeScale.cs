using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;

[TaskCategory("Unity/Time")]
[TaskDescription("Returns the scale at which time is passing.")]
public class GetTimeScale : Action
{
	[Tooltip("The variable to store the result")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<float>)storeResult).Value = Time.timeScale;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		storeResult = 0f;
	}
}
