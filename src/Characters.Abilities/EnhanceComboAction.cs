using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class EnhanceComboAction : Ability
{
	public class Instance : AbilityInstance<EnhanceComboAction>
	{
		public Instance(Character owner, EnhanceComboAction ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			ability._enhanceableComboAction.enhanced = true;
		}

		protected override void OnDetach()
		{
			ability._enhanceableComboAction.enhanced = false;
		}
	}

	[SerializeField]
	private EnhanceableComboAction _enhanceableComboAction;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
