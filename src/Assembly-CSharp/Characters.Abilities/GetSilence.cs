using System;

namespace Characters.Abilities;

[Serializable]
public class GetSilence : Ability
{
	public class Instance : AbilityInstance<GetSilence>
	{
		public Instance(Character owner, GetSilence ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.silence.Attach(this);
		}

		protected override void OnDetach()
		{
			owner.silence.Detach(this);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
