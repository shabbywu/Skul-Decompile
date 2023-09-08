using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class OverrideHealthPercentDamage : Ability
{
	[Serializable]
	public struct HealthPercentByKey
	{
		[Range(0f, 1f)]
		[SerializeField]
		public float percent;

		public string key;
	}

	public class Instance : AbilityInstance<OverrideHealthPercentDamage>
	{
		internal Instance(Character owner, OverrideHealthPercentDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(int.MaxValue, HandleOnGiveDamage);
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if ((Object)(object)character == (Object)null)
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(damage.key))
			{
				return false;
			}
			HealthPercentByKey[] multiplierByKey = ability._multiplierByKey;
			for (int i = 0; i < multiplierByKey.Length; i++)
			{
				HealthPercentByKey healthPercentByKey = multiplierByKey[i];
				if (damage.key.Equals(healthPercentByKey.key, StringComparison.OrdinalIgnoreCase))
				{
					damage.@base = character.health.maximumHealth * (double)healthPercentByKey.percent;
					break;
				}
			}
			return false;
		}
	}

	[SerializeField]
	private HealthPercentByKey[] _multiplierByKey;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
