using System;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class CompareHealth : ICondition
{
	public enum Operation
	{
		LessThan,
		LessThanOrEqualTo,
		EqualTo,
		NotEqualTo,
		GreaterThanOrEqualTo,
		GreaterThan
	}

	public enum HealthType
	{
		Percent,
		Constant
	}

	[SerializeField]
	private Operation _operation;

	[SerializeField]
	private HealthType _healthType;

	[SerializeField]
	private CustomFloat _value;

	public bool Satisfied(Character character)
	{
		float num = ((_healthType == HealthType.Constant) ? ((float)character.health.currentHealth) : ((float)character.health.percent * 100f));
		switch (_operation)
		{
		case Operation.LessThan:
			if (!(num < _value.value))
			{
				return false;
			}
			return true;
		case Operation.LessThanOrEqualTo:
			if (!(num <= _value.value))
			{
				return false;
			}
			return true;
		case Operation.EqualTo:
			if (!Mathf.Approximately(num, _value.value))
			{
				return false;
			}
			return true;
		case Operation.NotEqualTo:
			if (Mathf.Approximately(num, _value.value))
			{
				return false;
			}
			return true;
		case Operation.GreaterThanOrEqualTo:
			if (!(num >= _value.value))
			{
				return false;
			}
			return true;
		case Operation.GreaterThan:
			if (!(num > _value.value))
			{
				return false;
			}
			return true;
		default:
			return false;
		}
	}
}
