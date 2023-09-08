using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;

[TaskCategory("Unity/Vector2")]
[TaskDescription("Returns the distance between two Vector2s.")]
public class Distance : Action
{
	[Tooltip("The first Vector2")]
	public SharedVector2 firstVector2;

	[Tooltip("The second Vector2")]
	public SharedVector2 secondVector2;

	[RequiredField]
	[Tooltip("The distance")]
	public SharedFloat storeResult;

	[SerializeField]
	private bool _compareOnlyX;

	public override TaskStatus OnUpdate()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (!_compareOnlyX)
		{
			((SharedVariable<float>)storeResult).Value = Vector2.Distance(((SharedVariable<Vector2>)firstVector2).Value, ((SharedVariable<Vector2>)secondVector2).Value);
		}
		else
		{
			((SharedVariable<float>)storeResult).Value = Mathf.Abs(((SharedVariable<Vector2>)firstVector2).Value.x - ((SharedVariable<Vector2>)secondVector2).Value.x);
		}
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		firstVector2 = Vector2.zero;
		secondVector2 = Vector2.zero;
		storeResult = 0f;
	}
}
