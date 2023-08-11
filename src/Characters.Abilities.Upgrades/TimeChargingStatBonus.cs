using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class TimeChargingStatBonus : Ability
{
	public class Instance : AbilityInstance<TimeChargingStatBonus>
	{
		private Stat.Values _stat;

		private bool _charging;

		private float _remainChargingTime;

		private float _remainDetachTime;

		public override int iconStacks => ability._stack;

		public override float iconFillAmount
		{
			get
			{
				if (!(_remainDetachTime > 0f) || !(_remainDetachTime < ability._detachTime - ability._resetTime))
				{
					return base.iconFillAmount;
				}
				return 0f;
			}
		}

		public Instance(Character owner, TimeChargingStatBonus ability)
			: base(owner, ability)
		{
			_stat = ability._statPerStack.Clone();
		}

		protected override void OnAttach()
		{
			owner.stat.AttachValues(_stat);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			ability._stack = 0;
			_charging = true;
			UpdateStack();
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (_charging && ((EnumArray<Damage.MotionType, bool>)ability._motionFilter)[gaveDamage.motionType])
			{
				_charging = false;
				_remainDetachTime = ability._detachTime;
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (_charging)
			{
				_remainChargingTime -= deltaTime;
				if (_remainChargingTime <= 0f)
				{
					ability._stack = Mathf.Min(ability._maxStack, ability._stack + 1);
					_remainChargingTime = ability._chargingTime;
					UpdateStack();
				}
				return;
			}
			_remainDetachTime -= deltaTime;
			if (_remainDetachTime <= ability._detachTime - ability._resetTime && ability._stack > 0)
			{
				ability._stack = 0;
				UpdateStack();
			}
			if (_remainDetachTime <= 0f)
			{
				_charging = true;
			}
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		public void UpdateStack()
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)ability._statPerStack).values[i].GetStackedValue(ability._stack);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	private int _stack;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private float _chargingTime;

	[SerializeField]
	private float _resetTime;

	[SerializeField]
	private float _detachTime;

	[SerializeField]
	private MotionTypeBoolArray _motionFilter;

	[SerializeField]
	private Stat.Values _statPerStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
