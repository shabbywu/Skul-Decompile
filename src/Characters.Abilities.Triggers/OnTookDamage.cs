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
		_character.health.onTakeDamage.Add(-2147483647, (TakeDamageDelegate)OnCharacterTakeDamage);
		_character.health.onTookDamage += OnCharacterTookDamage;
	}

	public override void Detach()
	{
		_character.health.onTakeDamage.Remove((TakeDamageDelegate)OnCharacterTakeDamage);
		_character.health.onTookDamage -= OnCharacterTookDamage;
	}

	private void OnCharacterTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if ((_onCritical && !tookDamage.critical) || !((EnumArray<Damage.MotionType, bool>)_attackTypes)[tookDamage.motionType] || !((EnumArray<Damage.AttackType, bool>)_damageTypes)[tookDamage.attackType])
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
