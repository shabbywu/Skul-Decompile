using Characters.Operations;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class RunCharacterOperation : Operation
{
	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation _operation;

	public override void Run(IProjectile projectile)
	{
		_operation.Initialize();
		_operation.Run(projectile.owner);
	}
}
