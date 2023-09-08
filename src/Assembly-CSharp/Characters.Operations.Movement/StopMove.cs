using UnityEngine;

namespace Characters.Operations.Movement;

public class StopMove : CharacterOperation
{
	public override void Run(Character owner)
	{
		((MonoBehaviour)owner.movement).StopAllCoroutines();
	}
}
