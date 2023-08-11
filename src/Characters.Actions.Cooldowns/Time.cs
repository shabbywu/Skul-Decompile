using System;
using System.Collections;
using UnityEngine;

namespace Characters.Actions.Cooldowns;

public class Time : Basic
{
	[NonSerialized]
	public float remainTime;

	[SerializeField]
	protected float _cooldownTime;

	[SerializeField]
	private bool _continual;

	private CoroutineReference _updateReference;

	public override float remainPercent
	{
		get
		{
			if (_remainStreaks > 0)
			{
				return 0f;
			}
			if (base.stacks != _maxStacks)
			{
				return remainTime / _cooldownTime;
			}
			return 0f;
		}
	}

	public float remainStreakPercent => _remainStreaksTime / _streakTimeout;

	public float cooldownTime => _cooldownTime;

	internal override void Initialize(Character character)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize(character);
		((CoroutineReference)(ref _updateReference)).Stop();
		_updateReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)CoroutineProxy.instance, CUpdate());
	}

	protected override void Awake()
	{
		base.Awake();
		remainTime = _cooldownTime;
		if (_continual)
		{
			((Component)this).GetComponentInParent<Action>().onEnd += delegate
			{
				_remainStreaksTime = 0f;
			};
		}
	}

	private void OnDestroy()
	{
		((CoroutineReference)(ref _updateReference)).Stop();
	}

	private IEnumerator CUpdate()
	{
		while (true)
		{
			yield return null;
			if (!((Object)(object)_character == (Object)null) && base.stacks != _maxStacks && _remainStreaks <= 0)
			{
				remainTime -= ((ChronometerBase)_character.chronometer.master).deltaTime;
				if (remainTime <= 0f)
				{
					remainTime = _cooldownTime;
					base.stacks++;
				}
			}
		}
	}
}
