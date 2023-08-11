using Level;
using UnityEngine.AddressableAssets;

namespace GameResources;

public sealed class MapRequest : Request<Map>
{
	public MapRequest(AssetReference reference)
		: base(reference)
	{
	}
}
