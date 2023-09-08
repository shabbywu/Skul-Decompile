using System;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class GiantsCavalry : Ability
{
	public class Instance : AbilityInstance<GiantsCavalry>
	{
		public Instance(Character owner, GiantsCavalry ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(int.MinValue, OnGiveDamage);
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (damage.motionType == Damage.MotionType.Dash)
			{
				damage.stoppingPower *= 2f;
			}
			return false;
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(OnGiveDamage);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
