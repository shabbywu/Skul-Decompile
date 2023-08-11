using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level.BlackMarket;

public class CollectorGearSlot : MonoBehaviour
{
	[SerializeField]
	private Transform _itemPosition;

	[SerializeField]
	private TMP_Text _text;

	private DroppedGear _droppedGear;

	private string soldOutText = "---";

	public Vector3 itemPosition => _itemPosition.position;

	public DroppedGear droppedGear
	{
		get
		{
			return _droppedGear;
		}
		set
		{
			_droppedGear = value;
			_text.text = _droppedGear.price.ToString();
		}
	}

	private void Update()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_droppedGear == (Object)null)
		{
			_text.text = soldOutText;
			((Graphic)_text).color = Color.white;
		}
		else if (_droppedGear.price > 0)
		{
			((Graphic)_text).color = (GameData.Currency.gold.Has(_droppedGear.price) ? Color.white : Color.red);
		}
		else
		{
			_text.text = soldOutText;
			((Graphic)_text).color = Color.white;
		}
	}
}
