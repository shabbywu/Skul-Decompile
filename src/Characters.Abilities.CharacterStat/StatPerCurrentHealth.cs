using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class StatPerCurrentHealth : Ability
{
	public sealed class Instance : AbilityInstance<StatPerCurrentHealth>
	{
		private const float _checkInterval = 0.15f;

		private Stat.Values _stat;

		private int _stack;

		private double _healthCache;

		private float _remainCheckTime;

		public override int iconStacks => _stack;

		public Instance(Character owner, StatPerCurrentHealth ability)
			: base(owner, ability)
		{
			_stat = ability._statPerCurrentHealth.Clone();
		}

		protected override void OnAttach()
		{
			UpdateStat();
			owner.stat.AttachValues(_stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCheckTime -= deltaTime;
			if (_remainCheckTime < 0f)
			{
				_remainCheckTime += 0.15f;
				TryUpdateStat();
			}
		}

		private void TryUpdateStat()
		{
			if (!(Mathf.Abs((float)(owner.health.currentHealth - _healthCache)) < float.Epsilon))
			{
				UpdateStat();
			}
		}

		private void UpdateStat()
		{
			_stack = Mathf.Min(ability._maxStack, Mathf.FloorToInt((float)owner.health.currentHealth / (float)ability._healthPerStack));
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				double num = (double)_stack * ((ReorderableArray<Stat.Value>)ability._statPerCurrentHealth).values[i].value;
				if (((ReorderableArray<Stat.Value>)ability._statPerCurrentHealth).values[i].categoryIndex == Stat.Category.Percent.index)
				{
					num += 1.0;
				}
				((ReorderableArray<Stat.Value>)_stat).values[i].value = num;
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _statPerCurrentHealth;

	[SerializeField]
	private int _healthPerStack;

	[SerializeField]
	private int _maxStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
