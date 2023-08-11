using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom;

public sealed class GetTargetVelocity : Action
{
	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private SharedVector2 _storedVelocity;

	public override TaskStatus OnUpdate()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		((SharedVariable)_storedVelocity).SetValue((object)((SharedVariable<Character>)_target).Value.movement.velocity);
		return (TaskStatus)2;
	}
}
