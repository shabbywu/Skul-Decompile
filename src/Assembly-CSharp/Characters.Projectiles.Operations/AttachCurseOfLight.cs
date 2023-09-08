using Characters.Abilities;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public sealed class AttachCurseOfLight : CharacterHitOperation
{
	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
	{
		if (target.playerComponents != null)
		{
			target.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.Curse);
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
