using System;
using TMPro;
using UnityEngine;

namespace UI;

public class TextHolderSizer : MonoBehaviour
{
	[Flags]
	public enum Mode
	{
		Width = 1,
		Height = 2,
		Both = 3
	}

	[SerializeField]
	private RectTransform _rectTransform;

	[SerializeField]
	private TMP_Text _text;

	[EnumFlag]
	[SerializeField]
	private Mode _mode;

	[SerializeField]
	private Vector2 _minSize = Vector2.zero;

	[SerializeField]
	private Vector2 _maxSize = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

	[SerializeField]
	private Vector2 _padding = Vector2.zero;

	[SerializeField]
	private Vector2 _multiplier = Vector2.one;

	private void Update()
	{
		UpdateSize();
	}

	public void UpdateSize()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2(_text.preferredWidth * _multiplier.x, _text.preferredHeight * _multiplier.y) + _padding;
		Vector2 sizeDelta = _rectTransform.sizeDelta;
		if (_mode.HasFlag(Mode.Width))
		{
			sizeDelta.x = Mathf.Clamp(_minSize.x, val.x, _maxSize.x);
		}
		if (_mode.HasFlag(Mode.Height))
		{
			sizeDelta.y = Mathf.Clamp(_minSize.y, val.y, _maxSize.y);
		}
		_rectTransform.sizeDelta = sizeDelta;
	}
}
