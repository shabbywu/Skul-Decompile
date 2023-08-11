using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable;

public sealed class PurchasedShield : IAbility, IAbilityInstance, ISavableAbility
{
	private Character _owner;

	private Characters.Shield.Instance _shieldInstance;

	private static float _currentValue;

	Character IAbilityInstance.owner => _owner;

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon
	{
		get
		{
			if (!(remainTime < 1f))
			{
				return Singleton<DarktechManager>.Instance.resource.bigCloverIcon;
			}
			return Singleton<DarktechManager>.Instance.resource.smallCloverIcon;
		}
	}

	public float iconFillAmount => 0f;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => 0;

	public bool expired => false;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public float stack
	{
		get
		{
			return _currentValue;
		}
		set
		{
			_currentValue = value;
		}
	}

	public event Action<Characters.Shield.Instance> onBroke;

	public event Action<Characters.Shield.Instance> onDetach;

	public IAbilityInstance CreateInstance(Character owner)
	{
		_owner = owner;
		return this;
	}

	public void Initialize()
	{
	}

	public void UpdateTime(float deltaTime)
	{
		_currentValue = (float)_shieldInstance.amount;
	}

	public void Refresh()
	{
	}

	void IAbilityInstance.Attach()
	{
		_shieldInstance = _owner.health.shield.Add(ability, _currentValue, OnShieldBroke);
	}

	void IAbilityInstance.Refresh()
	{
		if (_shieldInstance != null)
		{
			_shieldInstance.amount = _currentValue;
		}
	}

	void IAbilityInstance.Detach()
	{
		this.onDetach?.Invoke(_shieldInstance);
		if (_owner.health.shield.Remove(ability))
		{
			_shieldInstance = null;
		}
	}

	private void OnShieldBroke()
	{
		this.onBroke?.Invoke(_shieldInstance);
		_shieldInstance = null;
		_currentValue = 0f;
		_owner.ability.Remove(this);
	}
}
