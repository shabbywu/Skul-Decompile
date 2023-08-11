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
			((Sum<float>)(object)owner.status.durationMultiplier[ability._kind]).AddOrUpdate((object)this, ability._multiplier);
		}

		protected override void OnDetach()
		{
			((Sum<float>)(object)owner.status.durationMultiplier[ability._kind]).Remove((object)this);
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
