using Characters.Gear.Items;
using Level;
using UnityEngine;

namespace Characters.Operations.Items;

public class Drop : CharacterOperation
{
	[SerializeField]
	private Item _item;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Item item = Object.Instantiate<Item>(_item, ((Component)owner).transform.position, Quaternion.identity);
		((Object)item).name = ((Object)_item).name;
		((Component)item).transform.parent = ((Component)Map.Instance).transform;
		item.Initialize();
	}
}
