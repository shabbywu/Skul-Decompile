using System;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class StoneMask : Ability
{
	public sealed class Instance : AbilityInstance<StoneMask>
	{
		private WeaponInventory _inventory;

		public Instance(Character owner, StoneMask ability)
			: base(owner, ability)
		{
			_inventory = owner.playerComponents.inventory.weapon;
		}

		protected override void OnAttach()
		{
			_inventory.onChanged += UpdateMultiplier;
			UpdateMultiplier(null, null);
		}

		protected override void OnDetach()
		{
			_inventory.onChanged -= UpdateMultiplier;
			GameData.Currency.currencies[ability._type].multiplier.Remove(this);
		}

		private void UpdateMultiplier(Weapon old, Weapon @new)
		{
			GameData.Currency.currencies[ability._type].multiplier.Remove(this);
			Rarity rarity = _inventory.weapons[0].rarity;
			float num = ability._percentByRarity[rarity];
			if ((Object)(object)_inventory.weapons[1] != (Object)null)
			{
				Rarity rarity2 = _inventory.weapons[1].rarity;
				if (rarity2 > rarity)
				{
					num = ability._percentByRarity[rarity2];
				}
			}
			GameData.Currency.currencies[ability._type].multiplier.AddOrUpdate(this, num);
		}
	}

	[Serializable]
	private class PercentByRarity : EnumArray<Rarity, float>
	{
	}

	[SerializeField]
	private GameData.Currency.Type _type;

	[SerializeField]
	private PercentByRarity _percentByRarity;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
