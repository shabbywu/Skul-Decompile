using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyDamageByOwnerAndTargetHealth : Ability
{
	public class Instance : AbilityInstance<ModifyDamageByOwnerAndTargetHealth>
	{
		internal Instance(Character owner, ModifyDamageByOwnerAndTargetHealth ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(0, OnOwnerGiveDamage);
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(OnOwnerGiveDamage);
		}

		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			Character character = target.character;
			if ((Object)(object)character == (Object)null)
			{
				return false;
			}
			if (ability._ownerHealthGreaterThanTargerHealth)
			{
				if (owner.health.percent < character.health.percent)
				{
					return false;
				}
			}
			else if (owner.health.percent > character.health.percent)
			{
				return false;
			}
			if (ability._targetLayer.Evaluate(((Component)owner).gameObject).Contains(((Component)target.character).gameObject.layer))
			{
				damage.percentMultiplier *= ability._damagePercent;
				damage.multiplier += ability._damagePercentPoint;
				damage.criticalChance += ability._extraCriticalChance;
				damage.criticalDamageMultiplier += ability._extraCriticalDamageMultiplier;
			}
			return false;
		}
	}

	[SerializeField]
	private bool _ownerHealthGreaterThanTargerHealth;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private float _damagePercent = 1f;

	[SerializeField]
	private float _damagePercentPoint;

	[SerializeField]
	private float _extraCriticalChance;

	[SerializeField]
	private float _extraCriticalDamageMultiplier;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
