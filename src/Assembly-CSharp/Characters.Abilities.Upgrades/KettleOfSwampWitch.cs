using System;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;

namespace Characters.Abilities.Upgrades;

[Serializable]
public class KettleOfSwampWitch : Ability
{
	public class Instance : AbilityInstance<KettleOfSwampWitch>
	{
		private readonly Inscription.Key[] _statusKeys = new Inscription.Key[5]
		{
			Inscription.Key.AbsoluteZero,
			Inscription.Key.Arson,
			Inscription.Key.Dizziness,
			Inscription.Key.ExcessiveBleeding,
			Inscription.Key.Poisoning
		};

		public Instance(Character owner, KettleOfSwampWitch ability)
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
			foreach (Inscription.Key key2 in Inscription.keys)
			{
				bool flag = false;
				Inscription.Key[] statusKeys = _statusKeys;
				foreach (Inscription.Key key in statusKeys)
				{
					if (key2 == key)
					{
						flag = true;
						break;
					}
				}
				ref int count = ref playerComponents.inventory.synergy.inscriptions[key2].count;
				if (count >= 1 && flag)
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
