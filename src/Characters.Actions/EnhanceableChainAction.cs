using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class EnhanceableChainAction : Action
{
	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion.Subcomponents _motions;

	[SerializeField]
	[Subcomponent(typeof(Motion))]
	protected Motion.Subcomponents _enhancedMotions;

	private Character.LookingDirection _lookingDirection;

	[NonSerialized]
	public bool enhanced;

	public override Motion[] motions
	{
		get
		{
			if (!enhanced)
			{
				return ((SubcomponentArray<Motion>)_motions).components;
			}
			return ((SubcomponentArray<Motion>)_enhancedMotions).components;
		}
	}

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(motions[0]);
			}
			return false;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		InitailizeMotionEvents(((SubcomponentArray<Motion>)_motions).components);
		InitailizeMotionEvents(((SubcomponentArray<Motion>)_enhancedMotions).components);
	}

	private void InitailizeMotionEvents(Motion[] motions)
	{
		bool flag = false;
		for (int i = 0; i < motions.Length; i++)
		{
			Motion motion = motions[i];
			if (motion.blockLook)
			{
				if (flag)
				{
					motion.onStart += delegate
					{
						base.owner.lookingDirection = _lookingDirection;
					};
				}
				motion.onStart += delegate
				{
					_lookingDirection = base.owner.lookingDirection;
				};
			}
			flag = motion.blockLook;
			if (i + 1 < motions.Length)
			{
				int cached = i + 1;
				motions[i].onEnd += delegate
				{
					DoMotion(motions[cached]);
				};
			}
		}
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		for (int i = 0; i < motions.Length; i++)
		{
			((SubcomponentArray<Motion>)_motions).components[i].Initialize(this);
		}
		for (int j = 0; j < motions.Length; j++)
		{
			((SubcomponentArray<Motion>)_enhancedMotions).components[j].Initialize(this);
		}
	}

	public override bool TryStart()
	{
		if (!((Component)this).gameObject.activeSelf || !canUse || !ConsumeCooldownIfNeeded())
		{
			return false;
		}
		DoAction(motions[0]);
		return true;
	}
}
