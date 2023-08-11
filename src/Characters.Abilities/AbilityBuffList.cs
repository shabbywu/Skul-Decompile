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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return ExtensionMethods.Random<AbilityBuff>(_abilityBuff.Where((AbilityBuff food) => food.rarity == rarity), random);
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
