using System;
using UnityEngine;

namespace Characters.Operations.FindOptions.Decorator;

[Serializable]
public class Inverter : ICondition
{
	[SerializeReference]
	[SubclassSelector]
	private ICondition _condition;

	public bool Satisfied(Character character)
	{
		return !_condition.Satisfied(character);
	}
}
