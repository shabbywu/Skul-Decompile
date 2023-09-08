using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ChangeAction : Ability
{
	public class Instance : AbilityInstance<ChangeAction>
	{
		public Instance(Character owner, ChangeAction ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.playerComponents.inventory.weapon.onSwap += OnSwap;
			for (int i = 0; i < ability._actions.Length; i++)
			{
				ability._weapon.ChangeAction(ability._actions[i], ability._actionsToChange[i]);
				if (ability._copyCooldown)
				{
					ability._actionsToChange[i].cooldown.CopyCooldown(ability._actions[i].cooldown);
				}
			}
			ability._weapon.AttachSkillChanges(ability._skills, ability._skillsToChange, ability._copyCooldown);
		}

		protected override void OnDetach()
		{
			owner.playerComponents.inventory.weapon.onSwap -= OnSwap;
			for (int i = 0; i < ability._actions.Length; i++)
			{
				ability._weapon.ChangeAction(ability._actionsToChange[i], ability._actions[i]);
				if (ability._copyCooldown)
				{
					ability._actions[i].cooldown.CopyCooldown(ability._actionsToChange[i].cooldown);
				}
			}
			ability._weapon.DetachSkillChanges(ability._skills, ability._skillsToChange, ability._copyCooldown);
		}

		private void OnSwap()
		{
			if ((Object)(object)owner.playerComponents.inventory.weapon.current == (Object)(object)ability._weapon)
			{
				SkillInfo[] skills = ability._skills;
				for (int i = 0; i < skills.Length; i++)
				{
					((Component)skills[i]).gameObject.SetActive(true);
				}
				skills = ability._skillsToChange;
				for (int i = 0; i < skills.Length; i++)
				{
					((Component)skills[i]).gameObject.SetActive(true);
				}
				Characters.Actions.Action[] actions = ability._actions;
				for (int i = 0; i < actions.Length; i++)
				{
					((Component)actions[i]).gameObject.SetActive(true);
				}
				actions = ability._actionsToChange;
				for (int i = 0; i < actions.Length; i++)
				{
					((Component)actions[i]).gameObject.SetActive(true);
				}
			}
		}
	}

	[SerializeField]
	[Information("반드시 개수와 순서가 일치해야함, SkillInfo를 가지고 있을 경우 actions 대신 skills쪽에 할당", InformationAttribute.InformationType.Info, true)]
	private Weapon _weapon;

	[Space]
	[SerializeField]
	private SkillInfo[] _skills;

	[SerializeField]
	private SkillInfo[] _skillsToChange;

	[Space]
	[SerializeField]
	private Characters.Actions.Action[] _actions;

	[SerializeField]
	private Characters.Actions.Action[] _actionsToChange;

	[SerializeField]
	private bool _copyCooldown;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
