using System;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInput;

namespace UI;

public class Confirm : Dialogue
{
	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private Button _yes;

	[SerializeField]
	private Button _no;

	private float _elaspedTime;

	private GameObject _lastSelectedGameObject;

	public override bool closeWithPauseKey => true;

	public void Open(string text, Action action)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		if ((Object)(object)_text != (Object)null)
		{
			_text.text = text;
		}
		((UnityEventBase)_yes.onClick).RemoveAllListeners();
		((UnityEvent)_yes.onClick).AddListener((UnityAction)delegate
		{
			if (_elaspedTime > 0.3f)
			{
				Close();
				action();
			}
		});
		((UnityEventBase)_no.onClick).RemoveAllListeners();
		((UnityEvent)_no.onClick).AddListener((UnityAction)delegate
		{
			if (_elaspedTime > 0.3f)
			{
				Close();
			}
		});
		Open();
	}

	public void Open(string text, Action onYes, Action onNo)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		if ((Object)(object)_text != (Object)null)
		{
			_text.text = text;
		}
		((UnityEventBase)_yes.onClick).RemoveAllListeners();
		((UnityEvent)_yes.onClick).AddListener((UnityAction)delegate
		{
			if (_elaspedTime > 0.3f)
			{
				Close();
				onYes();
			}
		});
		((UnityEventBase)_no.onClick).RemoveAllListeners();
		((UnityEvent)_no.onClick).AddListener((UnityAction)delegate
		{
			if (_elaspedTime > 0.3f)
			{
				Close();
				onNo();
			}
		});
		Open();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0f);
		_elaspedTime = 0f;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
	}

	protected override void Update()
	{
		EventSystem current = EventSystem.current;
		if ((Object)(object)current.currentSelectedGameObject == (Object)null)
		{
			current.SetSelectedGameObject(_lastSelectedGameObject);
		}
		else
		{
			_lastSelectedGameObject = current.currentSelectedGameObject;
		}
		if (((OneAxisInputControl)KeyMapper.Map.Pause).WasPressed)
		{
			((UnityEvent)_no.onClick).Invoke();
		}
		_elaspedTime += Time.unscaledDeltaTime;
	}
}
