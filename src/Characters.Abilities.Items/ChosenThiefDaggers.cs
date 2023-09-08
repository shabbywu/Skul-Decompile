using System;
using System.Collections.Generic;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class ChosenThiefDaggers : Ability
{
	public sealed class Instance : AbilityInstance<ChosenThiefDaggers>
	{
		private HashSet<Character> _targets;

		public Instance(Character owner, ChosenThiefDaggers ability)
			: base(owner, ability)
		{
			_targets = new HashSet<Character>();
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(int.MaxValue, HandleOnGiveDamage);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			Singleton<Service>.Instance.levelManager.onMapLoaded += LevelManager_onMapLoaded;
		}

		private void LevelManager_onMapLoaded()
		{
			_targets.Clear();
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			Character character = target.character;
			if ((Object)(object)character == (Object)null || !_targets.Contains(character))
			{
				return;
			}
			_targets.Remove(character);
			if (damageDealt != 0.0)
			{
				owner.ability.Add(ability._ownerAbility.ability);
				AbilityComponent[] components = ability._targetAbility.components;
				foreach (AbilityComponent abilityComponent in components)
				{
					character.ability.Add(abilityComponent.ability);
				}
			}
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			if ((Object)(object)target.character == (Object)null)
			{
				return false;
			}
			if (target.character.health.percent < 1.0)
			{
				return false;
			}
			if (!_targets.Contains(target.character))
			{
				_targets.Add(target.character);
			}
			return false;
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			Singleton<Service>.Instance.levelManager.onMapLoaded -= LevelManager_onMapLoaded;
			owner.ability.Remove(ability._ownerAbility.ability);
		}
	}

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _ownerAbility;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent.Subcomponents _targetAbility;

	public override void Initialize()
	{
		base.Initialize();
		_ownerAbility.Initialize();
		_targetAbility.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
