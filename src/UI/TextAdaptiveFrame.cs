using TMPro;
using UnityEngine;

namespace UI;

public class TextAdaptiveFrame : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private RectTransform _rectTransform;

	[SerializeField]
	private TMP_Text _text;

	public string text
	{
		get
		{
			return _text.text;
		}
		set
		{
			_text.text = value;
		}
	}

	private void OnEnable()
	{
		UpdateSize();
	}

	public void UpdateSize()
	{
		float num = _text.preferredWidth * 0.2f + 40f;
		_rectTransform.SetSizeWithCurrentAnchors((Axis)0, num);
	}
}
