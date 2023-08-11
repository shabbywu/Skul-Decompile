using System;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class OrdoxSwamp : Ability
{
	public class Instance : AbilityInstance<OrdoxSwamp>
	{
		public Instance(Character owner, OrdoxSwamp ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.onApplyPoison += Status_onApplyPoison;
		}

		private void Status_onApplyPoison(Character attacker, Character target)
		{
			_ = target.status.poisoned;
		}

		protected override void OnDetach()
		{
			owner.status.onApplyPoison -= Status_onApplyPoison;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
