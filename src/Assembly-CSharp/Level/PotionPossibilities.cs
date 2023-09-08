using System;
using System.Collections.ObjectModel;
using System.Linq;
using GameResources;
using UnityEngine;

namespace Level;

[Serializable]
public class PotionPossibilities
{
	public static readonly ReadOnlyCollection<Potion.Size> values = EnumValues<Potion.Size>.Values;

	[SerializeField]
	[Range(0f, 100f)]
	private int[] _possibilities;

	public int this[int index] => _possibilities[index];

	public int this[Potion.Size size] => _possibilities[(int)size];

	public Potion Get()
	{
		Potion.Size? size = Evaluate(_possibilities);
		if (size.HasValue)
		{
			return CommonResource.instance.potions[size.Value];
		}
		return null;
	}

	public Potion.Size? Evaluate()
	{
		return Evaluate(_possibilities);
	}

	public PotionPossibilities(params int[] possibilities)
	{
		_possibilities = possibilities;
	}

	public static Potion.Size? Evaluate(int[] possibilities)
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
