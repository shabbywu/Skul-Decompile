using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Weapons.Minotaurus;

[Serializable]
public sealed class MinotaurusPassive : Ability, IAbilityInstance
{
	[SerializeField]
	private string _passiveAttackKey;

	[SerializeField]
	private ValueGauge _gaugeWithValue;

	private bool _attacked;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon
	{
		get
		{
			if (!(_gaugeWithValue.currentValue > 0f) || !(remainTime > 0f))
			{
				return null;
			}
			return _defaultIcon;
		}
	}

	public float iconFillAmount => 1f - remainTime / base.duration;

	public int iconStacks => 0;

	public bool expired => false;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	public void Attach()
	{
		remainTime = base.duration;
		_attacked = false;
		ResetStack();
	}

	public void Detach()
	{
		remainTime = 0f;
		_attacked = false;
		ResetStack();
	}

	public void Refresh()
	{
	}

	public void UpdateTime(float deltaTime)
	{
		if (_gaugeWithValue.currentValue > 0f)
		{
			remainTime -= deltaTime;
			if (remainTime <= 0f)
			{
				ResetStack();
			}
		}
	}

	private void AddStack()
	{
		_gaugeWithValue.Add(1f);
		remainTime = base.duration;
	}

	public void StartRecordingAttacks()
	{
		_attacked = false;
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		Character character2 = owner;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
	}

	public void StopRecodingAttacks()
	{
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		if (!_attacked)
		{
			ResetStack();
		}
		else
		{
			AddStack();
		}
	}

	private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		if (!(gaveDamage.key != _passiveAttackKey))
		{
			_attacked = true;
		}
	}

	private void ResetStack()
	{
		if (!(_gaugeWithValue.currentValue >= _gaugeWithValue.maxValue))
		{
			_gaugeWithValue.Clear();
		}
	}
}
