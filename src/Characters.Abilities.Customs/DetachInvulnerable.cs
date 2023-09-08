using System;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class DetachInvulnerable : Ability
{
	public class Instance : AbilityInstance<DetachInvulnerable>
	{
		public Instance(Character owner, DetachInvulnerable ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.invulnerable.Detach(ability._key);
		}

		protected override void OnDetach()
		{
			owner.invulnerable.Attach(ability._key);
		}
	}

	[SerializeField]
	private Transform _key;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
