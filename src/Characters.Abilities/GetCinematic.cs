using System;

namespace Characters.Abilities;

[Serializable]
public class GetCinematic : Ability
{
	public class Instance : AbilityInstance<GetCinematic>
	{
		public Instance(Character owner, GetCinematic ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.cinematic.Attach((object)this);
		}

		protected override void OnDetach()
		{
			owner.cinematic.Detach((object)this);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
