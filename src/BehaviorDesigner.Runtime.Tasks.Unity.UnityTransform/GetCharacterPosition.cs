using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;

[TaskCategory("Unity/Transform")]
[TaskDescription("Stores the position of the Transform. Returns Success.")]
public class GetCharacterPosition : Action
{
	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private SharedVector2 _storeValue;

	private Transform _targetTransform;

	public override void OnStart()
	{
		_targetTransform = ((Component)((SharedVariable<Character>)_target).Value).transform;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_targetTransform == (Object)null))
		{
			((SharedVariable<Vector2>)_storeValue).Value = Vector2.op_Implicit(_targetTransform.position);
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
