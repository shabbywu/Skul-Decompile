using UnityEngine;

namespace Characters.Operations.Health;

public class Heal : CharacterOperation
{
	private enum Type
	{
		Percent,
		Constnat
	}

	[SerializeField]
	private Character _target;

	[SerializeField]
	private Type _type;

	[SerializeField]
	private CustomFloat _amount;

	public override void Run(Character owner)
	{
		if ((Object)(object)_target == (Object)null)
		{
			owner.health.Heal(GetAmount(owner));
		}
		else
		{
			_target.health.Heal(GetAmount(_target));
		}
	}

	private double GetAmount(Character target)
	{
		return _type switch
		{
			Type.Percent => (double)_amount.value * target.health.maximumHealth * 0.01, 
			Type.Constnat => _amount.value, 
			_ => 0.0, 
		};
	}
}
