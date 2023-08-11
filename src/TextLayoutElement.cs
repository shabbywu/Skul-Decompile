using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class TextLayoutElement : MonoBehaviour, ILayoutElement
{
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private int _layoutPriority = 1;

	[SerializeField]
	private float _padding;

	[SerializeField]
	private float _userMinHeight;

	[SerializeField]
	private float _maxWidth = 100f;

	[SerializeField]
	private float _maxHeight = float.PositiveInfinity;

	private float _minWidth = -1f;

	private float _minHeight = -1f;

	public string text
	{
		get
		{
			return ((TMP_Text)_text).text;
		}
		set
		{
			((TMP_Text)_text).text = value;
		}
	}

	public float preferredWidth => -1f;

	public float flexibleWidth => -1f;

	public float minWidth => _minWidth;

	public float minHeight => _minHeight;

	public float preferredHeight => -1f;

	public float flexibleHeight => -1f;

	public int layoutPriority => _layoutPriority;

	public void CalculateLayoutInputHorizontal()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_text == (Object)null)
		{
			_minWidth = -1f;
			return;
		}
		float num = ((TMP_Text)_text).preferredWidth;
		Rect rect = ((TMP_Text)_text).rectTransform.rect;
		_minWidth = Mathf.Min(num, ((Rect)(ref rect)).width) + _padding;
	}

	public void CalculateLayoutInputVertical()
	{
		if ((Object)(object)_text == (Object)null)
		{
			_minHeight = -1f;
			return;
		}
		_minHeight = ((TMP_Text)_text).preferredHeight + _padding;
		if (_userMinHeight > 0f)
		{
			_minHeight = Mathf.Max(_userMinHeight, _minHeight);
		}
		_minHeight = Mathf.Min(_maxHeight, _minHeight);
	}
}
