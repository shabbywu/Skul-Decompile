using Characters.Gear.Quintessences;

namespace GameResources;

public sealed class EssenceRequest : Request<Quintessence>
{
	public EssenceRequest(string path)
		: base(path)
	{
	}
}
