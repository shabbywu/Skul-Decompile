using InControl;
using UnityEngine;

namespace UI.TestingTool;

public class ScrollByRightStick : MonoBehaviour
{
	[SerializeField]
	private Transform _container;

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Transform container = _container;
		container.localPosition += new Vector3(0f, (0f - ((OneAxisInputControl)InputManager.ActiveDevice.RightStickY).Value) * 100f, 0f);
	}
}
