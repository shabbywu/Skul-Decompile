using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public abstract class EnumArray
{
	public abstract int Length { get; }
}
[Serializable]
public class EnumArray<TEnum, T> : EnumArray, IEnumerable<T>, IEnumerable where TEnum : Enum
{
	[SerializeField]
	private T[] _array;

	public T[] Array
	{
		get
		{
			return _array;
		}
		private set
		{
			_array = value;
		}
	}

	public override int Length => Array.Length;

	public ReadOnlyCollection<TEnum> Keys => EnumValues<TEnum>.Values;

	public int Count => Keys.Count;

	public bool IsReadOnly => false;

	public T this[TEnum key]
	{
		get
		{
			return Array[Convert.ToInt32(key)];
		}
		set
		{
			Array[Convert.ToInt32(key)] = value;
		}
	}

	public EnumArray()
	{
		Array = new T[Keys.Count];
	}

	public EnumArray(params T[] array)
	{
		Array = new T[Keys.Count];
		for (int i = 0; i < Math.Min(array.Length, Array.Length); i++)
		{
			Array[i] = array[i];
		}
	}

	public EnumArray(EnumArray<TEnum, T> source)
	{
		Array = (T[])source.Array.Clone();
	}

	public bool IsDefined(string type)
	{
		return Enum.IsDefined(typeof(TEnum), type);
	}

	public T Get(string type)
	{
		if (!IsDefined(type))
		{
			throw new ArgumentException("Try to get invalid data type", "type");
		}
		return Array[(int)Enum.Parse(typeof(TEnum), type, ignoreCase: true)];
	}

	public T Get(TEnum type)
	{
		return Array[Convert.ToInt32(type)];
	}

	public T GetOrDefault(TEnum type)
	{
		int num = Convert.ToInt32(type);
		if (num < 0 || num >= Array.Length)
		{
			return default(T);
		}
		return Array[num];
	}

	public bool TryGet(TEnum type, out T value)
	{
		int num = Convert.ToInt32(type);
		if (num < 0 || num >= Array.Length)
		{
			value = default(T);
			return false;
		}
		value = Array[num];
		return true;
	}

	public void Set(string type, T value)
	{
		if (!IsDefined(type))
		{
			throw new ArgumentException("Try to set invalid data type", "type");
		}
		Array[(int)Enum.Parse(typeof(TEnum), type, ignoreCase: true)] = value;
	}

	public void Set(TEnum type, T value)
	{
		Array[Convert.ToInt32(type)] = value;
	}

	public void Set(EnumArray<TEnum, T> other)
	{
		System.Array.Copy(other.Array, Array, Array.Length);
	}

	public void SetAll(T value)
	{
		for (int i = 0; i < Array.Length; i++)
		{
			Array[i] = value;
		}
	}

	public KeyValuePair<TEnum, T>[] ToKeyValuePairs()
	{
		return Keys.Zip(Array, (TEnum key, T value) => new KeyValuePair<TEnum, T>(key, value)).ToArray();
	}

	public EnumArray<TEnum, T> Clone()
	{
		return new EnumArray<TEnum, T>(this);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)Array).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Array.GetEnumerator();
	}
}
