using Characters.Actions;
using UnityEngine;

namespace Characters.Operations;

public class ReduceCooldownTime : CharacterOperation
{
	private enum Type
	{
		Constant,
		Percent
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	[Information("Percent의 경우 (0~1)", InformationAttribute.InformationType.Info, false)]
	private float _amount;

	[SerializeField]
	[Information("특정 액션만 적용할 때", InformationAttribute.InformationType.Info, false)]
	private Action _specificAction;

	[SerializeField]
	private bool _skill;

	[SerializeField]
	private bool _dash;

	[SerializeField]
	private bool _swap;

	public override void Run(Character owner)
	{
		if ((Object)(object)_specificAction != (Object)null)
		{
			Reduce(_specificAction);
			return;
		}
		foreach (Action action in owner.actions)
		{
			if (action.cooldown.time != null)
			{
				bool num = _skill && action.type == Action.Type.Skill;
				bool flag = _dash && action.type == Action.Type.Dash;
				bool flag2 = _swap && action.type == Action.Type.Swap;
				if (num || flag || flag2)
				{
					Reduce(action);
				}
			}
		}
	}

	private void Reduce(Action action)
	{
		switch (_type)
		{
		case Type.Constant:
			action.cooldown.time.ReduceCooldown(_amount);
			break;
		case Type.Percent:
			action.cooldown.time.ReduceCooldownPercent(_amount);
			break;
		}
	}
}
