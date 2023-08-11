using System;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.Abilities.Constraints;
using UnityEngine;

namespace Characters;

public class CharacterAbilityManager : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Character _character;

	private Constraint[] _constraints = new Constraint[4]
	{
		new LetterBox(),
		new Dialogue(),
		new Story(),
		new Characters.Abilities.Constraints.EndingCredit()
	};

	private readonly PriorityList<IAbilityInstance> _abilities = new PriorityList<IAbilityInstance>();

	public int Count => _abilities.Count;

	public IAbilityInstance this[int index] => _abilities[index];

	public static CharacterAbilityManager AddComponent(Character character)
	{
		CharacterAbilityManager characterAbilityManager = ((Component)character).gameObject.AddComponent<CharacterAbilityManager>();
		characterAbilityManager._character = character;
		characterAbilityManager.Initialize();
		return characterAbilityManager;
	}

	public IAbilityInstance Add(IAbility ability)
	{
		IAbilityInstance instance = GetInstance(ability);
		if (instance != null)
		{
			instance.Refresh();
			return instance;
		}
		instance = ability.CreateInstance(_character);
		if (ability.removeOnSwapWeapon && (Object)(object)_character.playerComponents.inventory.weapon != (Object)null)
		{
			_character.playerComponents.inventory.weapon.onSwap += RemoveAbilityOnSwap;
		}
		if (_character.liveAndActive)
		{
			instance.Attach();
			_abilities.Add(ability.iconPriority, instance);
		}
		return instance;
		void RemoveAbilityOnSwap()
		{
			_character.playerComponents.inventory.weapon.onSwap -= RemoveAbilityOnSwap;
			_character.ability.Remove(ability);
		}
	}

	public bool Remove(IAbility ability)
	{
		for (int num = _abilities.Count - 1; num >= 0; num--)
		{
			if (_abilities[num].ability.Equals(ability))
			{
				_abilities[num].Detach();
				_abilities.RemoveAt(num);
				return true;
			}
		}
		return false;
	}

	public bool Remove(AbilityInstance abilityInstance)
	{
		for (int num = _abilities.Count - 1; num >= 0; num--)
		{
			if (_abilities[num].Equals(abilityInstance))
			{
				_abilities[num].Detach();
				_abilities.RemoveAt(num);
				return true;
			}
		}
		return false;
	}

	public void Clear()
	{
		for (int num = _abilities.Count - 1; num >= 0; num--)
		{
			_abilities[num].Detach();
		}
		_abilities.Clear();
	}

	public IAbilityInstance GetInstance(IAbility ability)
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].ability.Equals(ability))
			{
				return _abilities[i];
			}
		}
		return null;
	}

	public IAbilityInstance GetInstanceType(IAbility ability)
	{
		Type type = ability.GetType();
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].ability.GetType() == type)
			{
				return _abilities[i];
			}
		}
		return null;
	}

	public IAbilityInstance GetInstance<T>() where T : IAbility
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].ability is T)
			{
				return _abilities[i];
			}
		}
		return null;
	}

	public T GetInstanceByInstanceType<T>() where T : IAbilityInstance
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			IAbilityInstance abilityInstance = _abilities[i];
			if (abilityInstance is T)
			{
				return (T)abilityInstance;
			}
		}
		return default(T);
	}

	public List<T> GetInstancesByInstanceType<T>() where T : IAbilityInstance
	{
		List<T> list = new List<T>();
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i] is T item)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public bool Contains(IAbility ability)
	{
		for (int i = 0; i < _abilities.Count; i++)
		{
			if (_abilities[i].ability.Equals(ability))
			{
				return true;
			}
		}
		return false;
	}

	private void Initialize()
	{
		if ((Object)(object)_character.health != (Object)null)
		{
			_character.health.onDied += OnDied;
		}
	}

	private void OnDied()
	{
		Clear();
		if ((Object)(object)_character.health != (Object)null)
		{
			_character.health.onDied -= OnDied;
		}
	}

	private void Update()
	{
		if (_character.playerComponents != null && !_constraints.Pass())
		{
			return;
		}
		try
		{
			float deltaTime = ((ChronometerBase)_character.chronometer.master).deltaTime;
			int num = _abilities.Count - 1;
			while (num >= 0 && num < _abilities.Count)
			{
				IAbilityInstance abilityInstance = _abilities[num];
				abilityInstance.UpdateTime(deltaTime);
				if (abilityInstance.expired)
				{
					if (_abilities[num] != abilityInstance)
					{
						break;
					}
					_abilities.RemoveAt(num);
					abilityInstance.Detach();
				}
				num--;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError((object)ex);
		}
	}
}
