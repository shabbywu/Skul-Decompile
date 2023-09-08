using System;

namespace Characters.Abilities;

[Serializable]
public sealed class GetEvasion : Ability
{
	public sealed class Instance : AbilityInstance<GetEvasion>
	{
		public Instance(Character owner, GetEvasion ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.evasion.Attach(this);
		}

		protected override void OnDetach()
		{
			owner.evasion.Detach(this);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
