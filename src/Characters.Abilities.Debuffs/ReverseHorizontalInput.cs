using System;
using Characters.Controllers;

namespace Characters.Abilities.Debuffs;

[Serializable]
public sealed class ReverseHorizontalInput : Ability
{
	public sealed class Instance : AbilityInstance<ReverseHorizontalInput>
	{
		public Instance(Character owner, ReverseHorizontalInput ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			PlayerInput.reverseHorizontal.Attach((object)this);
		}

		protected override void OnDetach()
		{
			PlayerInput.reverseHorizontal.Detach((object)this);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
