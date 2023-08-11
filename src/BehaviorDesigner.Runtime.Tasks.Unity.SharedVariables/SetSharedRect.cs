using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;

[TaskDescription("Sets the SharedRect variable to the specified object. Returns Success.")]
[TaskCategory("Unity/SharedVariable")]
public class SetSharedRect : Action
{
	[Tooltip("The value to set the SharedRect to")]
	public SharedRect targetValue;

	[Tooltip("The SharedRect to set")]
	[RequiredField]
	public SharedRect targetVariable;

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable<Rect>)targetVariable).Value = ((SharedVariable<Rect>)targetValue).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		targetValue = (SharedRect)default(Rect);
		targetVariable = (SharedRect)default(Rect);
	}
}
