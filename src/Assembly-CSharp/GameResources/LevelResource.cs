using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources;

public class LevelResource : ScriptableObject
{
	[SerializeField]
	private AssetReference[] _chapters;

	public static LevelResource instance { get; private set; }

	public AssetReference[] chapters => _chapters;

	public void Initialize()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		instance = this;
		((Object)this).hideFlags = (HideFlags)(((Object)this).hideFlags | 0x20);
		for (int i = 1; i < _chapters.Length; i++)
		{
		}
	}

	public AssetReference GetChapter(int index)
	{
		return _chapters[index];
	}
}
