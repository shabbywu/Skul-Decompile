using Level;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class LuckyMeasuringInstrumentSlot : MonoBehaviour
{
	[SerializeField]
	private Transform _dropPoint;

	private DroppedGear _droppedGear;

	public Vector3 dropPoint => _dropPoint.position;

	public DroppedGear droppedGear
	{
		get
		{
			return _droppedGear;
		}
		set
		{
			_droppedGear = value;
		}
	}
}
