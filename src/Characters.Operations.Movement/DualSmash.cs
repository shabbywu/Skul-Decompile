using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement;

public class DualSmash : TargetedCharacterOperation
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

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _onCollide;

	public override void Run(Character owner, Character target)
	{
	}
}
