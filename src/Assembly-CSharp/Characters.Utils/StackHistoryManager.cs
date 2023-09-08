using System.Collections.Generic;
using UnityEngine;

namespace Characters.Utils;

public class StackHistoryManager<T>
{
	private struct StackHistory
	{
		public float startTime;

		public float lastTime;

		public int stack;

		public float lifeTime;
	}

	private readonly Dictionary<T, StackHistory> _history;

	private readonly List<T> _expiredTargets;

	public int Count => _history.Count;

	public StackHistoryManager(int capacity)
	{
		_history = new Dictionary<T, StackHistory>(capacity);
		_expiredTargets = new List<T>(capacity);
	}

	public bool IsElapsedFromLastTime(T target, float time, bool defaultResult = false)
	{
		if (_history.TryGetValue(target, out var value))
		{
			return Time.time - value.lastTime >= time;
		}
		return defaultResult;
	}

	public bool TryAddStack(T target, int increasement, int maxStack, float lifeTime)
	{
		if (_history.TryGetValue(target, out var value))
		{
			value.lastTime = Time.time;
			value.lifeTime = lifeTime;
			if (IsExpired(target))
			{
				value.startTime = Time.time;
				value.stack = increasement;
				_history[target] = value;
				return true;
			}
			if (value.stack + increasement > maxStack)
			{
				value.stack = maxStack;
				_history[target] = value;
				return false;
			}
			value.stack++;
			_history[target] = value;
		}
		else
		{
			value.lastTime = Time.time;
			value.startTime = Time.time;
			value.lifeTime = lifeTime;
			value.stack = increasement;
			_history.Add(target, value);
		}
		return true;
	}

	public bool IsExpired(T target)
	{
		if (_history.TryGetValue(target, out var value))
		{
			if (Time.time - value.startTime > value.lifeTime)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool IsFull(int maxStack)
	{
		if (_history.Count >= maxStack)
		{
			return false;
		}
		return true;
	}

	public bool Has(T target)
	{
		return _history.ContainsKey(target);
	}

	public void Clear()
	{
		_history.Clear();
	}

	public void ClearIfExpired()
	{
		_expiredTargets.Clear();
		foreach (T key in _history.Keys)
		{
			if (IsExpired(key))
			{
				_expiredTargets.Add(key);
			}
		}
		foreach (T expiredTarget in _expiredTargets)
		{
			_history.Remove(expiredTarget);
		}
	}
}
