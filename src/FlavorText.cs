using Characters.Gear;
using UnityEngine;
using UnityEngine.UI;

public class FlavorText : MonoBehaviour
{
	[SerializeField]
	private Text _text;

	private Gear _gear;

	private void Awake()
	{
		_gear = ((Component)this).GetComponentInParent<Gear>();
		if (!_gear.hasFlavor)
		{
			((Component)this).gameObject.SetActive(false);
		}
		else
		{
			_text.text = _gear.flavor;
		}
	}
}
