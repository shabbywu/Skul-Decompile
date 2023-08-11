using UnityEngine;

namespace Level;

public class PositionCache : MonoBehaviour
{
	[SerializeField]
	private Transform _transform;

	private Vector2 _position;

	private void Awake()
	{
		if ((Object)(object)_transform == (Object)null)
		{
			_transform = ((Component)this).transform;
		}
	}

	public Vector2 Load()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return _position;
	}

	public void Save()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_position = Vector2.op_Implicit(_transform.position);
	}
}
