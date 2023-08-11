using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class WeightedSelector : Decorator
{
	private WeightedRandomizer<Behaviour> _weightedRandomizer;

	[SerializeField]
	[Subcomponent(typeof(Weight))]
	private Weight.Subcomponents _weights;

	private void Awake()
	{
		List<(Behaviour, float)> list = new List<(Behaviour, float)>();
		Weight[] components = ((SubcomponentArray<Weight>)_weights).components;
		foreach (Weight weight in components)
		{
			list.Add((weight.key, weight.value));
		}
		_weightedRandomizer = new WeightedRandomizer<Behaviour>((ICollection<ValueTuple<Behaviour, float>>)list);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _weightedRandomizer.TakeOne().CRun(controller);
		base.result = Result.Success;
	}
}
