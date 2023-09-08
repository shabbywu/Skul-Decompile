using System;
using System.Collections.Generic;

public class EnumDeck<TEnum> where TEnum : Enum
{
	private readonly List<TEnum> _list;

	private int _current;

	public int Count => _list.Count;

	public int Remains => _list.Count - _current;

	public EnumDeck(IEnumerable<TEnum> items)
	{
		_list = new List<TEnum>(items);
	}

	public EnumDeck(EnumArray<TEnum, int> items)
	{
		_list = new List<TEnum>();
		foreach (TEnum key in items.Keys)
		{
			for (int i = 0; i < items[key]; i++)
			{
				_list.Add(key);
			}
		}
		_list.Shuffle();
	}

	public void Reset()
	{
		_current = 0;
	}

	public void Shuffle()
	{
		_current = 0;
		_list.Shuffle();
	}

	public TEnum Draw()
	{
		return _list[_current++];
	}

	public TEnum Peek()
	{
		return _list[_current + 1];
	}
}
