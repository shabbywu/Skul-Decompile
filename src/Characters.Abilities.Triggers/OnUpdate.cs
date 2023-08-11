using System;
using Characters.Abilities.Constraints;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnUpdate : Trigger
{
	[Constraint.Subcomponent]
	[SerializeField]
	private Constraint.Subcomponents _constraint;

	[SerializeField]
	private int _times;

	private int _remainTimes;

	public override void Attach(Character character)
	{
		if (_times == 0)
		{
			_times = int.MaxValue;
		}
		_remainTimes = _times;
	}

	public override void Refresh()
	{
		base.Refresh();
		_remainCooldownTime = 0f;
		_remainTimes = _times;
	}

	public override void Detach()
	{
	}

	public override void UpdateTime(float deltaTime)
	{
		base.UpdateTime(deltaTime);
		if (_remainTimes > 0 && !(_remainCooldownTime > 0f) && ((SubcomponentArray<Constraint>)_constraint).components.Pass())
		{
			if (MMMaths.PercentChance(_possibility))
			{
				_remainTimes--;
				_onTriggered?.Invoke();
			}
			_remainCooldownTime = base.cooldownTime;
		}
	}
}
