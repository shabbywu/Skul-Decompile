using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks;

public sealed class DarkAbilityContainer : MonoBehaviour
{
	private static int extraSeed;

	private const int _randomSeed = 1177618293;

	[Header("Ability 개수 설정")]
	[Range(0f, 100f)]
	[SerializeField]
	private int _singleAbility = 100;

	[SerializeField]
	[Range(0f, 100f)]
	private int _dualAbility;

	[Header("Ability Pool")]
	[SerializeField]
	[Subcomponent(typeof(WeightedDarkAbility))]
	private WeightedDarkAbility.Subcomponents _weightedDarkAbility;

	private WeightedRandomizer<DarkAbility> _weightedRandomizer;

	private List<(DarkAbility, float)> _candidates;

	private Random _random;

	private int[] _countWeight;

	private List<DarkAbility> _electedAbilities;

	private Character _owner;

	public void Initialize(Character owner)
	{
		extraSeed++;
		_owner = owner;
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 1177618293 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex + extraSeed);
		_countWeight = new int[2] { _singleAbility, _dualAbility };
		_candidates = new List<(DarkAbility, float)>();
		WeightedDarkAbility[] components = ((SubcomponentArray<WeightedDarkAbility>)_weightedDarkAbility).components;
		foreach (WeightedDarkAbility weightedDarkAbility in components)
		{
			if (weightedDarkAbility.Available(owner))
			{
				_candidates.Add((weightedDarkAbility.key, weightedDarkAbility.value));
			}
		}
		_weightedRandomizer = new WeightedRandomizer<DarkAbility>((ICollection<ValueTuple<DarkAbility, float>>)_candidates);
	}

	public ICollection<DarkAbility> GetDarkAbility()
	{
		_electedAbilities = new List<DarkAbility>();
		int num = EvaluateCount(_random);
		while (num > 0)
		{
			DarkAbility darkAbility = _weightedRandomizer.TakeOne(_random);
			DarkAbility darkAbility2 = Object.Instantiate<DarkAbility>(darkAbility, _owner.attach.transform);
			((Object)darkAbility2).name = ((Object)darkAbility).name;
			darkAbility2.Initialize();
			_electedAbilities.Add(darkAbility2);
			num--;
			if (num <= 0)
			{
				continue;
			}
			for (int num2 = _candidates.Count; num2 >= 0; num2--)
			{
				if ((Object)(object)_candidates[num2].Item1 == (Object)(object)darkAbility)
				{
					_candidates.RemoveAt(num2);
					_weightedRandomizer = new WeightedRandomizer<DarkAbility>((ICollection<ValueTuple<DarkAbility, float>>)_candidates);
					break;
				}
			}
		}
		return _electedAbilities;
	}

	private void OnDestroy()
	{
		extraSeed--;
		if ((Object)(object)_owner == (Object)null || _owner.health.dead || _electedAbilities == null)
		{
			return;
		}
		foreach (DarkAbility electedAbility in _electedAbilities)
		{
			electedAbility.RemoveFrom(_owner);
		}
	}

	public int EvaluateCount(Random random)
	{
		int maxValue = Mathf.Max(_countWeight.Sum(), 100);
		int num = random.Next(0, maxValue) + 1;
		for (int i = 0; i < _countWeight.Length; i++)
		{
			num -= _countWeight[i];
			if (num <= 0)
			{
				return i + 1;
			}
		}
		return 1;
	}

	public void ResetAllWeightValue()
	{
		WeightedDarkAbility[] components = ((SubcomponentArray<WeightedDarkAbility>)_weightedDarkAbility).components;
		for (int i = 0; i < components.Length; i++)
		{
			components[i].ResetValue();
		}
	}
}
