using System;

namespace Characters.Abilities.Debuffs;

[Serializable]
public sealed class BlockCritical : Ability
{
	public sealed class Instance : AbilityInstance<BlockCritical>
	{
		public Instance(Character owner, BlockCritical ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(int.MinValue, HandleOnGiveDamage);
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			damage.SetGuaranteedCritical(int.MaxValue, critical: false);
			return false;
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
