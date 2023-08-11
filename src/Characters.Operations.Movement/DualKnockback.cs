using UnityEngine;

namespace Characters.Operations.Movement;

public class DualKnockback : TargetedCharacterOperation
{
	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private PushForce _pushForce1;

	[SerializeField]
	private Curve _curve1;

	[SerializeField]
	private PushForce _pushForce2;

	[SerializeField]
	private Curve _curve2;

	public override void Run(Character owner, Character target)
	{
	}
}
