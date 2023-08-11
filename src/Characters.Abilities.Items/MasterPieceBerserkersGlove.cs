using System;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public class MasterPieceBerserkersGlove : Ability
{
	public class Instance : AbilityInstance<MasterPieceBerserkersGlove>
	{
		private Stat.Values _stat;

		private double _statValue;

		private int _bonusStack;

		public override int iconStacks => (int)(_statValue * 100.0) + _bonusStack;

		public Instance(Character owner, MasterPieceBerserkersGlove ability)
			: base(owner, ability)
		{
			_stat = ability._statPerLostHealth.Clone();
		}

		protected override void OnAttach()
		{
			_bonusStack = 0;
			owner.health.onTookDamage += UpdateStat;
			owner.health.onHealed += UpdateStat;
			owner.stat.AttachValues(_stat);
			UpdateStat();
		}

		protected override void OnDetach()
		{
			owner.health.onTookDamage -= UpdateStat;
			owner.health.onHealed -= UpdateStat;
			owner.stat.DetachValues(_stat);
			owner.stat.DetachValues(ability._bonus);
		}

		private void UpdateStat(double healed, double overHealed)
		{
			UpdateStat();
		}

		private void UpdateStat(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			UpdateStat();
		}

		private void UpdateStat()
		{
			double num = Math.Min(1.0 - owner.health.percent, ability._maxLostPercent) * 100.0;
			_statValue = num * ((ReorderableArray<Stat.Value>)ability._statPerLostHealth).values[0].value;
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				double num2 = num * ((ReorderableArray<Stat.Value>)ability._statPerLostHealth).values[i].value;
				if (((ReorderableArray<Stat.Value>)ability._statPerLostHealth).values[i].categoryIndex == Stat.Category.Percent.index)
				{
					num2 += 1.0;
				}
				((ReorderableArray<Stat.Value>)_stat).values[i].value = num2;
			}
			if (owner.health.percent <= (double)ability._bonusHealthPercent)
			{
				if (_bonusStack <= 0)
				{
					_bonusStack = (int)(((ReorderableArray<Stat.Value>)ability._bonus).values[0].value * 100.0);
					owner.stat.AttachValues(ability._bonus);
				}
			}
			else
			{
				_bonusStack = 0;
				owner.stat.DetachValues(ability._bonus);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _statPerLostHealth;

	[Range(0f, 1f)]
	[SerializeField]
	private float _maxLostPercent;

	[Range(0f, 1f)]
	[Header("Bonus Stat")]
	[SerializeField]
	private float _bonusHealthPercent;

	[SerializeField]
	private Stat.Values _bonus;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
