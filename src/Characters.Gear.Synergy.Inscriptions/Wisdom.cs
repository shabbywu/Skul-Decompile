using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Wisdom : SimpleStatBonusKeyword
{
	[Serializable]
	public sealed class Buffs : CooldownAbility
	{
		public sealed class Instance : CooldownAbilityInstance
		{
			private Buffs _buffs;

			public Instance(Character owner, Buffs ability)
				: base(owner, ability)
			{
				_buffs = ability;
			}

			protected override void OnAttach()
			{
				((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)HandleOnGiveDamage);
				ChangeIconFillToBuffTime();
			}

			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (damage.attribute != Damage.Attribute.Magic)
				{
					return false;
				}
				if (_remainBuffTime > 0f)
				{
					return false;
				}
				if (_remainCooldownTime > 0f)
				{
					return false;
				}
				owner.stat.AttachValues(_buffs._statValues);
				ability._onAttached.Run(owner);
				OnAttachBuff();
				return false;
			}

			protected override void OnDetach()
			{
				owner.stat.DetachValues(_buffs._statValues);
				((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)HandleOnGiveDamage);
				OnDetachBuff();
			}

			protected override void OnDetachBuff()
			{
				base.OnDetachBuff();
				owner.stat.DetachValues(_buffs._statValues);
			}
		}

		[Header("4세트 효과")]
		[SerializeField]
		private Stat.Values _statValues;

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

	protected override Stat.Category statCategory => Stat.Category.Percent;

	protected override Stat.Kind statKind => Stat.Kind.MagicAttackDamage;

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
