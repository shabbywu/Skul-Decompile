using UnityEngine;

namespace FX.Connections;

public class SimpleConnector : MonoBehaviour
{
	[SerializeField]
	private Connection _connection;

	[SerializeField]
	private Transform _start;

	[SerializeField]
	private Vector2 _startOffset;

	[SerializeField]
	private Transform _end;

	[SerializeField]
	private Vector2 _endOffset;

	private void Awake()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_connection.Connect(_start, _startOffset, _end, _endOffset);
	}
}
