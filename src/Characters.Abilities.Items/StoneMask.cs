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
			((Sum<double>)(object)GameData.Currency.currencies[ability._type].multiplier).Remove((object)this);
		}

		private void UpdateMultiplier(Weapon old, Weapon @new)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			((Sum<double>)(object)GameData.Currency.currencies[ability._type].multiplier).Remove((object)this);
			Rarity rarity = _inventory.weapons[0].rarity;
			float num = ((EnumArray<Rarity, float>)ability._percentByRarity)[rarity];
			if ((Object)(object)_inventory.weapons[1] != (Object)null)
			{
				Rarity rarity2 = _inventory.weapons[1].rarity;
				if (rarity2 > rarity)
				{
					num = ((EnumArray<Rarity, float>)ability._percentByRarity)[rarity2];
				}
			}
			((Sum<double>)(object)GameData.Currency.currencies[ability._type].multiplier).AddOrUpdate((object)this, (double)num);
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
