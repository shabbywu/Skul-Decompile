using System;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;

namespace Characters.Abilities.Upgrades;

[Serializable]
public class LazinessOfGenius : Ability
{
	public class Instance : AbilityInstance<LazinessOfGenius>
	{
		public Instance(Character owner, LazinessOfGenius ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			PlayerComponents playerComponents = owner.playerComponents;
			if (playerComponents != null)
			{
				playerComponents.inventory.onUpdatedKeywordCounts += HandleOnUpdatedKeywordCounts;
			}
		}

		private void HandleOnUpdatedKeywordCounts()
		{
			PlayerComponents playerComponents = owner.playerComponents;
			if (playerComponents == null)
			{
				return;
			}
			foreach (Inscription.Key key in Inscription.keys)
			{
				ref int count = ref playerComponents.inventory.synergy.inscriptions[key].count;
				if (count == 1)
				{
					count++;
				}
			}
		}

		protected override void OnDetach()
		{
			PlayerComponents playerComponents = owner.playerComponents;
			if (playerComponents != null)
			{
				playerComponents.inventory.onUpdatedKeywordCounts -= HandleOnUpdatedKeywordCounts;
			}
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
