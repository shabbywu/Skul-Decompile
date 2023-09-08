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
			owner.cinematic.Attach(this);
		}

		protected override void OnDetach()
		{
			owner.cinematic.Detach(this);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
