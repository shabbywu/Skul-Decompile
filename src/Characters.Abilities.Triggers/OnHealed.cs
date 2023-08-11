using System;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnHealed : Trigger
{
	private enum Type
	{
		Constant,
		Percent
	}

	private enum Comparer
	{
		GreaterThanOrEqual,
		LessThan,
		Equal
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private Comparer _comparer;

	[SerializeField]
	private int _amount;

	private Character _character;

	public override void Attach(Character character)
	{
		_character = character;
		_character.health.onHealed += HandleOnHealed;
	}

	private void HandleOnHealed(double healed, double overHealed)
	{
		if (CheckHealthCondition(healed))
		{
			Invoke();
		}
	}

	public override void Detach()
	{
		_character.health.onHealed -= HandleOnHealed;
	}

	private bool CheckHealthCondition(double healed)
	{
		switch (_comparer)
		{
		case Comparer.GreaterThanOrEqual:
			if (_type == Type.Constant && healed >= (double)_amount)
			{
				return true;
			}
			if (_type == Type.Percent && healed / _character.health.maximumHealth >= (double)_amount * 0.01)
			{
				return true;
			}
			break;
		case Comparer.LessThan:
			if (_type == Type.Constant && healed <= (double)_amount)
			{
				return true;
			}
			if (_type == Type.Percent && healed / _character.health.maximumHealth <= (double)_amount * 0.01)
			{
				return true;
			}
			break;
		case Comparer.Equal:
			if (_type == Type.Constant && healed >= (double)_amount && healed < (double)(_amount + 1))
			{
				return true;
			}
			if (_type == Type.Percent && healed / _character.health.maximumHealth > (double)(_amount - 1) * 0.01 && healed / _character.health.maximumHealth < (double)(_amount + 1) * 0.01)
			{
				return true;
			}
			break;
		}
		return false;
	}
}
