using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class GetEvasionShield : Ability
{
	public sealed class Instance : AbilityInstance<GetEvasionShield>
	{
		private int _remainAmount;

		public Instance(Character owner, GetEvasionShield ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_remainAmount = ability._amount;
			owner.evasion.Attach(this);
			owner.onEvade += OnEvade;
		}

		private void OnEvade(ref Damage damage)
		{
			ability._onEvade.Run(owner);
			_remainAmount--;
			if (_remainAmount <= 0)
			{
				owner.ability.Remove(this);
			}
		}

		protected override void OnDetach()
		{
			ability._onBroken.Run(owner);
			owner.evasion.Detach(this);
			owner.onEvade -= OnEvade;
		}
	}

	[SerializeField]
	private int _amount;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onEvade;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onBroken;

	public override void Initialize()
	{
		base.Initialize();
		_onEvade.Initialize();
		_onBroken.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
