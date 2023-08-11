using Characters.Gear;

namespace GameResources;

public sealed class GearRequest : Request<Gear>
{
	public GearRequest(string path)
		: base(path)
	{
	}
}
