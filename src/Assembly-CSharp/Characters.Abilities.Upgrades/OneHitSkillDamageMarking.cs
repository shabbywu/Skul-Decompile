using System;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class OneHitSkillDamageMarking : Ability
{
	public sealed class Instance : AbilityInstance<OneHitSkillDamageMarking>
	{
		public Instance(Character owner, OneHitSkillDamageMarking ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
		}

		protected override void OnDetach()
		{
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
