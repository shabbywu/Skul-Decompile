using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;

[TaskDescription("Returns the time in seconds it took to complete the last frame.")]
[TaskCategory("Unity/Time")]
public class GetDeltaTime : Action
{
	[Tooltip("The variable to store the result")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<float>)storeResult).Value = Time.deltaTime;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		storeResult = 0f;
	}
}
