using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Pause;

public class Selection : Selectable
{
	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private PointerDownHandler _left;

	[SerializeField]
	private PointerDownHandler _right;

	private IList<string> _texts;

	private int _value;

	public int value
	{
		get
		{
			return _value;
		}
		set
		{
			SetValueWithoutNotify(value);
			this.onValueChanged?.Invoke(_value);
		}
	}

	public string text => _texts[value];

	public event Action<int> onValueChanged;

	protected override void Awake()
	{
		((Selectable)this).Awake();
		_left.onPointerDown = MoveLeft;
		_right.onPointerDown = MoveRight;
	}

	private void ValidateValue()
	{
		if (_value < 0)
		{
			_value = _texts.Count - 1;
		}
		else if (_value >= _texts.Count)
		{
			_value %= _texts.Count;
		}
	}

	public void SetTexts(IList<string> texts)
	{
		_texts = texts;
		ValidateValue();
		_text.text = _texts[_value];
	}

	public void SetValueWithoutNotify(int value)
	{
		_value = value;
		ValidateValue();
		_text.text = _texts[_value];
	}

	public override void OnMove(AxisEventData eventData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		MoveDirection moveDir = eventData.moveDir;
		if ((int)moveDir != 0)
		{
			if ((int)moveDir == 2)
			{
				MoveRight();
			}
			else
			{
				((Selectable)this).OnMove(eventData);
			}
		}
		else
		{
			MoveLeft();
		}
	}

	public void MoveLeft()
	{
		value--;
	}

	public void MoveRight()
	{
		value++;
	}
}
