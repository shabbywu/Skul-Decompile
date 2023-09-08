using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Misfortune : SimpleStatBonusKeyword
{
	[Serializable]
	public sealed class Buffs : CooldownAbility
	{
		public sealed class Instance : CooldownAbilityInstance
		{
			public Instance(Character owner, Buffs ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
				ChangeIconFillToBuffTime();
			}

			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (gaveDamage.critical && !_buffAttached && !(_remainCooldownTime > 0f))
				{
					OnAttachBuff();
				}
			}

			protected override void OnDetach()
			{
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
				owner.onGiveDamage.Remove(OnGiveDamage);
				OnDetachBuff();
			}

			protected override void OnAttachBuff()
			{
				base.OnAttachBuff();
				owner.onGiveDamage.Add(int.MinValue, OnGiveDamage);
			}

			protected override void OnDetachBuff()
			{
				base.OnDetachBuff();
				owner.onGiveDamage.Remove(OnGiveDamage);
			}

			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				damage.criticalChance = 1.0;
				damage.Evaluate(immuneToCritical: false);
				return false;
			}
		}

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[Header("2세트 효과")]
	[SerializeField]
	private double[] _statBonusByStep;

	[Header("4세트 효과")]
	[SerializeField]
	private Buffs _buff;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.CriticalChance;

	protected override void Initialize()
	{
		base.Initialize();
		_buff.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.isMaxStep)
		{
			if (!base.character.ability.Contains(_buff))
			{
				base.character.ability.Add(_buff);
			}
		}
		else
		{
			base.character.ability.Remove(_buff);
		}
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_buff);
	}
}
