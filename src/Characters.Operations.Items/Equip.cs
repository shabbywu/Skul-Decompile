using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Operations.Items;

public class Equip : CharacterOperation
{
	[SerializeField]
	private Item _item;

	public override void Run(Character owner)
	{
		Debug.Log((object)owner.playerComponents.inventory.item.TryEquip(_item.Instantiate()));
	}
}
