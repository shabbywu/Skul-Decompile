using System;
using Characters.Gear;
using Characters.Gear.Weapons;

namespace GameResources;

[Serializable]
public class WeaponReference : GearReference
{
	public Weapon.Category category;

	public override Gear.Type type => Gear.Type.Weapon;

	public new WeaponRequest LoadAsync()
	{
		return new WeaponRequest(path);
	}
}
