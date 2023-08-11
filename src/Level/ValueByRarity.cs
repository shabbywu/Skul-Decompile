using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Level;

[Serializable]
public class ValueByRarity
{
	public static readonly ReadOnlyCollection<string> names = EnumValues<Rarity>.Names;

	public static readonly ReadOnlyCollection<Rarity> values = EnumValues<Rarity>.Values;

	[SerializeField]
	private float[] _values;

	public float this[int index] => _values[index];

	public float this[Rarity rarity] => _values[rarity];

	public ValueByRarity(params float[] values)
	{
		_values = values;
	}
}
