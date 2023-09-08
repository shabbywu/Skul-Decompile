using Characters.Gear;
using Characters.Gear.Items;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup;

public class GearPopupForItemSelection : MonoBehaviour
{
	[SerializeField]
	private Image _image;

	[Space]
	[SerializeField]
	private RectTransform _rectTransform;

	[Space]
	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	[Space]
	private TMP_Text _rarity;

	[SerializeField]
	[Space]
	private TMP_Text _description;

	[Space]
	[SerializeField]
	private GearPopupKeyword _keyword1;

	[SerializeField]
	private GearPopupKeyword _keyword2;

	private Gear _gear;

	private const string _omenKey = "synergy/key/Omen/name";

	public RectTransform rectTransform => _rectTransform;

	private static string _interactionLootLabel => Localization.GetLocalizedString("label/interaction/loot");

	private static string _interactionPurcaseLabel => Localization.GetLocalizedString("label/interaction/purchase");

	public void Set(Item item)
	{
		_gear = item;
		_name.text = item.displayName;
		if (item.gearTag.HasFlag(Gear.Tag.Omen))
		{
			_rarity.text = Localization.GetLocalizedString("synergy/key/Omen/name");
		}
		else
		{
			_rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", item.rarity));
		}
		_description.text = item.description;
		_keyword1.Set(item.keyword1);
		_keyword2.Set(item.keyword2);
	}
}
