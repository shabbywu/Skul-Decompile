using System;
using Level;
using UnityEngine.AddressableAssets;

namespace GameResources;

[Serializable]
public class MapReference
{
	public Map.Type type;

	public SpecialMap.Type specialMapType;

	[NonSerialized]
	public bool darkEnemy;

	public AssetReference reference;

	public string path;

	public bool empty => string.IsNullOrWhiteSpace(reference.AssetGUID);

	public MapReference()
	{
	}

	public MapReference(AssetReference reference)
	{
		this.reference = reference;
	}

	public MapRequest LoadAsync()
	{
		return new MapRequest(reference);
	}

	public Map Load()
	{
		throw new NotImplementedException("어드레서블 이용하도록 바꿔야함");
	}

	public static MapReference FromPath(string path)
	{
		return null;
	}
}
