using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Utils;

public sealed class GrabBoard : MonoBehaviour
{
	[SerializeField]
	private int _maxTargetCount = 32;

	[SerializeField]
	private int _maxFailedTargetCount = 32;

	private List<Target> _targets;

	private List<Target> _failTargets;

	public List<Target> targets => _targets;

	public List<Target> failTargets => _failTargets;

	public int maxTargetCount => _maxTargetCount;

	public void Clear()
	{
		_targets.Clear();
		_failTargets.Clear();
	}

	public void Add(Target target)
	{
		if (!_targets.Contains(target) && targets.Count <= _maxTargetCount)
		{
			_targets.Add(target);
		}
	}

	public void AddFailed(Target target)
	{
		if (!_failTargets.Contains(target) && failTargets.Count <= _maxFailedTargetCount)
		{
			_failTargets.Add(target);
		}
	}

	public bool HasInTargets(Character character)
	{
		foreach (Target target in _targets)
		{
			if ((Object)(object)target.character == (Object)(object)character)
			{
				return true;
			}
		}
		return false;
	}

	public int TargetCount()
	{
		return _targets.Count;
	}

	private void Awake()
	{
		_targets = new List<Target>(_maxTargetCount);
		_failTargets = new List<Target>(_maxFailedTargetCount);
	}

	public void Remove(Target target)
	{
		if (_targets.Contains(target))
		{
			_targets.Remove(target);
		}
	}
}
