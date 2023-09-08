using System;

namespace Characters.Abilities;

[Serializable]
public class GetLockout : Ability
{
	public class Instance : AbilityInstance<GetLockout>
	{
		public Instance(Character owner, GetLockout ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.unstoppable.Attach(this);
		}

		protected override void OnDetach()
		{
			owner.status.unstoppable.Detach(this);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
