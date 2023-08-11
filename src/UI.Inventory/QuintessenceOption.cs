using Characters.Gear.Quintessences;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public class QuintessenceOption : MonoBehaviour
{
	[SerializeField]
	private Image _thumnailIcon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _rarity;

	[SerializeField]
	private TMP_Text _cooldown;

	[Space]
	[SerializeField]
	private TMP_Text _flavor;

	[Space]
	[SerializeField]
	private TMP_Text _activeName;

	[SerializeField]
	private TMP_Text _activeDescription;

	public void Set(Quintessence essence)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_thumnailIcon).enabled = true;
		_thumnailIcon.sprite = essence.thumbnail;
		((Component)_thumnailIcon).transform.localScale = Vector3.one * 3f;
		((Graphic)_thumnailIcon).SetNativeSize();
		_name.text = essence.displayName;
		_rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", essence.rarity));
		TMP_Text cooldown = _cooldown;
		float cooldownTime = essence.cooldown.time.cooldownTime;
		cooldown.text = cooldownTime.ToString();
		_flavor.text = (essence.hasFlavor ? essence.flavor : string.Empty);
		_activeName.text = essence.activeName;
		_activeDescription.text = essence.activeDescription;
	}
}
