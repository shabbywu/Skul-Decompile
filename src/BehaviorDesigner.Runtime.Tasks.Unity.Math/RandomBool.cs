using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskDescription("Sets a random bool value")]
[TaskCategory("Unity/Math")]
public class RandomBool : Action
{
	[Tooltip("The variable to store the result")]
	public SharedBool storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<bool>)storeResult).Value = Random.value < 0.5f;
		return (TaskStatus)2;
	}
}
