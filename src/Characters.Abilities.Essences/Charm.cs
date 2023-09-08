using System;
using UnityEngine;

namespace Characters.Abilities.Essences;

[Serializable]
public sealed class Charm : Ability
{
	public class Instance : AbilityInstance<Charm>
	{
		public Instance(Character owner, Charm ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(int.MaxValue, OnGiveDamage);
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(OnGiveDamage);
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if ((Object)(object)target.character != (Object)(object)ability._attacker)
			{
				return false;
			}
			return true;
		}
	}

	private Character _attacker;

	public void SetAttacker(Character attacker)
	{
		_attacker = attacker;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
