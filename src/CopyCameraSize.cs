using UnityEngine;

public class CopyCameraSize : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Camera _camera;

	[SerializeField]
	private Camera _sourceCamera;

	private void OnPreRender()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_camera.orthographicSize = _sourceCamera.orthographicSize;
		_camera.rect = _sourceCamera.rect;
	}
}
