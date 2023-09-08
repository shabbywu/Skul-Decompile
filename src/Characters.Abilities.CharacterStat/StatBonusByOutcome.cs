using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class StatBonusByOutcome : Ability
{
	public class Instance : AbilityInstance<StatBonusByOutcome>
	{
		private int _remainGoldForStack;

		private int _remainDarkQuartzForStack;

		private Stat.Values _stat;

		public override int iconStacks => ability.stack;

		public Instance(Character owner, StatBonusByOutcome ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			GameData.Currency.gold.onConsume += OnGoldConsume;
			GameData.Currency.darkQuartz.onConsume += OnDarkQuartzConsume;
			_stat = ability._statPerStack.Clone();
			owner.stat.AttachValues(_stat);
			TryUpdateStat();
		}

		protected override void OnDetach()
		{
			GameData.Currency.gold.onEarn -= OnGoldConsume;
			GameData.Currency.darkQuartz.onEarn -= OnDarkQuartzConsume;
			owner.stat.DetachValues(_stat);
		}

		private void OnGoldConsume(int amount)
		{
			if (ability._goldPerStack != 0)
			{
				amount += _remainGoldForStack;
				AddStack(amount / ability._goldPerStack);
				_remainGoldForStack = amount % ability._goldPerStack;
			}
		}

		private void OnDarkQuartzConsume(int amount)
		{
			if (ability._darkQuartzPerStack != 0)
			{
				amount += _remainDarkQuartzForStack;
				AddStack(amount / ability._darkQuartzPerStack);
				_remainDarkQuartzForStack = amount % ability._darkQuartzPerStack;
			}
		}

		private void AddStack(int amount)
		{
			ability.stack += amount;
			TryUpdateStat();
		}

		public void TryUpdateStat()
		{
			if (ability._maxStack > 0 && ability.stack > ability._maxStack)
			{
				ability.stack = ability._maxStack;
				GameData.Currency.gold.onEarn -= OnGoldConsume;
				GameData.Currency.darkQuartz.onEarn -= OnDarkQuartzConsume;
				UpdateStat();
			}
			else
			{
				UpdateStat();
			}
		}

		private void UpdateStat()
		{
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = (double)ability.stack * ability._statPerStack.values[i].value;
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _statPerStack;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private int _goldPerStack;

	[SerializeField]
	private int _darkQuartzPerStack;

	private Instance _instance;

	private int _stack;

	public int stack
	{
		get
		{
			return _stack;
		}
		set
		{
			if (_instance != null)
			{
				_stack = value;
				_instance.TryUpdateStat();
			}
		}
	}

	public void Load(Character owner, int stack)
	{
		owner.ability.Add(this);
		this.stack = stack;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_instance = new Instance(owner, this);
		return _instance;
	}
}
