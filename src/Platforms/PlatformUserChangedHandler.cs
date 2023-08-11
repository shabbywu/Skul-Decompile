using Data;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platforms;

public class PlatformUserChangedHandler : MonoBehaviour
{
	private const string _mainSceneName = "Main";

	[SerializeField]
	private PlatformManager _platformManager;

	private void Start()
	{
		_platformManager.onUserChanged += OnUserChanged;
	}

	private void OnUserChanged()
	{
		SceneManager.LoadScene("Main");
		Singleton<Service>.Instance.levelManager.DestroyPlayer();
		Singleton<Service>.Instance.levelManager.ClearEvents();
		PoolObject.DespawnAll();
		PoolObject.Clear();
		GameData.Initialize();
		GameResourceLoader.instance.PreloadSavedGear();
	}
}
