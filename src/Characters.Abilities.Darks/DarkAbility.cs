using System;
using GameResources;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class DarkAbility : MonoBehaviour
{
	[SerializeField]
	private DarkAbilityGauge _gauge;

	[SerializeField]
	private Key[] _exceptTarget;

	[SerializeField]
	private AbilityComponent[] _abilityComponents;

	public string displayName => Localization.GetLocalizedString("darkEnemy/ability/" + ((Object)((Component)this).gameObject).name + "/name");

	public void Initialize()
	{
		AbilityComponent[] abilityComponents = _abilityComponents;
		for (int i = 0; i < abilityComponents.Length; i++)
		{
			abilityComponents[i].Initialize();
		}
	}

	public bool Available(Character owner)
	{
		Key[] exceptTarget = _exceptTarget;
		for (int i = 0; i < exceptTarget.Length; i++)
		{
			Key key = exceptTarget[i];
			if (key.Equals(owner.key))
			{
				return false;
			}
		}
		return true;
	}

	public void AttachTo(Character owner, DarkAbilityAttacher attacher)
	{
		AbilityComponent[] abilityComponents = _abilityComponents;
		foreach (AbilityComponent abilityComponent in abilityComponents)
		{
			owner.ability.Add(abilityComponent.ability);
		}
		attacher.gauge = _gauge;
	}

	public void RemoveFrom(Character owner)
	{
		AbilityComponent[] abilityComponents = _abilityComponents;
		foreach (AbilityComponent abilityComponent in abilityComponents)
		{
			owner.ability.Remove(abilityComponent.ability);
		}
	}
}
