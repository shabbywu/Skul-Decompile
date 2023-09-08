using System;
using Data;
using UnityEngine;

namespace Characters.Gear.Upgrades;

[Serializable]
public class Upgrade
{
	[UpgradeAbility.Subcomponent]
	[SerializeField]
	private UpgradeAbility.Subcomponents _passive;

	[UpgradeAbility.Subcomponent]
	[SerializeField]
	private UpgradeAbility.Subcomponents _abilityByLevel;

	private Character _owner;

	public GameData.Currency.Type currencyTypeByDiscard => GameData.Currency.Type.Gold;

	public int maxLevel => _abilityByLevel.components.Length;

	public UpgradeAbility GetAbility(int level)
	{
		if (level == 0)
		{
			return null;
		}
		return _abilityByLevel[level - 1];
	}

	public void Attach(Character character)
	{
		_owner = character;
		_passive.Attach(character);
		_abilityByLevel[0].Attach(character);
	}

	public void LevelUp(int level)
	{
		if (level > maxLevel || maxLevel == 0)
		{
			Debug.Log((object)"대상의 레벨이 최대 레벨을 초과했습니다.");
			return;
		}
		_abilityByLevel.DetachAll();
		_abilityByLevel[level - 1].Attach(_owner);
	}

	public void Detach()
	{
		_passive.DetachAll();
		_abilityByLevel.DetachAll();
	}
}
