using UnityEngine;

namespace UI.GearPopup;

public class CopySizeAndPosition : MonoBehaviour
{
	[SerializeField]
	private RectTransform _rectTransform;

	[SerializeField]
	private RectTransform _targetTransform;

	private void Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_rectTransform.sizeDelta = _targetTransform.sizeDelta;
		((Transform)_rectTransform).position = ((Transform)_targetTransform).position;
	}
}
