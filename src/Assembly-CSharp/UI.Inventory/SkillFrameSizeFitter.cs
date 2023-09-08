using UnityEngine;

namespace UI.Inventory;

public class SkillFrameSizeFitter : MonoBehaviour
{
	[SerializeField]
	private RectTransform _rectTransform;

	[SerializeField]
	private RectTransform _targetRectTransform;

	[SerializeField]
	private float _startHeight;

	private float _scale;

	private float _defaultHeight;

	private void Awake()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 sizeDelta = _rectTransform.sizeDelta;
		_defaultHeight = sizeDelta.y;
		_scale = ((Component)this).transform.localScale.y;
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		float y = _targetRectTransform.sizeDelta.y;
		float num = 0f;
		if (y > _startHeight)
		{
			num = (y - _startHeight) / _scale;
		}
		Vector2 sizeDelta = _rectTransform.sizeDelta;
		sizeDelta.y = _defaultHeight + num;
		_rectTransform.sizeDelta = sizeDelta;
	}
}
