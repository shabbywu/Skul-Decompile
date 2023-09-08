using System;
using System.Collections.ObjectModel;
using System.Linq;
using Characters.Gear;
using UnityEngine;

namespace Level;

[Serializable]
public class GearPossibilities
{
	public static readonly ReadOnlyCollection<Gear.Type> values = EnumValues<Gear.Type>.Values;

	[SerializeField]
	[Range(0f, 100f)]
	private int[] _possibilities;

	public int this[int index] => _possibilities[index];

	public int this[Gear.Type size] => _possibilities[(int)size];

	public Gear.Type? Evaluate(Random random)
	{
		return Evaluate(random, _possibilities);
	}

	public Gear.Type? Evaluate()
	{
		return Evaluate(new Random(), _possibilities);
	}

	public GearPossibilities(params int[] possibilities)
	{
		_possibilities = possibilities;
	}

	public static Gear.Type? Evaluate(Random random, int[] possibilities)
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
		return values.Random();
	}
}
