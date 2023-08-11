using System;
using Characters.Actions;
using Characters.Player;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class ReduceCooldownAnotherSkill : Ability
{
	public class Instance : AbilityInstance<ReduceCooldownAnotherSkill>
	{
		public Instance(Character owner, ReduceCooldownAnotherSkill ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += HandleOnStartAction;
		}

		private void HandleOnStartAction(Characters.Actions.Action skill)
		{
			if (skill.type == Characters.Actions.Action.Type.Skill)
			{
				WeaponInventory weapon = owner.playerComponents.inventory.weapon;
				Reduce(weapon.current.actionsByType[Characters.Actions.Action.Type.Skill], skill);
				if (ability._containsNextWeapon && (Object)(object)weapon.next != (Object)null)
				{
					Reduce(weapon.next.actionsByType[Characters.Actions.Action.Type.Skill], skill);
				}
			}
		}

		private void Reduce(Characters.Actions.Action[] actions, Characters.Actions.Action except)
		{
			foreach (Characters.Actions.Action action in actions)
			{
				if (action.type != Characters.Actions.Action.Type.Skill || (Object)(object)action == (Object)(object)except)
				{
					continue;
				}
				switch (ability._type)
				{
				case Type.Set:
					switch (ability._valueType)
					{
					case ValueType.Constant:
						action.cooldown.time.remainTime = ability._amount;
						if (action.cooldown.time.remainTime < 0f)
						{
							action.cooldown.time.remainTime = 0f;
						}
						break;
					case ValueType.RemainPercent:
					{
						float num = action.cooldown.time.remainTime * (float)ability._remainPercent * 0.01f;
						action.cooldown.time.remainTime = num;
						break;
					}
					}
					break;
				case Type.Reduce:
					if (action.cooldown.time == null)
					{
						return;
					}
					switch (ability._valueType)
					{
					case ValueType.Constant:
						action.cooldown.time.ReduceCooldown(ability._amount);
						break;
					case ValueType.RemainPercent:
					{
						float time = action.cooldown.time.remainTime * (float)ability._remainPercent * 0.01f;
						action.cooldown.time.ReduceCooldown(time);
						break;
					}
					}
					break;
				}
			}
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= HandleOnStartAction;
		}
	}

	private enum Type
	{
		Set,
		Reduce
	}

	private enum ValueType
	{
		Constant,
		RemainPercent
	}

	[SerializeField]
	private bool _containsNextWeapon;

	[SerializeField]
	private Type _type;

	[SerializeField]
	private ValueType _valueType;

	[SerializeField]
	private float _amount;

	[Range(0f, 100f)]
	[SerializeField]
	private int _remainPercent;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
