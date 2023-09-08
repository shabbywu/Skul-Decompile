using System;
using System.Runtime.CompilerServices;
using Characters.Controllers;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Ending;

public class Panel : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static UnityAction _003C_003E9__7_0;

		public static UnityAction _003C_003E9__7_1;

		public static UnityAction _003C_003E9__7_2;

		public static UnityAction _003C_003E9__7_3;

		internal void _003CAwake_003Eb__7_0()
		{
			Application.OpenURL("https://tumblbug.com/skul");
		}

		internal void _003CAwake_003Eb__7_1()
		{
			Application.OpenURL("https://twitter.com/Skul_game");
		}

		internal void _003CAwake_003Eb__7_2()
		{
			Application.OpenURL("https://otrade.co/funding/334");
		}

		internal void _003CAwake_003Eb__7_3()
		{
			Application.OpenURL("https://skulthegame.tumblr.com/");
		}
	}

	[SerializeField]
	private Image _focus;

	[SerializeField]
	private Button _tumblbug;

	[SerializeField]
	private Button _twitter;

	[SerializeField]
	private Button _openTrade;

	[SerializeField]
	private Button _tumblr;

	[SerializeField]
	private Button _newGame;

	[SerializeField]
	private Button _quit;

	private void Awake()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		ButtonClickedEvent onClick = _tumblbug.onClick;
		object obj = _003C_003Ec._003C_003E9__7_0;
		if (obj == null)
		{
			UnityAction val = delegate
			{
				Application.OpenURL("https://tumblbug.com/skul");
			};
			_003C_003Ec._003C_003E9__7_0 = val;
			obj = (object)val;
		}
		((UnityEvent)onClick).AddListener((UnityAction)obj);
		ButtonClickedEvent onClick2 = _twitter.onClick;
		object obj2 = _003C_003Ec._003C_003E9__7_1;
		if (obj2 == null)
		{
			UnityAction val2 = delegate
			{
				Application.OpenURL("https://twitter.com/Skul_game");
			};
			_003C_003Ec._003C_003E9__7_1 = val2;
			obj2 = (object)val2;
		}
		((UnityEvent)onClick2).AddListener((UnityAction)obj2);
		ButtonClickedEvent onClick3 = _openTrade.onClick;
		object obj3 = _003C_003Ec._003C_003E9__7_2;
		if (obj3 == null)
		{
			UnityAction val3 = delegate
			{
				Application.OpenURL("https://otrade.co/funding/334");
			};
			_003C_003Ec._003C_003E9__7_2 = val3;
			obj3 = (object)val3;
		}
		((UnityEvent)onClick3).AddListener((UnityAction)obj3);
		ButtonClickedEvent onClick4 = _tumblr.onClick;
		object obj4 = _003C_003Ec._003C_003E9__7_3;
		if (obj4 == null)
		{
			UnityAction val4 = delegate
			{
				Application.OpenURL("https://skulthegame.tumblr.com/");
			};
			_003C_003Ec._003C_003E9__7_3 = val4;
			obj4 = (object)val4;
		}
		((UnityEvent)onClick4).AddListener((UnityAction)obj4);
		((UnityEvent)_newGame.onClick).AddListener((UnityAction)delegate
		{
			((Component)this).gameObject.SetActive(false);
			Singleton<Service>.Instance.ResetGameScene();
		});
		((UnityEvent)_quit.onClick).AddListener(new UnityAction(Application.Quit));
	}

	private void OnEnable()
	{
		PlayerInput.blocked.Attach(this);
		Chronometer.global.AttachTimeScale(this, 0f);
		EventSystem.current.SetSelectedGameObject(((Component)_tumblbug).gameObject);
		((Selectable)_tumblbug).Select();
	}

	private void OnDisable()
	{
		PlayerInput.blocked.Detach(this);
		Chronometer.global.DetachTimeScale(this);
	}

	private void Update()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		Transform val = ((currentSelectedGameObject != null) ? currentSelectedGameObject.transform : null);
		if (!((Object)(object)val == (Object)null))
		{
			RectTransform component = ((Component)val).GetComponent<RectTransform>();
			((Transform)((Graphic)_focus).rectTransform).position = val.position;
			Vector2 sizeDelta = component.sizeDelta;
			sizeDelta.x /= ((Component)_focus).transform.localScale.x;
			sizeDelta.y /= ((Component)_focus).transform.localScale.y;
			sizeDelta.x -= 6f;
			sizeDelta.y -= 6f;
			((Graphic)_focus).rectTransform.sizeDelta = sizeDelta;
		}
	}
}
