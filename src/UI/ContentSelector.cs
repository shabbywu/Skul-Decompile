using System;
using FX;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInput;

namespace UI;

public class ContentSelector : Dialogue
{
	private Action _onButton1Click;

	private Action _onButton2Click;

	private Action _onButton3Click;

	private Action _onCancel;

	[SerializeField]
	private Button _button1;

	[SerializeField]
	private TMP_Text _button1Label;

	[SerializeField]
	private Button _button2;

	[SerializeField]
	private TMP_Text _button2Label;

	[SerializeField]
	private Button _button3;

	[SerializeField]
	private TMP_Text _button3Label;

	[SerializeField]
	private SoundInfo _selectSound;

	public override bool closeWithPauseKey => false;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		UnityAction val = new UnityAction(base.Close);
		((UnityEvent)_button1.onClick).AddListener(val);
		((UnityEvent)_button2.onClick).AddListener(val);
		((UnityEvent)_button3.onClick).AddListener(val);
		((UnityEvent)_button1.onClick).AddListener(new UnityAction(InvokeButton1Click));
		((UnityEvent)_button2.onClick).AddListener(new UnityAction(InvokeButton2Click));
		((UnityEvent)_button3.onClick).AddListener(new UnityAction(InvokeButton3Click));
		Selectable[] componentsInChildren = ((Component)this).GetComponentsInChildren<Selectable>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Component)componentsInChildren[i]).gameObject.AddComponent<PlaySoundOnSelected>().soundInfo = _selectSound;
		}
	}

	private void InvokeButton1Click()
	{
		_onButton1Click();
	}

	private void InvokeButton2Click()
	{
		_onButton2Click();
	}

	private void InvokeButton3Click()
	{
		_onButton3Click();
	}

	public void Open(string label1, Action action1, string cancelLabel, Action onCancel)
	{
		((Component)_button3).gameObject.SetActive(false);
		_button1Label.text = label1;
		_button2Label.text = cancelLabel;
		_onButton1Click = action1;
		_onButton2Click = onCancel;
		_onCancel = onCancel;
		Open();
	}

	public void Open(string label1, Action action1, string label2, Action action2, string cancelLabel, Action onCancel)
	{
		((Component)_button3).gameObject.SetActive(true);
		_button1Label.text = label1;
		_button2Label.text = label2;
		_button3Label.text = cancelLabel;
		_onButton1Click = action1;
		_onButton2Click = action2;
		_onButton3Click = onCancel;
		_onCancel = onCancel;
		Open();
	}

	private new void Update()
	{
		if (((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed)
		{
			_onCancel?.Invoke();
		}
	}
}
