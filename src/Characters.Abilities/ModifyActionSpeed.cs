using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyActionSpeed : Ability
{
	public class Instance : AbilityInstance<ModifyActionSpeed>
	{
		private int _remainCount;

		public override int iconStacks => _remainCount;

		internal Instance(Character owner, ModifyActionSpeed ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Characters.Actions.Action[] actions = ability._actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].extraSpeedMultiplier += ability._extraSpeedMultiplier;
			}
			if (ability._count > 0)
			{
				_remainCount = ability._count;
				owner.onStartAction -= HandleOnStartAction;
				owner.onStartAction += HandleOnStartAction;
			}
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if (action.type == Characters.Actions.Action.Type.Swap)
			{
				owner.ability.Remove(this);
				return;
			}
			bool flag = false;
			Characters.Actions.Action[] actions = ability._actions;
			for (int i = 0; i < actions.Length; i++)
			{
				if ((Object)(object)actions[i] == (Object)(object)action)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				_remainCount--;
				if (_remainCount <= 0)
				{
					owner.ability.Remove(this);
				}
			}
		}

		protected override void OnDetach()
		{
			Characters.Actions.Action[] actions = ability._actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].extraSpeedMultiplier -= ability._extraSpeedMultiplier;
			}
			if (ability._count > 0)
			{
				owner.onStartAction -= HandleOnStartAction;
			}
		}
	}

	[SerializeField]
	private int _count;

	[SerializeField]
	private Characters.Actions.Action[] _actions;

	[SerializeField]
	private float _extraSpeedMultiplier;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
