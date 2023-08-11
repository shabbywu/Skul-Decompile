using System;
using Characters.Abilities.Constraints;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnHealthValue : Trigger
{
	private enum CompareType
	{
		GreaterThanOrEqual,
		LessThan,
		Equal
	}

	private enum HealthType
	{
		Constant,
		Percent
	}

	[SerializeField]
	private CompareType _compareType;

	[SerializeField]
	private HealthType _healthType;

	[SerializeField]
	private int _amount;

	[FrameTime]
	[SerializeField]
	private float _checkInterval;

	[Constraint.Subcomponent]
	[SerializeField]
	private Constraint.Subcomponents _constraint;

	[SerializeField]
	private int _times;

	private int _remainTimes;

	private Character _character;

	private float _elapsed;

	public override void Attach(Character character)
	{
		if (_times == 0)
		{
			_times = int.MaxValue;
		}
		_remainTimes = _times;
		_character = character;
	}

	public override void Refresh()
	{
		base.Refresh();
		_remainCooldownTime = 0f;
		_remainTimes = _times;
	}

	public override void Detach()
	{
	}

	public override void UpdateTime(float deltaTime)
	{
		base.UpdateTime(deltaTime);
		_elapsed += deltaTime;
		if (_remainTimes <= 0 || _remainCooldownTime > 0f || !((SubcomponentArray<Constraint>)_constraint).components.Pass() || _elapsed < _checkInterval)
		{
			return;
		}
		_elapsed = 0f;
		if (CheckHealthCondition())
		{
			if (MMMaths.PercentChance(_possibility))
			{
				_remainTimes--;
				_onTriggered?.Invoke();
			}
			_remainCooldownTime = base.cooldownTime;
		}
	}

	private bool CheckHealthCondition()
	{
		switch (_compareType)
		{
		case CompareType.GreaterThanOrEqual:
			if (_healthType == HealthType.Constant && _character.health.currentHealth >= (double)_amount)
			{
				return true;
			}
			if (_healthType == HealthType.Percent && _character.health.percent >= (double)_amount * 0.01)
			{
				return true;
			}
			break;
		case CompareType.LessThan:
			if (_healthType == HealthType.Constant && _character.health.currentHealth <= (double)_amount)
			{
				return true;
			}
			if (_healthType == HealthType.Percent && _character.health.percent <= (double)_amount * 0.01)
			{
				return true;
			}
			break;
		case CompareType.Equal:
			if (_healthType == HealthType.Constant && _character.health.currentHealth >= (double)_amount && _character.health.currentHealth < (double)(_amount + 1))
			{
				return true;
			}
			if (_healthType == HealthType.Percent && _character.health.percent > (double)(_amount - 1) * 0.01 && _character.health.percent < (double)(_amount + 1) * 0.01)
			{
				return true;
			}
			break;
		}
		return false;
	}
}
