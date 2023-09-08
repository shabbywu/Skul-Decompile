using System;
using System.Collections.Generic;
using UnityEngine;

public static class WeightedRandomizer
{
	public static WeightedRandomizer<R> From<R>(ICollection<(R key, float value)> spawnRate)
	{
		return new WeightedRandomizer<R>(spawnRate);
	}

	public static WeightedRandomizer<R> From<R>(params (R key, float value)[] spawnRate)
	{
		return new WeightedRandomizer<R>(spawnRate);
	}
}
public class WeightedRandomizer<T>
{
	private ICollection<(T key, float value)> _weights;

	public WeightedRandomizer(ICollection<(T key, float value)> weights)
	{
		_weights = weights;
	}

	public WeightedRandomizer(params (T key, float value)[] weights)
	{
		_weights = weights;
	}

	public T TakeOne()
	{
		List<(T, float)> list = Sort(_weights);
		float num = 0f;
		foreach (var weight in _weights)
		{
			num += weight.value;
		}
		float num2 = Random.Range(0f, num) + 1f;
		T result = list[list.Count - 1].Item1;
		foreach (var item in list)
		{
			num2 -= item.Item2;
			if (num2 < item.Item2)
			{
				(result, _) = item;
				break;
			}
		}
		return result;
	}

	public T TakeOne(Random seed)
	{
		List<(T, float)> list = Sort(_weights);
		float num = 0f;
		foreach (var weight in _weights)
		{
			num += weight.value;
		}
		float num2 = seed.Next((int)num) + 1;
		T result = list[list.Count - 1].Item1;
		foreach (var item in list)
		{
			num2 -= item.Item2;
			if (num2 < item.Item2)
			{
				(result, _) = item;
				break;
			}
		}
		return result;
	}

	private List<(T key, float value)> Sort(ICollection<(T key, float value)> weights)
	{
		List<(T, float)> list = new List<(T, float)>(weights);
		list.Sort(((T key, float value) firstPair, (T key, float value) nextPair) => firstPair.value.CompareTo(nextPair.value));
		return list;
	}
}
