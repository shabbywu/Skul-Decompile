using System;
using Characters.Gear.Quintessences;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class Specter : Ability
{
	public class Instance : AbilityInstance<Specter>
	{
		private Quintessence _quintessence;

		public Instance(Character owner, Specter ability, Quintessence quintessence)
			: base(owner, ability)
		{
			_quintessence = quintessence;
		}

		protected override void OnAttach()
		{
			base.remainTime = ability.duration;
		}

		protected override void OnDetach()
		{
			if (owner.health.dead)
			{
				Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.quintessence.items[0].cooldown.time.remainTime = 0f;
			}
		}

		public override void Refresh()
		{
			base.remainTime = ability.duration;
		}
	}

	[SerializeField]
	private Quintessence _quintessence;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this, _quintessence);
	}
}
