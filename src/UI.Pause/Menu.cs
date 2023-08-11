using System;
using System.Runtime.CompilerServices;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Pause;

public class Menu : Dialogue
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static UnityAction _003C_003E9__8_4;

		internal void _003CAwake_003Eb__8_4()
		{
			Scene<GameBase>.instance.uiManager.confirm.Open(Localization.GetLocalizedString("label/pause/menu/quit/confirm"), (Action)Application.Quit);
		}
	}

	[SerializeField]
	private Panel _panel;

	[SerializeField]
	private Button _continue;

	[SerializeField]
	private Button _newGame;

	[SerializeField]
	private Button _controls;

	[SerializeField]
	private Button _settings;

	[SerializeField]
	private Button _quit;

	public override bool closeWithPauseKey => false;

	private void Awake()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		((UnityEvent)_continue.onClick).AddListener((UnityAction)delegate
		{
			_panel.Close();
		});
		((UnityEvent)_newGame.onClick).AddListener((UnityAction)delegate
		{
			Scene<GameBase>.instance.uiManager.confirm.Open(Localization.GetLocalizedString("label/pause/menu/newGame/confirm"), delegate
			{
				Singleton<Service>.Instance.fadeInOut.FadeOutImmediately();
				((Component)_panel).gameObject.SetActive(false);
				GameData.Generic.tutorial.Stop();
				Singleton<Service>.Instance.levelManager.ResetGame();
			});
		});
		((UnityEvent)_controls.onClick).AddListener((UnityAction)delegate
		{
			_panel.state = Panel.State.Controls;
		});
		((UnityEvent)_settings.onClick).AddListener((UnityAction)delegate
		{
			_panel.state = Panel.State.Settings;
		});
		ButtonClickedEvent onClick = _quit.onClick;
		object obj = _003C_003Ec._003C_003E9__8_4;
		if (obj == null)
		{
			UnityAction val = delegate
			{
				Scene<GameBase>.instance.uiManager.confirm.Open(Localization.GetLocalizedString("label/pause/menu/quit/confirm"), (Action)Application.Quit);
			};
			_003C_003Ec._003C_003E9__8_4 = val;
			obj = (object)val;
		}
		((UnityEvent)onClick).AddListener((UnityAction)obj);
	}
}
