using Characters.Gear;
using UnityEngine;
using UnityEngine.UI;

public class GearInfo : MonoBehaviour
{
	private Gear _gear;

	[SerializeField]
	private Text _rarity;

	[SerializeField]
	private Text _name;

	[SerializeField]
	private Image _icon;

	private void Awake()
	{
		_gear = ((Component)this).GetComponentInParent<Gear>();
		_rarity.text = _gear.rarity.ToString();
		_name.text = _gear.displayName;
		_icon.sprite = ((Component)_gear.dropped).GetComponent<SpriteRenderer>().sprite;
	}
}
