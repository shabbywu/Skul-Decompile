using Characters.Gear.Weapons;

namespace GameResources;

public sealed class WeaponRequest : Request<Weapon>
{
	public WeaponRequest(string path)
		: base(path)
	{
	}
}
