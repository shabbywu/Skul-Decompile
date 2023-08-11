using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public class GreatMountainForce : Ability
{
	public class Instance : AbilityInstance<GreatMountainForce>
	{
		private Stat.Values _stat;

		private int _stack;

		private float _remainUpdateTime;

		public Instance(Character owner, GreatMountainForce ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			_remainUpdateTime = ability._updateInterval;
			owner.stat.AttachValues(_stat);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			_stack = 0;
			_remainUpdateTime = ability._updateInterval;
			UpdateStack();
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainUpdateTime -= deltaTime;
			if (_remainUpdateTime < 0f)
			{
				_stack = Mathf.Min(_stack + 1, ability._maxStack);
				UpdateStack();
			}
		}

		public void UpdateStack()
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)ability._statPerStack).values[i].GetStackedValue(_stack);
			}
			owner.stat.SetNeedUpdate();
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}
	}

	[SerializeField]
	private float _updateInterval;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private Stat.Values _statPerStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
