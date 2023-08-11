using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class EqualMovementType : ICondition
{
	[SerializeField]
	private Characters.Movements.Movement.Config.Type _movementType;

	public bool Satisfied(Character character)
	{
		return character.movement.config.type == _movementType;
	}
}
