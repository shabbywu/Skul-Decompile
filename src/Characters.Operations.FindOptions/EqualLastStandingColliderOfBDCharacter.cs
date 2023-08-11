using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class EqualLastStandingColliderOfBDCharacter : ICondition
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _targetName = "Target";

	public bool Satisfied(Character character)
	{
		Character value = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_targetName)).Value;
		if ((Object)(object)value == (Object)null)
		{
			return false;
		}
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		Collider2D lastStandingCollider2 = value.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null || (Object)(object)lastStandingCollider2 == (Object)null)
		{
			return false;
		}
		return (Object)(object)lastStandingCollider == (Object)(object)lastStandingCollider2;
	}
}
