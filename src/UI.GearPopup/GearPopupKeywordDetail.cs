using System.Collections.ObjectModel;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup;

public class GearPopupKeywordDetail : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _level;

	[Space]
	[SerializeField]
	private InscriptionStepElement[] _stepElements;

	[SerializeField]
	private Vector2[] _stepElementsHeightByStepLength;

	public void Set(Inscription.Key key)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
		_icon.sprite = inscription.activeIcon;
		_name.text = inscription.name;
		_level.text = $"{inscription.count}/{inscription.maxStep}";
		ReadOnlyCollection<int> steps = inscription.steps;
		if (_stepElements.Length != 0)
		{
			InscriptionStepElement[] stepElements = _stepElements;
			for (int i = 0; i < stepElements.Length; i++)
			{
				((Component)stepElements[i]).gameObject.SetActive(false);
			}
			Vector2 val = _stepElementsHeightByStepLength[steps.Count - 1];
			int num = 0;
			for (int j = 1; j < steps.Count; j++)
			{
				_stepElements[num].Set(key, steps, j, activated: true);
				_stepElements[num].ClampHeight(val.x, val.y);
				num++;
			}
		}
	}
}
