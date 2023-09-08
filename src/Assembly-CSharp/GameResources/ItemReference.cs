using System;
using Characters.Gear;
using Characters.Gear.Synergy.Inscriptions;

namespace GameResources;

[Serializable]
public class ItemReference : GearReference
{
	public Inscription.Key prefabKeyword1;

	public Inscription.Key prefabKeyword2;

	public override Gear.Type type => Gear.Type.Item;

	public new ItemRequest LoadAsync()
	{
		return new ItemRequest(path);
	}
}
