using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class Shield : Ability
{
	public class Instance : AbilityInstance<Shield>
	{
		private Characters.Shield.Instance _shieldInstance;

		public Instance(Character owner, Shield ability)
			: base(owner, ability)
		{
		}

		public override void Refresh()
		{
			base.Refresh();
			if (_shieldInstance != null)
			{
				_shieldInstance.amount = ability._amount;
			}
		}

		private void OnShieldBroke()
		{
			ability.onBroke?.Invoke(_shieldInstance);
			owner.ability.Remove(this);
		}

		protected override void OnAttach()
		{
			if (ability._healthType == HealthType.Constant)
			{
				_shieldInstance = owner.health.shield.Add(ability, ability._amount, OnShieldBroke);
			}
			else
			{
				_shieldInstance = owner.health.shield.Add(ability, (float)(owner.health.maximumHealth * (double)ability._amount * 0.009999999776482582), OnShieldBroke);
			}
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

	public enum HealthType
	{
		Constant,
		Percent
	}

	[SerializeField]
	private float _amount;

	[SerializeField]
	private HealthType _healthType;

	public float amount
	{
		get
		{
			return _amount;
		}
		set
		{
			_amount = value;
		}
	}

	public event Action<Characters.Shield.Instance> onBroke;

	public event Action<Characters.Shield.Instance> onDetach;

	public Shield()
	{
	}

	public Shield(float amount)
	{
		_amount = amount;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
