using Characters.Gear.Synergy.Inscriptions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup;

public class GearPopupKeyword : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TMP_Text _name;

	public void Set(Inscription.Key key)
	{
		_icon.sprite = Inscription.GetActiveIcon(key);
		_name.text = Inscription.GetName(key);
	}
}
