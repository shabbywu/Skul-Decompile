using System;
using Characters.Gear;

namespace GameResources;

[Serializable]
public class EssenceReference : GearReference
{
	public override Gear.Type type => Gear.Type.Quintessence;

	public new EssenceRequest LoadAsync()
	{
		return new EssenceRequest(path);
	}
}
