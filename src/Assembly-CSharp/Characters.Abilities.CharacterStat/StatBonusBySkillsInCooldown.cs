using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Player;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatBonusBySkillsInCooldown : Ability
{
	public class Instance : AbilityInstance<StatBonusBySkillsInCooldown>
	{
		private int _stacks;

		private Stat.Values _stat;

		public override Sprite icon
		{
			get
			{
				if (_stacks > 0)
				{
					return base.icon;
				}
				return null;
			}
		}

		public override int iconStacks => _stacks;

		public Instance(Character owner, StatBonusBySkillsInCooldown ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			owner.stat.AttachValues(_stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			int num = 0;
			WeaponInventory weapon = owner.playerComponents.inventory.weapon;
			Weapon current = weapon.current;
			Weapon next = weapon.next;
			SkillInfo[] skills = current.skills;
			for (int i = 0; i < skills.Length; i++)
			{
				Characters.Actions.Action action = skills[i].action;
				if (action.cooldown.time != null && !action.cooldown.canUse)
				{
					num++;
				}
			}
			if ((Object)(object)next != (Object)null)
			{
				skills = next.skills;
				for (int i = 0; i < skills.Length; i++)
				{
					Characters.Actions.Action action2 = skills[i].action;
					if (action2.cooldown.time != null && !action2.cooldown.canUse)
					{
						num++;
					}
				}
			}
			if (num >= ability._maxStack)
			{
				num = ability._maxStack;
			}
			_stacks = num;
			UpdateStat();
		}

		private void UpdateStat()
		{
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._statPerStack.values[i].GetStackedValue(_stacks);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _statPerStack;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private int _skillInCooldownPerStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
