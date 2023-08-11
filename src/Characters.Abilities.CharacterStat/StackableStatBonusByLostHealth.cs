using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class StackableStatBonusByLostHealth : Ability
{
	public sealed class Instance : AbilityInstance<StackableStatBonusByLostHealth>
	{
		private Stat.Values _stat;

		private int _stack;

		public override int iconStacks => _stack;

		public override float iconFillAmount => base.remainTime / ability.duration;

		public Instance(Character owner, StackableStatBonusByLostHealth ability)
			: base(owner, ability)
		{
			_stat = ability._targetPerStack.Clone();
		}

		protected override void OnAttach()
		{
			owner.stat.AttachValues(_stat);
			owner.health.onChanged += UpdateStat;
			UpdateStat();
		}

		protected override void OnDetach()
		{
			owner.health.onChanged -= UpdateStat;
			owner.stat.DetachValues(_stat);
		}

		public void UpdateStat()
		{
			int num = (int)((owner.health.maximumHealth - owner.health.currentHealth) / (double)ability._healthStackUnit);
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
				values[i].value = ((ReorderableArray<Stat.Value>)ability._targetPerStack).values[i].GetStackedValue(stack);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _targetPerStack;

	[SerializeField]
	[Range(0f, 1000f)]
	private int _healthStackUnit;

	[SerializeField]
	private int _maxStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
