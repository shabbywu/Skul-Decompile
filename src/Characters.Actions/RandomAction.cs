using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class RandomAction : Action
{
	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion.Subcomponents _motions;

	private int _indexToUse;

	public override Motion[] motions => ((SubcomponentArray<Motion>)_motions).components;

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(((SubcomponentArray<Motion>)_motions).components[_indexToUse]);
			}
			return false;
		}
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		RandomizeIndex();
		for (int i = 0; i < motions.Length; i++)
		{
			motions[i].Initialize(this);
		}
	}

	private void RandomizeIndex()
	{
		_indexToUse = ExtensionMethods.RandomIndex<Motion>((IEnumerable<Motion>)((SubcomponentArray<Motion>)_motions).components);
	}

	public override bool TryStart()
	{
		if (!base.cooldown.canUse || !ConsumeCooldownIfNeeded())
		{
			return false;
		}
		DoAction(((SubcomponentArray<Motion>)_motions).components[_indexToUse]);
		RandomizeIndex();
		return true;
	}
}
