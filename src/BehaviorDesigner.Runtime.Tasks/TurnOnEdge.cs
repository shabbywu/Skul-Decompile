using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}TurnOnEdge.png")]
[TaskDescription("Allows multiple action tasks to be added to a single node.")]
public sealed class TurnOnEdge : Action
{
	[SerializeField]
	private SharedVector2 _moveDirection;

	[SerializeField]
	private SharedCharacter _owner;

	public override TaskStatus OnUpdate()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_owner).Value;
		Vector2 direction = ((SharedVariable<Vector2>)_moveDirection).Value;
		value.movement.TurnOnEdge(ref direction);
		((SharedVariable)_moveDirection).SetValue((object)direction);
		return (TaskStatus)2;
	}
}
