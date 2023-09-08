using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[Serializable]
public class RarityPossibilities
{
	public static readonly ReadOnlyCollection<Rarity> values = EnumValues<Rarity>.Values;

	[SerializeField]
	[Range(0f, 100f)]
	private int[] _possibilities;

	public int this[int index] => _possibilities[index];

	public int this[Rarity rarity] => _possibilities[(int)rarity];

	public Rarity Evaluate(Random random)
	{
		return Evaluate(random, _possibilities);
	}

	public Rarity Evaluate()
	{
		return Evaluate(new Random(), _possibilities);
	}

	public RarityPossibilities(params int[] possibilities)
	{
		_possibilities = possibilities;
	}

	public static Rarity Evaluate(Random random, int[] possibilities)
	{
		int maxValue = possibilities.Sum();
		int num = random.Next(0, maxValue) + 1;
		for (int i = 0; i < possibilities.Length; i++)
		{
			num -= possibilities[i];
			if (num <= 0)
			{
				return values[i];
			}
		}
		return values[0];
	}
}
