using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Level;

[Serializable]
public class WeightedTier
{
	public static readonly ReadOnlyCollection<Tier> values = EnumValues<Tier>.Values;

	[SerializeField]
	[Range(0f, 100f)]
	private int[] _possibilities;

	public int this[int index] => _possibilities[index];

	public int this[Tier size] => _possibilities[(int)size];

	public WeightedTier(params int[] possibilities)
	{
		_possibilities = possibilities;
	}

	public Tier Get(Random random)
	{
		Tier? tier = Evaluate(_possibilities, random);
		if (tier.HasValue)
		{
			return tier.Value;
		}
		return Tier.Low;
	}

	public static Tier? Evaluate(int[] possibilities)
	{
		int num = Mathf.Max(possibilities.Sum(), 100);
		int num2 = Random.Range(0, num) + 1;
		for (int i = 0; i < possibilities.Length; i++)
		{
			num2 -= possibilities[i];
			if (num2 <= 0)
			{
				return values[i];
			}
		}
		return null;
	}

	public static Tier? Evaluate(int[] possibilities, Random random)
	{
		int maxValue = Mathf.Max(possibilities.Sum(), 100);
		int num = random.Next(0, maxValue) + 1;
		for (int i = 0; i < possibilities.Length; i++)
		{
			num -= possibilities[i];
			if (num <= 0)
			{
				return values[i];
			}
		}
		return null;
	}
}
