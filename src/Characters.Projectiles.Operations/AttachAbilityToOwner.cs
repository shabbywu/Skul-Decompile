using Characters.Abilities;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public sealed class AttachAbilityToOwner : Operation
{
	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	private void Awake()
	{
		_abilityComponent.Initialize();
	}

	public override void Run(IProjectile projectile)
	{
		Character owner = projectile.owner;
		if ((Object)(object)owner != (Object)null)
		{
			owner.ability.Add(_abilityComponent.ability);
		}
	}
}
