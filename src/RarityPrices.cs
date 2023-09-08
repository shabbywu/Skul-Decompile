using System;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class RarityPrices
{
	public static readonly ReadOnlyCollection<string> names = EnumValues<Rarity>.Names;

	public static readonly ReadOnlyCollection<Rarity> values = EnumValues<Rarity>.Values;

	[SerializeField]
	private int[] _prices;

	public int this[int index] => _prices[index];

	public int this[Rarity rarity] => _prices[(int)rarity];

	public RarityPrices(params int[] prices)
	{
		_prices = prices;
	}
}
