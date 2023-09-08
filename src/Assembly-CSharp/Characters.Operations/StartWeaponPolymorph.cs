using System;
using System.Collections.Generic;
using Characters.Controllers;
using Characters.Gear.Weapons;
using Services;
using UnityEngine;

namespace Characters.Operations;

public class StartWeaponPolymorph : CharacterOperation
{
	[Serializable]
	private class SkillMap
	{
		[Serializable]
		public class Reorderable : ReorderableArray<SkillMap>
		{
		}

		public SkillInfo originalSkill;

		public string polymorphSkillKey;

		public bool copyCooldown;
	}

	[SerializeField]
	private bool _followSkillOrder;

	[SerializeField]
	private Weapon _polymorphWeapon;

	[SerializeField]
	private SkillMap.Reorderable _skillMaps;

	private Weapon _weaponInstance;

	private Weapon _before;

	private List<SkillInfo> _matchedSkillInfos;

	private void Awake()
	{
		_weaponInstance = Object.Instantiate<Weapon>(_polymorphWeapon);
		((Component)_weaponInstance).transform.parent = null;
		((Object)_weaponInstance).name = ((Object)_polymorphWeapon).name;
		_weaponInstance.Initialize();
		((Component)_weaponInstance).gameObject.SetActive(false);
		if (_followSkillOrder)
		{
			_matchedSkillInfos = new List<SkillInfo>(_weaponInstance.currentSkills.Capacity);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!Service.quitting)
		{
			Object.Destroy((Object)(object)((Component)_weaponInstance).gameObject);
		}
	}

	public override void Run(Character target)
	{
		_before = target.playerComponents.inventory.weapon.current;
		target.playerComponents.inventory.weapon.Polymorph(_weaponInstance);
		ApplySkillMap();
	}

	private void ApplySkillMap()
	{
		if (_skillMaps.values.Length == 0)
		{
			return;
		}
		if (_weaponInstance.currentSkills != null)
		{
			_weaponInstance.currentSkills.Clear();
		}
		if (_followSkillOrder)
		{
			_matchedSkillInfos.Clear();
		}
		SkillMap[] values = _skillMaps.values;
		foreach (SkillMap skillMap in values)
		{
			SkillInfo[] skills = _weaponInstance.skills;
			foreach (SkillInfo skillInfo in skills)
			{
				if (skillMap.originalSkill.action.button != Button.None && skillInfo.key.Equals(skillMap.polymorphSkillKey, StringComparison.OrdinalIgnoreCase))
				{
					if (skillMap.copyCooldown)
					{
						skillInfo.action.cooldown.CopyCooldown(skillMap.originalSkill.action.cooldown);
					}
					_weaponInstance.currentSkills.Add(skillInfo);
					if (_followSkillOrder)
					{
						_matchedSkillInfos.Add(skillMap.originalSkill);
					}
					break;
				}
			}
		}
		if (_followSkillOrder)
		{
			string key = _before.currentSkills[0].key;
			string key2 = _matchedSkillInfos[0].key;
			if (!key.Equals(key2, StringComparison.OrdinalIgnoreCase))
			{
				_weaponInstance.SwapSkillOrder();
			}
		}
		_weaponInstance.SetSkillButtons();
	}
}
