using Characters.Abilities;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class AttachAbility : CharacterHitOperation
{
	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	private void Awake()
	{
		_abilityComponent.Initialize();
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
	{
		character.ability.Add(_abilityComponent.ability);
	}
}
