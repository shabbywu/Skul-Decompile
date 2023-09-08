using System;
using System.Linq;
using UnityEngine;

namespace Characters.Abilities;

public class AbilityBuffList : ScriptableObject
{
	[SerializeField]
	private AbilityBuff[] _abilityBuff;

	public AbilityBuff[] abilityBuff => _abilityBuff;

	public AbilityBuff Take(Random random, Rarity rarity)
	{
		return _abilityBuff.Where((AbilityBuff food) => food.rarity == rarity).Random(random);
	}

	public AbilityBuff Get(string name)
	{
		AbilityBuff[] array = _abilityBuff;
		foreach (AbilityBuff abilityBuff in array)
		{
			if (((Object)abilityBuff).name.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				return abilityBuff;
			}
		}
		return null;
	}
}
