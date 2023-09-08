using System;
using UnityEngine;

namespace FX.Connections;

public abstract class Connection : MonoBehaviour
{
	private Transform _start;

	private Transform _end;

	private Vector3 _startOffset = new Vector3(0f, 0f);

	private Vector3 _endOffset = new Vector3(0f, 0f);

	protected Vector3 startPosition => _start.position + _startOffset;

	protected Vector3 endPosition => _end.position + _endOffset;

	public bool connecting { get; private set; }

	public virtual bool lostConnection
	{
		get
		{
			if (!((Object)(object)_start == (Object)null))
			{
				return (Object)(object)_end == (Object)null;
			}
			return true;
		}
	}

	public event Action OnConnect;

	public event Action OnDisconnect;

	public void Connect(Transform start, Vector2 startOffset, Transform end, Vector2 endOffset)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		connecting = true;
		_start = start;
		_end = end;
		_startOffset = Vector2.op_Implicit(startOffset);
		_endOffset = Vector2.op_Implicit(endOffset);
		Show();
		this.OnConnect?.Invoke();
	}

	public void Disconnect()
	{
		connecting = false;
		Hide();
		this.OnDisconnect?.Invoke();
	}

	protected abstract void Show();

	protected abstract void Hide();
}
