using System.Collections.ObjectModel;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public sealed class KeywordOption : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _level;

	[SerializeField]
	[Space]
	private InscriptionStepElement[] _stepElements;

	[SerializeField]
	private Vector2[] _stepElementsHeightByStepLength;

	[SerializeField]
	private InscriptionStepLevel _stepLevel;

	public void Set(Inscription.Key key)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
		int count = inscription.count;
		_icon.sprite = inscription.activeIcon;
		_name.text = inscription.name;
		ReadOnlyCollection<int> steps = inscription.steps;
		int step = inscription.step;
		_stepLevel.Set(key);
		if ((Object)(object)_level != (Object)null)
		{
			_level.text = count.ToString();
		}
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
				_stepElements[num].Set(key, steps, j, step >= j);
				_stepElements[num].ClampHeight(val.x, val.y);
				num++;
			}
		}
	}
}
