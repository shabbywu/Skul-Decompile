using UnityEngine;

public class StickCameraPosition : MonoBehaviour
{
	[SerializeField]
	private Vector3 _offset;

	private Camera _camera;

	public void Start()
	{
		_camera = Camera.main;
	}

	public void Update()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((Component)_camera).transform.position = ((Component)this).transform.position + _offset;
	}
}
