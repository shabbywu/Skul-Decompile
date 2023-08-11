using System;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class FenrirFang : Ability
{
	public class Instance : AbilityInstance<FenrirFang>
	{
		public Instance(Character owner, FenrirFang ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.giveStoppingPowerOnPoison = true;
		}

		protected override void OnDetach()
		{
			owner.status.giveStoppingPowerOnPoison = false;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
