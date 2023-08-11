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
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)OnGiveDamage);
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
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
