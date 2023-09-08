using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu]
public class SettingsList : ScriptableObject
{
	[SerializeField]
	private AssetReference[] _settings;

	private void Awake()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (Application.isPlaying)
		{
			Addressables.LoadAssetsAsync<Object>((object)_settings, (Action<Object>)OnLoaded);
		}
	}

	private void OnLoaded(Object @object)
	{
	}
}
