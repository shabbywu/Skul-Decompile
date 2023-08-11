using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class StatBonusByBalance : Ability
{
	public sealed class Instance : AbilityInstance<StatBonusByBalance>
	{
		private int _stacks;

		private Stat.Values _stat;

		public Instance(Character owner, StatBonusByBalance ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			GameData.Currency.currencies[ability._type].onEarn += UpdateStackAndStat;
			GameData.Currency.currencies[ability._type].onConsume += UpdateStackAndStat;
			_stat = ability._statPerStack.Clone();
			owner.stat.AttachValues(_stat);
			UpdateStackAndStat(0);
		}

		protected override void OnDetach()
		{
			DetachEvent();
			owner.stat.DetachValues(_stat);
		}

		private void DetachEvent()
		{
			GameData.Currency.currencies[ability._type].onEarn -= UpdateStackAndStat;
			GameData.Currency.currencies[ability._type].onConsume -= UpdateStackAndStat;
		}

		private void UpdateStackAndStat(int amount)
		{
			int num = (_stacks = GameData.Currency.currencies[ability._type].balance / ability._balancePerStack);
			if (ability._maxStack > 0)
			{
				_stacks = Mathf.Min(num, ability._maxStack);
			}
			UpdateStat();
		}

		private void UpdateStat()
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)ability._statPerStack).values[i].GetStackedValue(_stacks);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private GameData.Currency.Type _type;

	[SerializeField]
	private Stat.Values _statPerStack;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private int _balancePerStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
