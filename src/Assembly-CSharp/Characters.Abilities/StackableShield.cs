using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class StackableShield : Ability
{
	public class Instance : AbilityInstance<StackableShield>
	{
		internal Characters.Shield.Instance _shieldInstance;

		public Instance(Character owner, StackableShield ability)
			: base(owner, ability)
		{
		}

		public override void Refresh()
		{
			base.Refresh();
			AddShield(ability._amount);
		}

		public void AddShield(float amount)
		{
			if (_shieldInstance != null)
			{
				_shieldInstance.amount = Mathf.Min((float)ability._maxAmount, (float)_shieldInstance.amount + amount);
			}
			else
			{
				_shieldInstance = owner.health.shield.Add(ability, ability._amount, OnShieldBroke);
			}
		}

		private void OnShieldBroke()
		{
			ability.onBroke?.Invoke(_shieldInstance);
			owner.ability.Remove(this);
			_shieldInstance = null;
		}

		protected override void OnAttach()
		{
			_shieldInstance = owner.health.shield.Add(ability, ability._amount, OnShieldBroke);
		}

		protected override void OnDetach()
		{
			ability.onDetach?.Invoke(_shieldInstance);
			if (owner.health.shield.Remove(ability))
			{
				_shieldInstance = null;
			}
		}
	}

	[SerializeField]
	private int _maxAmount;

	[SerializeField]
	private float _amount;

	private Instance _instance;

	public float amount
	{
		get
		{
			if (_instance == null)
			{
				return 0f;
			}
			if (_instance._shieldInstance == null)
			{
				return 0f;
			}
			return (float)_instance._shieldInstance.amount;
		}
		set
		{
			if (_instance != null && _instance._shieldInstance != null)
			{
				_instance.AddShield(value);
			}
		}
	}

	public event Action<Characters.Shield.Instance> onBroke;

	public event Action<Characters.Shield.Instance> onDetach;

	public void Load(Character owner, int stack)
	{
		owner.ability.Add(this);
		amount = stack;
	}

	public StackableShield()
	{
	}

	public StackableShield(float amount)
	{
		_amount = amount;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_instance = new Instance(owner, this);
		return _instance;
	}
}
