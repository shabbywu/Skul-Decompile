using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AirAndGroundAttackDamage : Ability
{
	public class Instance : AbilityInstance<AirAndGroundAttackDamage>
	{
		internal Instance(Character owner, AirAndGroundAttackDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(0, (GiveDamageDelegate)OnGiveDamage);
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if ((Object)(object)target.character?.health == (Object)null)
			{
				return false;
			}
			damage.percentMultiplier *= (owner.movement.isGrounded ? ability.groundPercent : ability.airPercent);
			damage.multiplier += (owner.movement.isGrounded ? ability.groundPercentPoint : ability.airPercentPoint);
			return false;
		}
	}

	[SerializeField]
	private float _groundPercent = 1f;

	[SerializeField]
	private float _airPercent = 1f;

	[SerializeField]
	private float _groundPercentPoint;

	[SerializeField]
	private float _airPercentPoint;

	public float groundPercent => _groundPercent;

	public float airPercent => _airPercent;

	public float groundPercentPoint => _groundPercentPoint;

	public float airPercentPoint => _airPercentPoint;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
