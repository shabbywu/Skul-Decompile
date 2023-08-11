using System.Collections;
using InControl;
using Platforms;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace Scenes;

public class Main : Scene<Main>
{
	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private AudioClip _backgroundMusic;

	[SerializeField]
	private GameObject _loading;

	[SerializeField]
	private GameObject _gameLogo;

	[SerializeField]
	private GameObject _pressAnyKey;

	[SerializeField]
	private TMP_Text _username;

	[SerializeField]
	private GameObject _versionName;

	[SerializeField]
	private int _gameBaseSceneNumber;

	private void Awake()
	{
		_loading.SetActive(false);
		_gameLogo.SetActive(false);
		_pressAnyKey.SetActive(false);
		_username.text = PersistentSingleton<PlatformManager>.Instance.platform.userName;
	}

	private void Start()
	{
		PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(_backgroundMusic);
	}

	public void LogoAnimationsCompleted()
	{
		((MonoBehaviour)this).StartCoroutine(CStartGameOnReady());
	}

	private IEnumerator CStartGameOnReady()
	{
		yield return CWaitForBackgroundFadeIn();
		yield return CWaitForInput();
		_loading.SetActive(true);
		StartGame();
	}

	private IEnumerator CWaitForBackgroundFadeIn()
	{
		_gameLogo.SetActive(true);
		((Component)_username).gameObject.SetActive(true);
		_versionName.SetActive(true);
		yield return (object)new WaitForSeconds(1f);
	}

	private IEnumerator CWaitForInput()
	{
		GameResourceLoader.instance.WaitForStrings();
		yield return null;
		_pressAnyKey.SetActive(true);
		while (!Input.anyKey && !InputManager.ActiveDevice.AnyButtonIsPressed)
		{
			yield return null;
		}
		yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
		_pressAnyKey.SetActive(false);
	}

	private void StartGame()
	{
		Singleton<Service>.Instance.fadeInOut.ShowLoadingIcon();
		GameResourceLoader.instance.WaitForCompletion();
		Singleton<Service>.Instance.levelManager.LoadGame();
		Object.Destroy((Object)(object)_container);
	}
}
