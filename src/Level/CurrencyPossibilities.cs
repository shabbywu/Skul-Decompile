using System;
using System.Collections.ObjectModel;
using System.Linq;
using Data;
using UnityEngine;

namespace Level;

[Serializable]
public class CurrencyPossibilities
{
	public static readonly ReadOnlyCollection<GameData.Currency.Type> values = EnumValues<GameData.Currency.Type>.Values;

	[Range(0f, 100f)]
	[SerializeField]
	private int[] _possibilities;

	public int this[int index] => _possibilities[index];

	public int this[GameData.Currency.Type size] => _possibilities[(int)size];

	public GameData.Currency.Type? Evaluate(Random random)
	{
		return Evaluate(random, _possibilities);
	}

	public GameData.Currency.Type? Evaluate()
	{
		return Evaluate(_possibilities);
	}

	public CurrencyPossibilities(params int[] possibilities)
	{
		_possibilities = possibilities;
	}

	public GameData.Currency.Type? Evaluate(Random random, int[] possibilities)
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

	public static GameData.Currency.Type? Evaluate(int[] possibilities)
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
}
