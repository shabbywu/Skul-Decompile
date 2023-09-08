using UnityEngine;

namespace UI;

public class Scaler : MonoBehaviour
{
	[SerializeField]
	private RectTransform _canvas;

	[SerializeField]
	private RectTransform _content;

	[SerializeField]
	private ScreenLetterBox _letterBox;

	private bool _verticalLetterBox;

	private Vector2 _contentSize;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_contentSize = _content.sizeDelta;
	}

	public void SetVerticalLetterBox(bool verticalLetterBox)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_verticalLetterBox = verticalLetterBox;
		_letterBox.SetVerticalLetterBox(verticalLetterBox);
		if (_verticalLetterBox)
		{
			_content.sizeDelta = _contentSize;
		}
		else
		{
			_content.sizeDelta = new Vector2(_canvas.sizeDelta.x, _contentSize.y);
		}
	}
}
