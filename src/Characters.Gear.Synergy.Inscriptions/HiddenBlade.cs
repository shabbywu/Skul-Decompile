using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class HiddenBlade : InscriptionInstance
{
	[Serializable]
	public sealed class HiddenBladeAbility2 : Ability
	{
		public sealed class Instance : AbilityInstance<HiddenBladeAbility2>
		{
			public Instance(Character owner, HiddenBladeAbility2 ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
				if (((EnumArray<Character.Type, bool>)ability._bossCharacterType)[owner.type])
				{
					base.remainTime = ability._durationForBoss;
				}
				else
				{
					base.remainTime = ability._originDuration;
				}
				owner.stat.AttachValues(ability._debuffStat);
				owner.stat.Update();
			}

			protected override void OnDetach()
			{
				owner.stat.DetachValues(ability._debuffStat);
			}
		}

		[SerializeField]
		private CharacterTypeBoolArray _bossCharacterType;

		[SerializeField]
		private Stat.Values _debuffStat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.0));

		[SerializeField]
		private float _originDuration;

		[SerializeField]
		private float _durationForBoss;

		[SerializeField]
		private bool _alwaysCritical;

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[SerializeField]
	[Header("2세트 효과")]
	private CharacterTypeBoolArray _targetTypes;

	[SerializeField]
	private HiddenBladeAbility2 _debuff;

	[Header("4세트 효과")]
	[SerializeField]
	private HiddenBladeAbility2 _enhancedDebuff;

	private Nothing _attacked;

	protected override void Initialize()
	{
		_attacked = new Nothing
		{
			duration = 2.1474836E+09f
		};
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)AttachMark);
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)EvaluateCritical);
	}

	private bool AttachMark(ITarget target, ref Damage damage)
	{
		if (keyword.step < 1)
		{
			return false;
		}
		if ((Object)(object)target.character == (Object)null)
		{
			return false;
		}
		if (!((EnumArray<Character.Type, bool>)_targetTypes)[target.character.type])
		{
			return false;
		}
		if (target.character.ability.Contains(_attacked))
		{
			return false;
		}
		target.character.ability.Add(_attacked);
		if (keyword.isMaxStep)
		{
			target.character.ability.Add(_enhancedDebuff);
		}
		else
		{
			target.character.ability.Add(_debuff);
		}
		return false;
	}

	private bool EvaluateCritical(ITarget target, ref Damage damage)
	{
		if (!keyword.isMaxStep)
		{
			return false;
		}
		if ((Object)(object)target.character == (Object)null)
		{
			return false;
		}
		if (!((EnumArray<Character.Type, bool>)_targetTypes)[target.character.type])
		{
			return false;
		}
		if (!target.character.ability.Contains(_enhancedDebuff))
		{
			return false;
		}
		damage.SetGuaranteedCritical(0, critical: true);
		return false;
	}

	public override void Detach()
	{
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)AttachMark);
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)EvaluateCritical);
	}
}
