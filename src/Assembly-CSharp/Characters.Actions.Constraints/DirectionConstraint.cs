using System;
using Characters.Controllers;
using UnityEngine;

namespace Characters.Actions.Constraints;

public class DirectionConstraint : Constraint
{
	[Flags]
	public enum Direction
	{
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8
	}

	public const float threshold = 0.66f;

	[SerializeField]
	[EnumFlag]
	protected Direction _direcion;

	public override bool Pass()
	{
		return Pass(_action.owner, _direcion);
	}

	public static bool Pass(Character character, Direction direction)
	{
		PlayerInput component = ((Component)character).GetComponent<PlayerInput>();
		if ((Object)(object)component == (Object)null)
		{
			return false;
		}
		Direction direction2 = (Direction)0;
		if (component.direction.y > 0.66f)
		{
			direction2 |= Direction.Up;
		}
		if (component.direction.y < -0.66f)
		{
			direction2 |= Direction.Down;
		}
		if (component.direction.x < -0.66f)
		{
			direction2 |= Direction.Left;
		}
		if (component.direction.x > 0.66f)
		{
			direction2 |= Direction.Right;
		}
		return (direction & direction2) == direction;
	}
}
