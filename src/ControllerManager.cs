using Data;
using InControl;
using Scenes;
using UnityEngine;
using UserInput;

public class ControllerManager : MonoBehaviour
{
	public readonly MaxOnlyTimedFloats vibration = new MaxOnlyTimedFloats();

	[SerializeField]
	private GameObject _disconnedtedPopup;

	private void Start()
	{
		KeyMapper.Bind(GameData.Settings.keyBindings);
	}

	private void OnEnable()
	{
		InputManager.OnActiveDeviceChanged += OnActiveDeviceChanged;
		InputManager.OnDeviceAttached += OnDeviceAttached;
		InputManager.OnDeviceDetached += OnDeviceDetached;
	}

	private void OnDisable()
	{
		InputManager.OnActiveDeviceChanged -= OnActiveDeviceChanged;
		InputManager.OnDeviceAttached -= OnDeviceAttached;
		InputManager.OnDeviceDetached -= OnDeviceDetached;
	}

	private void Update()
	{
		vibration.Update();
		float num = vibration.value * Chronometer.global.timeScale * 10f * GameData.Settings.vibrationIntensity;
		InputManager.ActiveDevice.Vibrate(num);
	}

	private void OnActiveDeviceChanged(InputDevice inputDevice)
	{
		HideControllerDisconnedtedPopup();
		Stop();
	}

	public void Stop()
	{
		vibration.Clear();
		InputManager.ActiveDevice.StopVibration();
	}

	public void ShowControllerDisconnedtedPopup()
	{
		if (!_disconnedtedPopup.activeSelf)
		{
			Chronometer.global.AttachTimeScale(_disconnedtedPopup, 0f);
			_disconnedtedPopup.SetActive(true);
		}
	}

	public void HideControllerDisconnedtedPopup()
	{
		if (_disconnedtedPopup.activeSelf)
		{
			Chronometer.global.DetachTimeScale(_disconnedtedPopup);
			_disconnedtedPopup.SetActive(false);
			if (!((Object)(object)Scene<GameBase>.instance == (Object)null))
			{
				Scene<GameBase>.instance.uiManager.ShowPausePopup();
			}
		}
	}

	private void OnDeviceAttached(InputDevice inputDevice)
	{
		HideControllerDisconnedtedPopup();
	}

	private void OnDeviceDetached(InputDevice inputDevice)
	{
		if (InputManager.ActiveDevice == null || InputManager.ActiveDevice == inputDevice)
		{
			ShowControllerDisconnedtedPopup();
		}
	}
}
