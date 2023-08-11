using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Level.EssenceSanctuary;

public class Room : MonoBehaviour
{
	[SerializeField]
	private PasteTile _pasteTile;

	[SerializeField]
	private Transform _machinePosition;

	[SerializeField]
	private UnityEvent _onAccept;

	[SerializeField]
	private UnityEvent _onClear;

	public void Initialize(Tilemap baseTilemap, Transform machine)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		_pasteTile.Paste(baseTilemap);
		machine.position = _machinePosition.position;
	}

	public void Accept()
	{
		UnityEvent onAccept = _onAccept;
		if (onAccept != null)
		{
			onAccept.Invoke();
		}
	}

	public void Clear()
	{
		UnityEvent onClear = _onClear;
		if (onClear != null)
		{
			onClear.Invoke();
		}
	}
}
