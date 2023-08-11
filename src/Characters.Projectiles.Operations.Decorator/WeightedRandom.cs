using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public class WeightedRandom : Operation
{
	[SerializeField]
	[Subcomponent(typeof(OperationWithWeight))]
	private OperationWithWeight.Subcomponents _toRandom;

	public override void Run(IProjectile projectile)
	{
		_toRandom.RunWeightedRandom(projectile);
	}
}
