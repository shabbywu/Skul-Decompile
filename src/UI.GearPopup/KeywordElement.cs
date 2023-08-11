using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup;

public class KeywordElement : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _level;

	[SerializeField]
	private TMP_Text _description;

	private Inscription.Key _key;

	private Inscription _keyword;

	public void Set(Inscription.Key key, int delta = 0)
	{
		_key = key;
		_keyword = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
		if ((Object)(object)_icon != (Object)null)
		{
			Sprite sprite = _keyword.activeIcon;
			int num = _keyword.count + delta;
			if (num >= _keyword.maxStep)
			{
				sprite = _keyword.fullActiveIcon;
			}
			else if (num < _keyword.settings.steps[1])
			{
				sprite = _keyword.deactiveIcon;
			}
			_icon.sprite = sprite;
		}
		if ((Object)(object)_name != (Object)null)
		{
			_name.text = _keyword.name;
		}
		UpdateLevel(delta);
		if ((Object)(object)_description != (Object)null)
		{
			_description.text = _keyword.GetDescription();
		}
	}

	public void UpdateLevel(int delta = 0)
	{
		if (!((Object)(object)_level == (Object)null))
		{
			string format = ((delta > 0) ? "<color=#5FED64>{0}</color>" : ((delta != 0) ? "<color=#FF4D4D>{0}</color>" : "{0}"));
			_level.text = string.Format(format, _keyword.count + delta);
		}
	}
}
