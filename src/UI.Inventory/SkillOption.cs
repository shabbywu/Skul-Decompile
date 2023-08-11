using Characters.Gear.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public class SkillOption : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _name;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private Cooldown _cooldown;

	public void Set(SkillInfo skillDescInfo)
	{
		((TMP_Text)_name).text = skillDescInfo.displayName;
		_icon.sprite = skillDescInfo.cachedIcon;
		((Graphic)_icon).SetNativeSize();
		if ((Object)(object)_description != (Object)null)
		{
			((TMP_Text)_description).text = skillDescInfo.description;
		}
		if ((Object)(object)_cooldown != (Object)null)
		{
			_cooldown.Set(skillDescInfo.action.cooldown);
		}
	}
}
