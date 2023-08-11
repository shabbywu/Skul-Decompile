using System;
using Characters.Abilities.CharacterStat;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class VictoryBaton : Ability
{
	public class Instance : AbilityInstance<VictoryBaton>
	{
		public Instance(Character owner, VictoryBaton ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.ability.Add(ability._statBonusComponent.ability);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}

		private void HandleOnKilled(ITarget target, ref Damage damage)
		{
			base.remainTime = ability.duration;
		}

		protected override void OnDetach()
		{
			owner.ability.Remove(ability._statBonusComponent.ability);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}
	}

	[SerializeField]
	[Subcomponent(typeof(StatBonusComponent))]
	private StatBonusComponent _statBonusComponent;

	public override void Initialize()
	{
		base.Initialize();
		_statBonusComponent.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
