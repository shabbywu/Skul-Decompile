using System.Collections.Generic;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public class Random : Operation
{
	[SerializeField]
	[Subcomponent]
	private Subcomponents _toRandom;

	public override void Run(IProjectile projectile)
	{
		ExtensionMethods.Random<Operation>((IEnumerable<Operation>)((SubcomponentArray<Operation>)_toRandom).components).Run(projectile);
	}
}
