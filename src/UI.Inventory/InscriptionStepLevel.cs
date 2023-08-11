using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public sealed class InscriptionStepLevel : MonoBehaviour
{
	[Serializable]
	private class Steps
	{
		private const string fullActiveLevel = "#fbd53e";

		public static readonly string activatedColor = "#E6D2C0";

		public static readonly string inactivatedColor = "#9C8161";

		[SerializeField]
		private GameObject _parent;

		[SerializeField]
		private TMP_Text[] _stepTexts;

		[SerializeField]
		private GameObject[] _arrowImages;

		public void Activate(IList<int> steps, int stepIndex, bool fullactiveColorChange)
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			if (_stepTexts.Length != steps.Count - 1)
			{
				Debug.LogError((object)"각인의 개수가 잘못 전달되었습니다.");
				return;
			}
			_parent.SetActive(true);
			Color color = default(Color);
			for (int i = 0; i < _stepTexts.Length; i++)
			{
				_stepTexts[i].text = steps[i + 1].ToString();
				string text = ((stepIndex == i + 1) ? activatedColor : inactivatedColor);
				if (fullactiveColorChange && i == steps.Count - 2 && stepIndex == i + 1)
				{
					text = "#fbd53e";
				}
				ColorUtility.TryParseHtmlString(text, ref color);
				((Graphic)_stepTexts[i]).color = color;
			}
		}

		public void Deactivate()
		{
			_parent.SetActive(false);
		}
	}

	[SerializeField]
	private Steps _oneStep;

	[SerializeField]
	private Steps _twoStep;

	[SerializeField]
	private Steps _threeStep;

	public void Set(Inscription.Key key, bool fullactiveColorChange = false)
	{
		Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
		ReadOnlyCollection<int> steps = inscription.steps;
		int step = inscription.step;
		switch (inscription.steps.Count)
		{
		case 2:
			_twoStep.Deactivate();
			_threeStep.Deactivate();
			_oneStep.Activate(steps, step, fullactiveColorChange);
			break;
		case 3:
			_oneStep.Deactivate();
			_threeStep.Deactivate();
			_twoStep.Activate(steps, step, fullactiveColorChange);
			break;
		case 4:
			_oneStep.Deactivate();
			_twoStep.Deactivate();
			_threeStep.Activate(steps, step, fullactiveColorChange);
			break;
		case 0:
		case 1:
			break;
		}
	}
}
