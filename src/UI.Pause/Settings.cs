using System.Collections.Generic;
using System.Linq;
using CutScenes;
using Data;
using GameResources;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace UI.Pause;

public class Settings : Dialogue
{
	public const string key = "label/pause/settings";

	[SerializeField]
	private Panel _panel;

	[SerializeField]
	[Header("Graphics")]
	private Selection _resolution;

	[SerializeField]
	private Selection _screen;

	[SerializeField]
	private Selection _light;

	[SerializeField]
	private Selection _particleQuality;

	[SerializeField]
	private Slider _cameraShake;

	[SerializeField]
	private Slider _vibrationPower;

	[Header("Audio")]
	[Space]
	[SerializeField]
	private Slider _master;

	[SerializeField]
	private Slider _music;

	[SerializeField]
	private Slider _sfx;

	[SerializeField]
	[Space]
	[Header("Data")]
	private Button _resetData;

	[SerializeField]
	private Confirm _resetDataConfirm;

	[SerializeField]
	private Button _resetCutsceneData;

	[SerializeField]
	private Confirm _resetCutsceneDataConfirm;

	[SerializeField]
	[Space]
	[Header("Game Play")]
	private Selection _language;

	[SerializeField]
	private Selection _easyMode;

	[SerializeField]
	private Confirm _easyModeConfirm;

	[SerializeField]
	private PointerDownHandler _left;

	[SerializeField]
	private PointerDownHandler _right;

	[SerializeField]
	private Selection _showTimer;

	[SerializeField]
	private Selection _showUI;

	[SerializeField]
	[Space]
	private Button _return;

	private List<Resolution> _resolutionList;

	public override bool closeWithPauseKey => false;

	private void Awake()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		LoadStrings();
		InitializeGraphicsOptions();
		InitializeAudioOptions();
		InitializeDataOptions();
		InitializeGameplayOptions();
		((UnityEvent)_return.onClick).AddListener((UnityAction)delegate
		{
			_panel.state = Panel.State.Menu;
		});
	}

	protected override void OnEnable()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		base.OnEnable();
		LoadStrings();
		FullScreenMode fullScreenMode = Screen.fullScreenMode;
		switch ((int)fullScreenMode)
		{
		case 1:
			_screen.value = 0;
			break;
		case 0:
			_screen.value = 1;
			break;
		case 3:
			_screen.value = 2;
			break;
		}
		UpdateEasyMode();
		SetDefaultFocus();
	}

	private void SetDefaultFocus()
	{
		Focus((Selectable)(object)_resolution);
	}

	private void InitializeGraphicsOptions()
	{
		InitializeResolutionOption();
		_light.value = (GameData.Settings.lightEnabled ? 1 : 0);
		_light.onValueChanged += delegate(int v)
		{
			Light2D.lightEnabled = (GameData.Settings.lightEnabled = v == 1);
		};
	}

	private void InitializeResolutionOption()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		_resolutionList = new List<Resolution>();
		Resolution[] resolutions = Screen.resolutions;
		Resolution val2;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution val = resolutions[i];
			bool flag = false;
			for (int j = 0; j < _resolutionList.Count; j++)
			{
				int width = ((Resolution)(ref val)).width;
				val2 = _resolutionList[j];
				if (width != ((Resolution)(ref val2)).width)
				{
					continue;
				}
				int height = ((Resolution)(ref val)).height;
				val2 = _resolutionList[j];
				if (height == ((Resolution)(ref val2)).height)
				{
					flag = true;
					int refreshRate = ((Resolution)(ref val)).refreshRate;
					val2 = _resolutionList[j];
					if (refreshRate > ((Resolution)(ref val2)).refreshRate)
					{
						_resolutionList[j] = val;
					}
				}
			}
			if (!flag)
			{
				_resolutionList.Add(val);
			}
		}
		_resolution.SetTexts(_resolutionList.Select((Resolution r) => $"{((Resolution)(ref r)).width} x {((Resolution)(ref r)).height}").ToArray());
		int num = -1;
		for (int k = 0; k < _resolutionList.Count; k++)
		{
			val2 = _resolutionList[k];
			if (((Resolution)(ref val2)).width == Screen.width)
			{
				val2 = _resolutionList[k];
				if (((Resolution)(ref val2)).height == Screen.height)
				{
					num = k;
				}
			}
		}
		if (num == -1)
		{
			Resolution item = default(Resolution);
			((Resolution)(ref item)).width = Screen.width;
			((Resolution)(ref item)).height = Screen.height;
			val2 = Screen.currentResolution;
			((Resolution)(ref item)).refreshRate = ((Resolution)(ref val2)).refreshRate;
			_resolutionList.Add(item);
			num = _resolutionList.Count - 1;
		}
		_resolution.value = num;
	}

	private void InitializeAudioOptions()
	{
		_master.value = GameData.Settings.masterVolume;
		((UnityEvent<float>)(object)_master.onValueChanged).AddListener((UnityAction<float>)delegate(float v)
		{
			GameData.Settings.masterVolume = v;
			PersistentSingleton<SoundManager>.Instance.UpdateMusicVolume();
		});
		_music.value = GameData.Settings.musicVolume;
		((UnityEvent<float>)(object)_music.onValueChanged).AddListener((UnityAction<float>)delegate(float v)
		{
			GameData.Settings.musicVolume = v;
			PersistentSingleton<SoundManager>.Instance.UpdateMusicVolume();
		});
		_sfx.value = GameData.Settings.sfxVolume;
		((UnityEvent<float>)(object)_sfx.onValueChanged).AddListener((UnityAction<float>)delegate(float v)
		{
			GameData.Settings.sfxVolume = v;
		});
	}

	private void InitializeDataOptions()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		((UnityEvent)_resetData.onClick).AddListener((UnityAction)delegate
		{
			_resetDataConfirm.Open(string.Empty, delegate
			{
				GameData.Generic.ResetAll();
				GameData.Currency.ResetAll();
				GameData.Progress.ResetAll();
				GameData.Gear.ResetAll();
				GameData.Save.instance.ResetAll();
				GameData.HardmodeProgress.ResetAll();
				Singleton<Service>.Instance.levelManager.player.playerComponents.savableAbilityManager.ResetAll();
				_panel.ReturnToTitleScreen();
				Focus((Selectable)(object)_resetData);
			}, delegate
			{
				Focus((Selectable)(object)_resetData);
			});
		});
		((UnityEvent)_resetCutsceneData.onClick).AddListener((UnityAction)delegate
		{
			_resetCutsceneDataConfirm.Open(string.Empty, delegate
			{
				foreach (Key value in EnumValues<Key>.Values)
				{
					if (value != Key.DarkMirrorArachne_건강보조장치 && value != Key.darkMirrorCollector_품목순환장치 && value != Key.각인합성장치_Intro && value != Key.행운계측기_Intro && value != Key.darkMirrorBlackMarket)
					{
						GameData.Progress.cutscene.SetData(value, value: false);
					}
				}
				GameData.Progress.skulstory.ResetAll();
				GameData.Progress.cutscene.SaveAll();
				GameData.Progress.skulstory.SaveAll();
				Focus((Selectable)(object)_resetCutsceneData);
			}, delegate
			{
				Focus((Selectable)(object)_resetCutsceneData);
			});
		});
	}

	private void InitializeGameplayOptions()
	{
		_language.value = GameData.Settings.language;
		_language.onValueChanged += delegate(int v)
		{
			GameData.Settings.language = v;
		};
		_cameraShake.value = GameData.Settings.cameraShakeIntensity;
		((UnityEvent<float>)(object)_cameraShake.onValueChanged).AddListener((UnityAction<float>)delegate(float v)
		{
			GameData.Settings.cameraShakeIntensity = v;
		});
		_vibrationPower.value = GameData.Settings.vibrationIntensity;
		((UnityEvent<float>)(object)_vibrationPower.onValueChanged).AddListener((UnityAction<float>)delegate(float v)
		{
			GameData.Settings.vibrationIntensity = v;
		});
		_particleQuality.value = GameData.Settings.particleQuality;
		_particleQuality.onValueChanged += delegate(int v)
		{
			GameData.Settings.particleQuality = v;
		};
		_easyMode.value = (GameData.Settings.easyMode ? 1 : 0);
		_easyMode.onValueChanged += delegate(int v)
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				_easyMode.SetValueWithoutNotify(0);
			}
			else if (v == 1)
			{
				_easyModeConfirm.Open(string.Empty, delegate
				{
					GameData.Settings.easyMode = true;
					ExtensionMethods.Set((Type)1);
					Focus((Selectable)(object)_easyMode);
				}, delegate
				{
					_easyMode.SetValueWithoutNotify(0);
					EventSystem.current.SetSelectedGameObject((GameObject)null);
					Focus((Selectable)(object)_easyMode);
				});
			}
			else
			{
				GameData.Settings.easyMode = false;
			}
		};
		_showTimer.value = (GameData.Settings.showTimer ? 1 : 0);
		_showTimer.onValueChanged += delegate(int v)
		{
			GameData.Settings.showTimer = v == 1;
		};
		_showUI.value = (int)Scene<GameBase>.instance.uiManager.hideOption;
		_showUI.onValueChanged += delegate(int v)
		{
			Scene<GameBase>.instance.uiManager.SetHideOption((UIManager.HideOption)v);
		};
	}

	private void LoadStrings()
	{
		_light.SetTexts(Localization.GetLocalizedStrings("label/pause/settings/off", "label/pause/settings/on"));
		string text = "label/pause/settings/graphics/screen";
		_screen.SetTexts(Localization.GetLocalizedStrings(text + "/borderless", text + "/fullscreen", text + "/windowed"));
		_language.SetTexts(Localization.nativeNames.ToArray());
		_particleQuality.SetTexts(Localization.GetLocalizedStrings("label/pause/settings/off", "label/pause/settings/low", "label/pause/settings/medium", "label/pause/settings/high"));
		_easyMode.SetTexts(Localization.GetLocalizedStrings("label/pause/settings/off", "label/pause/settings/on"));
		_showTimer.SetTexts(Localization.GetLocalizedStrings("label/pause/settings/off", "label/pause/settings/on"));
		_showUI.SetTexts(Localization.GetLocalizedStrings("label/pause/settings/gamePlay/showUI/all", "label/pause/settings/gamePlay/showUI/hideHUD", "label/pause/settings/gamePlay/showUI/hideAll"));
	}

	private void UpdateEasyMode()
	{
		if (GameData.HardmodeProgress.hardmode)
		{
			((Behaviour)_left).enabled = false;
			((Behaviour)_right).enabled = false;
			((Selectable)_easyMode).interactable = false;
			_easyMode.value = 0;
		}
		else
		{
			((Behaviour)_left).enabled = true;
			((Behaviour)_right).enabled = true;
			((Selectable)_easyMode).interactable = true;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		ApplyDisplayOptions();
		GameData.Settings.Save();
		PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
	}

	private void ApplyDisplayOptions()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		Resolution val = _resolutionList[_resolution.value];
		FullScreenMode val2 = (FullScreenMode)1;
		switch (_screen.value)
		{
		case 0:
			val2 = (FullScreenMode)1;
			break;
		case 1:
			val2 = (FullScreenMode)0;
			break;
		case 2:
			val2 = (FullScreenMode)3;
			break;
		}
		Screen.SetResolution(((Resolution)(ref val)).width, ((Resolution)(ref val)).height, val2, ((Resolution)(ref val)).refreshRate);
	}
}
