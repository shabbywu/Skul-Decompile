using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class EnhanceableSimpleAction : Action
{
	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion _motion;

	[SerializeField]
	[Subcomponent(typeof(Motion))]
	protected Motion _enhancedMotion;

	[NonSerialized]
	public bool enhanced;

	private Motion currentMotion
	{
		get
		{
			if (!enhanced)
			{
				return _motion;
			}
			return _enhancedMotion;
		}
	}

	public override Motion[] motions => new Motion[1] { _motion };

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(currentMotion);
			}
			return false;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_motion.onEnd += delegate
		{
			_onEnd?.Invoke();
		};
		_motion.onCancel += delegate
		{
			_onCancel?.Invoke();
		};
		_enhancedMotion.onEnd += delegate
		{
			_onEnd?.Invoke();
		};
		_enhancedMotion.onCancel += delegate
		{
			_onCancel?.Invoke();
		};
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		_motion.Initialize(this);
		_enhancedMotion.Initialize(this);
	}

	public override bool TryStart()
	{
		if (!canUse || !ConsumeCooldownIfNeeded())
		{
			return false;
		}
		DoAction(currentMotion);
		return true;
	}
}
