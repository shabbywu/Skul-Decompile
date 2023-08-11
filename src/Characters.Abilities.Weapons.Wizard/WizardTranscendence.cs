using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Wizard;

[Serializable]
public sealed class WizardTranscendence : Ability
{
	public class Instance : AbilityInstance<WizardTranscendence>
	{
		public Instance(Character owner, WizardTranscendence ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			ability._passive.transcendence = true;
		}

		protected override void OnDetach()
		{
			ability._passive.transcendence = false;
		}
	}

	[SerializeField]
	private WizardPassiveComponent _passive;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
