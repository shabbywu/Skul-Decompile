using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class LowestHealth : IFilter
{
	private enum HealthType
	{
		Percent,
		Constant
	}

	[SerializeField]
	private HealthType _healthType;

	public void Filtered(List<Character> characters)
	{
		double num = 2147483647.0;
		Character item = null;
		foreach (Character character in characters)
		{
			double num2 = ((_healthType != HealthType.Constant) ? character.health.percent : character.health.currentHealth);
			if (num2 < num)
			{
				num = num2;
				item = character;
			}
		}
		characters.Clear();
		characters.Add(item);
	}
}
