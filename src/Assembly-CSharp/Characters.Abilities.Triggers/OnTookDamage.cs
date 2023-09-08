using System;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnTookDamage : Trigger
{
	[SerializeField]
	private bool _onInvulnerable;

	[SerializeField]
	private double _minDamage = 1.0;

	[SerializeField]
	private double _minHealthChanged;

	[SerializeField]
	private bool _onCritical;

	[SerializeField]
	private bool _hasShield;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	private Character _character;

	private double _beforeHealth;

	public override void Attach(Character character)
	{
		_character = character;
		_character.health.onTakeDamage.Add(-2147483647, OnCharacterTakeDamage);
		_character.health.onTookDamage += OnCharacterTookDamage;
	}

	public override void Detach()
	{
		_character.health.onTakeDamage.Remove(OnCharacterTakeDamage);
		_character.health.onTookDamage -= OnCharacterTookDamage;
	}

	private void OnCharacterTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if ((_onCritical && !tookDamage.critical) || !_attackTypes[tookDamage.motionType] || !_damageTypes[tookDamage.attackType])
		{
			return;
		}
		if (_character.invulnerable.value || tookDamage.@null)
		{
			if (!_onInvulnerable)
			{
				return;
			}
		}
		else if (tookDamage.amount < _minDamage || (_hasShield && !_character.health.shield.hasAny) || _beforeHealth - _character.health.currentHealth < _minHealthChanged)
		{
			return;
		}
		Invoke();
	}

	private bool OnCharacterTakeDamage(ref Damage damage)
	{
		_beforeHealth = _character.health.currentHealth;
		return false;
	}
}
