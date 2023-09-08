using System.Collections.Generic;
using UnityEngine;

namespace Characters.Utils;

public class HitHistoryManager
{
	private struct HitHistory
	{
		public float time;

		public float count;
	}

	private readonly Dictionary<Target, HitHistory> _hitHistory;

	public int Count => _hitHistory.Count;

	public HitHistoryManager(int capacity)
	{
		_hitHistory = new Dictionary<Target, HitHistory>(capacity);
	}

	public void AddOrUpdate(Target target)
	{
		if (_hitHistory.TryGetValue(target, out var value))
		{
			value.time = Time.time;
			value.count += 1f;
			_hitHistory[target] = value;
		}
		else
		{
			value.time = Time.time;
			value.count = 1f;
			_hitHistory.Add(target, value);
		}
	}

	public void ClearIfExpired()
	{
	}

	public void Clear()
	{
		_hitHistory.Clear();
	}

	public bool CanAttack(Target target, int maxHit, int maxHitsPerUnit, float interval)
	{
		if (_hitHistory.Count >= maxHit)
		{
			return false;
		}
		if (_hitHistory.TryGetValue(target, out var value))
		{
			if (Time.time - value.time > interval)
			{
				return value.count < (float)maxHitsPerUnit;
			}
			return false;
		}
		return true;
	}
}
