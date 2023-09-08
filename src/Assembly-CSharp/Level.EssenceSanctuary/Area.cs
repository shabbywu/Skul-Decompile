using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level.EssenceSanctuary;

public class Area : MonoBehaviour
{
	[SerializeField]
	private Tilemap _baseTilemap;

	[SerializeField]
	private Transform _machine;

	[SerializeField]
	private Room[] _roomsForThisArea;

	private Room _room;

	public void Initialize()
	{
		_room = Object.Instantiate<Room>(_roomsForThisArea.Random(), ((Component)this).transform);
		_room.Initialize(_baseTilemap, _machine);
	}

	public void Accept()
	{
		_room.Accept();
	}

	public void Clear()
	{
		_room.Clear();
	}
}
