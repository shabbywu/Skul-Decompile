using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class ModifyStatusDuration : Ability
{
	public sealed class Instance : AbilityInstance<ModifyStatusDuration>
	{
		public Instance(Character owner, ModifyStatusDuration ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.durationMultiplier[ability._kind].AddOrUpdate(this, ability._multiplier);
		}

		protected override void OnDetach()
		{
			owner.status.durationMultiplier[ability._kind].Remove(this);
		}
	}

	[SerializeField]
	private CharacterStatus.Kind _kind;

	[SerializeField]
	private float _multiplier;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
