using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class BoundsAttackInfoSequence : MonoBehaviour
{
	[Serializable]
	internal sealed class Subcomponents : SubcomponentArray<BoundsAttackInfoSequence>
	{
		internal bool noDelay { get; private set; }

		internal void Initialize()
		{
			Array.Sort(base.components, (BoundsAttackInfoSequence x, BoundsAttackInfoSequence y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
			noDelay = true;
			BoundsAttackInfoSequence[] components = base.components;
			foreach (BoundsAttackInfoSequence obj in components)
			{
				if (obj._timeToTrigger > 0f)
				{
					noDelay = false;
				}
				obj.attackInfo.Initialize();
			}
		}

		internal void StopAllOperationsToOwner()
		{
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].attackInfo.operationsToOwner.StopAll();
			}
		}
	}

	[FrameTime]
	[SerializeField]
	private float _timeToTrigger;

	[SerializeField]
	[Subcomponent(typeof(BoundsAttackInfo))]
	private BoundsAttackInfo _attackInfo;

	public float timeToTrigger => _timeToTrigger;

	public BoundsAttackInfo attackInfo => _attackInfo;

	public override string ToString()
	{
		return $"{_timeToTrigger:0.##}s({_timeToTrigger * 60f:0.##}f), {ExtensionMethods.GetAutoName((object)_attackInfo)}";
	}
}
