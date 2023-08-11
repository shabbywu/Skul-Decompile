using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class OneHitSkillDamage : Ability
{
	public sealed class Instance : AbilityInstance<OneHitSkillDamage>
	{
		public Instance(Character owner, OneHitSkillDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)OnGiveDamageDelegate);
		}

		private bool OnGiveDamageDelegate(ITarget target, ref Damage damage)
		{
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			if (damage.motionType != Damage.MotionType.Skill)
			{
				return false;
			}
			if (damage.amount < 1.0)
			{
				return false;
			}
			Character character = target.character;
			if ((Object)(object)character == (Object)null)
			{
				return false;
			}
			if (character.ability.Contains(ability._marking.ability))
			{
				return false;
			}
			damage.percentMultiplier *= ability._damagePercent;
			character.ability.Add(ability._marking.ability);
			_ = (Object)(object)PersistentSingleton<SoundManager>.Instance.PlaySound(ability._effectAudioClipInfo, ((Component)owner).transform.position) == (Object)null;
			return false;
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamageDelegate);
		}
	}

	[SerializeField]
	private SoundInfo _effectAudioClipInfo;

	[SerializeField]
	private float _damagePercent;

	[SerializeField]
	private OneHitSkillDamageMarkingComponent _marking;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
