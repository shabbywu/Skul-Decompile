using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Usables;

public sealed class LiquidMaster : MonoBehaviour
{
	private List<Liquid> _liquidList;

	public event Action onChanged;

	private void Awake()
	{
		_liquidList = new List<Liquid>();
	}

	public int Count()
	{
		return _liquidList.Count;
	}

	public void Add(Liquid liquid)
	{
		_liquidList.Add(liquid);
		this.onChanged?.Invoke();
	}

	public void Remove(Liquid liquid)
	{
		_liquidList.Remove(liquid);
		this.onChanged?.Invoke();
	}

	public int GetStack()
	{
		int num = 0;
		foreach (Liquid liquid in _liquidList)
		{
			num += liquid.stack;
		}
		return num;
	}
}
