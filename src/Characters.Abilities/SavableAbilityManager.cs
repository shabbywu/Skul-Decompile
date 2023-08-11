using System;
using Characters.Abilities.Savable;
using Data;
using UnityEngine;

namespace Characters.Abilities;

public sealed class SavableAbilityManager : MonoBehaviour
{
	public enum Name
	{
		Curse,
		FogWolfBuff,
		HealthAuxiliaryDamage,
		HealthAuxiliaryHealth,
		HealthAuxiliarySpeed,
		PurchasedMaxHealth,
		PurchasedShield,
		OrganicIcedTea,
		BrutalityBuff,
		RageBuff,
		FortitudeBuff,
		EssenceSpirit,
		LifeChange
	}

	[GetComponent]
	[SerializeField]
	private Character _character;

	private EnumArray<Name, ISavableAbility> _abilities;

	private void Awake()
	{
		_abilities = new EnumArray<Name, ISavableAbility>();
		LoadAll();
	}

	public float GetStack(Name name)
	{
		return _abilities[name].stack;
	}

	public void Remove(Name name)
	{
		_character.ability.Remove(_abilities[name].ability);
	}

	public void Apply(Name name)
	{
		_character.ability.Add(_abilities[name].ability);
	}

	public void Apply(Name name, int stack)
	{
		_abilities[name].stack = stack;
		_character.ability.Add(_abilities[name].ability);
	}

	public void Apply(Name name, float stack)
	{
		_abilities[name].stack = stack;
		_character.ability.Add(_abilities[name].ability);
	}

	public void Apply(Name name, float stack, float remainTime)
	{
		_abilities[name].stack = stack;
		_abilities[name].remainTime = remainTime;
		_character.ability.Add(_abilities[name].ability);
	}

	public void IncreaseStack(Name name, float plus)
	{
		_abilities[name].stack += plus;
		Apply(name);
	}

	public void SaveAll()
	{
		foreach (Name value in Enum.GetValues(typeof(Name)))
		{
			if (_abilities[value] != null)
			{
				GameData.Buff buff = GameData.Buff.Get(value);
				buff.remainTime = _abilities[value].remainTime;
				buff.stack = _abilities[value].stack;
				buff.attached = _character.ability.Contains(_abilities[value].ability);
				buff.Save();
			}
		}
	}

	public void ResetAll()
	{
		foreach (ISavableAbility ability in _abilities)
		{
			_character.ability.Remove(ability.ability);
		}
		GameData.Buff.ResetAll();
	}

	public void LoadAll()
	{
		Load(new Curse(), Name.Curse);
		Load(new FogWolfBuff(), Name.FogWolfBuff);
		Load(new HealthAuxiliaryDamage(), Name.HealthAuxiliaryDamage);
		Load(new HealthAuxiliaryHealth(), Name.HealthAuxiliaryHealth);
		Load(new HealthAuxiliarySpeed(), Name.HealthAuxiliarySpeed);
		Load(new PurchasedMaxHealth(), Name.PurchasedMaxHealth);
		Load(new PurchasedShield(), Name.PurchasedShield);
		Load(new BrutalityBuff(), Name.BrutalityBuff);
		Load(new RageBuff(), Name.RageBuff);
		Load(new FortitudeBuff(), Name.FortitudeBuff);
		Load(new EssenceSpirit(), Name.EssenceSpirit);
		Load(new LifeChange(), Name.LifeChange);
		Load(new PurchasedShield(), Name.OrganicIcedTea);
	}

	private void Load(ISavableAbility savableAbility, Name name)
	{
		GameData.Buff buff = GameData.Buff.Get(name);
		if (buff.attached)
		{
			savableAbility.remainTime = buff.remainTime;
			savableAbility.stack = buff.stack;
			_character.ability.Add(savableAbility.ability);
		}
		_abilities[name] = savableAbility;
	}
}
