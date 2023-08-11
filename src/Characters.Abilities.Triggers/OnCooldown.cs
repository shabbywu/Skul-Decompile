using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Actions;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnCooldown : Trigger
{
	private enum Type
	{
		Any,
		All,
		Count
	}

	public enum Timing
	{
		Start,
		End
	}

	private const float _checkInterval = 0.1f;

	[SerializeField]
	private Type _type;

	[SerializeField]
	private int _count;

	private Character _character;

	private float _elapsed;

	public override void Attach(Character character)
	{
		_character = character;
	}

	public override void UpdateTime(float deltaTime)
	{
		base.UpdateTime(deltaTime);
		_elapsed += deltaTime;
		if (!(_elapsed < 0.1f))
		{
			_elapsed = 0f;
			Check();
		}
	}

	private void Check()
	{
		Weapon current = _character.playerComponents.inventory.weapon.current;
		List<Characters.Actions.Action> list = current.actionsByType[Characters.Actions.Action.Type.Skill].ToList();
		Weapon next = _character.playerComponents.inventory.weapon.next;
		if ((Object)(object)next != (Object)null)
		{
			list.AddRange(next.actionsByType[Characters.Actions.Action.Type.Skill]);
		}
		int num = 0;
		foreach (Characters.Actions.Action item in list)
		{
			if (item.type == Characters.Actions.Action.Type.Skill)
			{
				if (item.cooldown.maxStack > 1 && item.cooldown.stacks < item.cooldown.maxStack)
				{
					num++;
				}
				else if (!item.cooldown.canUse)
				{
					num++;
				}
			}
		}
		if (!base.canBeTriggered)
		{
			return;
		}
		switch (_type)
		{
		case Type.All:
			if (!((Object)(object)next == (Object)null) && current.currentSkills.Count + next.currentSkills.Count == num)
			{
				Invoke();
			}
			break;
		case Type.Any:
			if (num >= 1)
			{
				Invoke();
			}
			break;
		case Type.Count:
			if (num == _count)
			{
				Invoke();
			}
			break;
		}
	}

	public override void Detach()
	{
	}
}
