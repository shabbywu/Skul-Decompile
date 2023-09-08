using System;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers;

public sealed class EqualWeaponName : Trigger
{
	[SerializeField]
	private string _weaponName;

	protected override bool Check()
	{
		Weapon polymorphOrCurrent = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.polymorphOrCurrent;
		if ((Object)(object)polymorphOrCurrent == (Object)null)
		{
			return false;
		}
		if (((Object)polymorphOrCurrent).name.Equals(_weaponName, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}
}
