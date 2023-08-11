using System;

namespace Characters.Abilities;

[Serializable]
public class GetInvulnerable : Ability
{
	public class Instance : AbilityInstance<GetInvulnerable>
	{
		public Instance(Character owner, GetInvulnerable ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.invulnerable.Attach((object)this);
		}

		protected override void OnDetach()
		{
			owner.invulnerable.Detach((object)this);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
