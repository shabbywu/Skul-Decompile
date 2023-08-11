using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyDamageByTargetLayer : Ability
{
	public class Instance : AbilityInstance<ModifyDamageByTargetLayer>
	{
		internal Instance(Character owner, ModifyDamageByTargetLayer ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(0, (GiveDamageDelegate)OnOwnerGiveDamage);
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnOwnerGiveDamage);
		}

		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (ExtensionMethods.Contains(ability._targetLayer.Evaluate(((Component)owner).gameObject), ((Component)target.character).gameObject.layer))
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
