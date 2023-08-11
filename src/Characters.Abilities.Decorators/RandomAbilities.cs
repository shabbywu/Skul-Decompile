using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Abilities.Decorators;

[Serializable]
public sealed class RandomAbilities : Ability
{
	public sealed class Instance : AbilityInstance<RandomAbilities>
	{
		private AbilityComponent _target;

		public Instance(Character owner, RandomAbilities ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_target = ExtensionMethods.Random<AbilityComponent>((IEnumerable<AbilityComponent>)((SubcomponentArray<AbilityComponent>)ability._abilityComponents).components);
			owner.ability.Add(_target.ability);
		}

		public override void Refresh()
		{
			base.Refresh();
			if (ability._removeAbilityOnRefresh)
			{
				owner.ability.Remove(_target.ability);
			}
			_target = ExtensionMethods.Random<AbilityComponent>((IEnumerable<AbilityComponent>)((SubcomponentArray<AbilityComponent>)ability._abilityComponents).components);
			owner.ability.Add(_target.ability);
		}

		protected override void OnDetach()
		{
			owner.ability.Remove(_target.ability);
		}
	}

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent.Subcomponents _abilityComponents;

	[SerializeField]
	private bool _removeAbilityOnRefresh;

	public override void Initialize()
	{
		base.Initialize();
		_abilityComponents.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
