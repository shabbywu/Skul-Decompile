using Characters.Gear.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public sealed class UpgradeOption : MonoBehaviour
{
	[SerializeField]
	private Image _thumnailIcon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _type;

	[SerializeField]
	private TMP_Text _flavor;

	[SerializeField]
	private TMP_Text _description;

	private const string activateColorcode = "755754";

	private const string deactivateColorcode = "B2977B";

	public void Set(UpgradeObject upgrade)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_thumnailIcon).enabled = true;
		_thumnailIcon.sprite = upgrade.thumbnail;
		((Component)_thumnailIcon).transform.localScale = Vector3.one * 3f;
		((Graphic)_thumnailIcon).SetNativeSize();
		_name.text = upgrade.displayName;
		if (upgrade.type == UpgradeObject.Type.Cursed)
		{
			_type.text = UpgradeResource.Reference.curseText;
		}
		else
		{
			_type.text = string.Empty;
		}
		_flavor.text = upgrade.flavor;
		_description.text = upgrade.reference.GetCurrentDescription("755754", "B2977B");
	}
}
