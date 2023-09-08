using System;
using Characters.Movements;

namespace Characters.Operations.FindOptions;

[Serializable]
public class WalkingOnGround : ICondition
{
	public bool Satisfied(Character character)
	{
		return character.movement.config.type == Characters.Movements.Movement.Config.Type.Walking;
	}
}
