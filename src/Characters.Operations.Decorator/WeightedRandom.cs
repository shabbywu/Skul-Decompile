using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Decorator;

public class WeightedRandom : CharacterOperation
{
	[Subcomponent(typeof(OperationWithWeight))]
	[SerializeField]
	private OperationWithWeight.Subcomponents _toRandom;

	public override void Initialize()
	{
		_toRandom.Initialize();
	}

	public override void Run(Character owner)
	{
		_toRandom.RunWeightedRandom(owner);
	}

	public override void Stop()
	{
		_toRandom.StopAll();
	}
}
