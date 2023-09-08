using Scenes;
using UnityEngine;

namespace Platforms;

public class PlatformPauseHandler : MonoBehaviour
{
	[SerializeField]
	private PlatformManager _platformManager;

	private void Start()
	{
		_platformManager.onPause += OnPause;
		_platformManager.onResume += OnResume;
	}

	private void OnPause()
	{
		if ((Object)(object)Scene<GameBase>.instance != (Object)null)
		{
			Scene<GameBase>.instance.uiManager.ShowPausePopup();
		}
		Chronometer.global.AttachTimeScale(this, 0f);
	}

	private void OnResume()
	{
		Chronometer.global.DetachTimeScale(this);
	}
}
