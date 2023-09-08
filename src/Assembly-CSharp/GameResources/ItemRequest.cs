using Characters.Gear.Items;

namespace GameResources;

public sealed class ItemRequest : Request<Item>
{
	public ItemRequest(string path)
		: base(path)
	{
	}
}
