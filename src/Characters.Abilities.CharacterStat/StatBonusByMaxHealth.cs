using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatBonusByMaxHealth : Ability
{
	public class Instance : AbilityInstance<StatBonusByMaxHealth>
	{
		private const float _updateInterval = 0.2f;

		private float _remainUpdateTime;

		private Stat.Values _stat;

		private int _stack;

		public override int iconStacks => _stack;

		public override float iconFillAmount => 0f;

		public Instance(Character owner, StatBonusByMaxHealth ability)
			: base(owner, ability)
		{
			_stat = ability._targetPerStack.Clone();
		}

		protected override void OnAttach()
		{
			owner.stat.AttachValues(_stat);
			UpdateStat();
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainUpdateTime -= deltaTime;
			if (_remainUpdateTime < 0f)
			{
				_remainUpdateTime = 0.2f;
				UpdateStat();
			}
		}

		public void UpdateStat()
		{
			int num = (int)(owner.health.maximumHealth / (double)ability._stackHealth);
			if (num != _stack)
			{
				_stack = Mathf.Min(ability._maxStack, num);
				SetStat(_stack);
			}
		}

		private void SetStat(int stack)
		{
			Stat.Value[] values = ((ReorderableArray<Stat.Value>)_stat).values;
			for (int i = 0; i < values.Length; i++)
			{
				values[i].value = ((ReorderableArray<Stat.Value>)ability._targetPerStack).values[i].value * (double)stack;
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _targetPerStack;

	[Range(0f, 1000f)]
	[SerializeField]
	private int _stackHealth;

	[SerializeField]
	private int _maxStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
