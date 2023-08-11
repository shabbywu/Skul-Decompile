using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class MultipleAction : Action
{
	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion.Subcomponents _motions;

	public override Motion[] motions => ((SubcomponentArray<Motion>)_motions).components;

	public override bool canUse
	{
		get
		{
			if (!base.cooldown.canUse || _owner.stunedOrFreezed)
			{
				return false;
			}
			for (int i = 0; i < ((SubcomponentArray<Motion>)_motions).components.Length; i++)
			{
				if (PassAllConstraints(((SubcomponentArray<Motion>)_motions).components[i]))
				{
					return true;
				}
			}
			return true;
		}
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		for (int i = 0; i < motions.Length; i++)
		{
			motions[i].Initialize(this);
		}
	}

	public override bool TryStart()
	{
		if (!base.cooldown.canUse || _owner.stunedOrFreezed)
		{
			return false;
		}
		for (int i = 0; i < ((SubcomponentArray<Motion>)_motions).components.Length; i++)
		{
			if (PassAllConstraints(((SubcomponentArray<Motion>)_motions).components[i]) && ConsumeCooldownIfNeeded())
			{
				DoAction(((SubcomponentArray<Motion>)_motions).components[i]);
				return true;
			}
		}
		return false;
	}
}
