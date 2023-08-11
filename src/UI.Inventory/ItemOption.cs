using Characters.Gear;
using Characters.Gear.Items;
using GameResources;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.Inventory;

public class ItemOption : MonoBehaviour
{
	[SerializeField]
	private Image _thumnailIcon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _rarity;

	[SerializeField]
	[Space]
	private GameObject _simpleContainer;

	[SerializeField]
	private GameObject _detailContainer;

	[SerializeField]
	[Space]
	private TMP_Text _flavorSimple;

	[SerializeField]
	private TMP_Text _flavorDetail;

	[SerializeField]
	private TMP_Text _description;

	[SerializeField]
	[Space]
	private PressingButton _itemDiscardKey;

	[SerializeField]
	private TMP_Text _itemDiscardText;

	[Space]
	[SerializeField]
	private KeywordOption _keyword1;

	[SerializeField]
	private KeywordOption _keyword2;

	[SerializeField]
	[Space]
	private KeywordOption _keyword1Detail;

	[SerializeField]
	private KeywordOption _keyword2Detail;

	private const string _omenKey = "synergy/key/Omen/name";

	public void Set(Item item)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_thumnailIcon).enabled = true;
		_thumnailIcon.sprite = item.thumbnail;
		((Component)_thumnailIcon).transform.localScale = Vector3.one * 3f;
		((Graphic)_thumnailIcon).SetNativeSize();
		_name.text = item.displayName;
		if (item.gearTag.HasFlag(Gear.Tag.Omen))
		{
			_rarity.text = Localization.GetLocalizedString("synergy/key/Omen/name");
		}
		else
		{
			_rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", item.rarity));
		}
		string text = (item.hasFlavor ? item.flavor : string.Empty);
		_flavorSimple.text = text;
		_description.text = item.description;
		((Component)_itemDiscardKey).gameObject.SetActive(true);
		_itemDiscardText.text = Localization.GetLocalizedString("label/inventory/discardItem");
		if (item.currencyByDiscard > 0)
		{
			_itemDiscardText.text = $"{_itemDiscardText.text}(<color=#FFDE37>{item.currencyByDiscard}</color>)";
		}
		_keyword1.Set(item.keyword1);
		_keyword2.Set(item.keyword2);
		_keyword1Detail.Set(item.keyword1);
		_keyword2Detail.Set(item.keyword2);
	}

	private void Update()
	{
		if (_detailContainer.activeSelf && !((OneAxisInputControl)KeyMapper.Map.UiInteraction3).IsPressed)
		{
			if (!((OneAxisInputControl)KeyMapper.Map.UiInteraction2).IsPressed)
			{
				_simpleContainer.SetActive(true);
				_detailContainer.SetActive(false);
			}
		}
		else if (!_detailContainer.activeSelf && ((OneAxisInputControl)KeyMapper.Map.UiInteraction3).IsPressed)
		{
			_simpleContainer.SetActive(false);
			_detailContainer.SetActive(true);
		}
	}
}
