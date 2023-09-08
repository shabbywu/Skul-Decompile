using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Allows multiple action tasks to be added to a single node.")]
[TaskIcon("{SkinColor}StackedActionIcon.png")]
public sealed class Move : Action
{
	[SerializeField]
	private SharedVector2 _direction;

	[SerializeField]
	private SharedCharacter _actor;

	public override TaskStatus OnUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Vector2 value = ((SharedVariable<Vector2>)_direction).Value;
		Character value2 = ((SharedVariable<Character>)_actor).Value;
		if (((SharedVariable<Vector2>)_direction).Value != value)
		{
			((SharedVariable)_direction).SetValue((object)value);
		}
		value2.movement.Move(value);
		return (TaskStatus)2;
	}
}
