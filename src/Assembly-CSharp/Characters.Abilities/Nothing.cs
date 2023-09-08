using System;

namespace Characters.Abilities;

[Serializable]
public class Nothing : Ability
{
	public class Instance : AbilityInstance<Nothing>
	{
		public Instance(Character owner, Nothing ability)
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
