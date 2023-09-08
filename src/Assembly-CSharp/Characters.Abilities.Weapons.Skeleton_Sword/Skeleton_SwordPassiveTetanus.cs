using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Skeleton_Sword;

[Serializable]
public class Skeleton_SwordPassiveTetanus : Ability
{
	public class Instance : AbilityInstance<Skeleton_SwordPassiveTetanus>
	{
		public Instance(Character owner, Skeleton_SwordPassiveTetanus ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.onApplyBleed += OnApplyBleed;
			ability._damageComponent.baseAbility.attacker = owner;
		}

		protected override void OnDetach()
		{
			owner.status.onApplyBleed -= OnApplyBleed;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
		}

		private void OnApplyBleed(Character attacker, Character target)
		{
			if (!target.health.dead)
			{
				target.ability.Add(ability._damageComponent.ability);
			}
		}
	}

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private Skeleton_SwordTetanusDamageComponent _damageComponent;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
