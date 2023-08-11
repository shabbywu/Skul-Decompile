using UI;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
	private const float _timeToHide = 3f;

	private float _remainTimeToHide;

	private void Update()
	{
		if (!Input.mousePresent)
		{
			Cursor.visible = false;
			return;
		}
		if (Mathf.Abs(Input.GetAxis("mouse x")) > 0f || Mathf.Abs(Input.GetAxis("mouse y")) > 0f || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			_remainTimeToHide = 3f;
		}
		_remainTimeToHide -= Time.unscaledDeltaTime;
		Dialogue current = Dialogue.GetCurrent();
		Cursor.visible = _remainTimeToHide > 0f || ((Object)(object)current != (Object)null && !(current is NpcConversationBody));
	}
}
