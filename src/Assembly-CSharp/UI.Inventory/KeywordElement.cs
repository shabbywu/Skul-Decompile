using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public class KeywordElement : MonoBehaviour
{
	private const string fullActiveLevel = "#fbd53e";

	private const string activeLevel = "#D7C0AA";

	private const string deactiveLevel = "#9c8160";

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _level;

	[SerializeField]
	private InscriptionStepLevel _stepLevel;

	public void Set(Inscription.Key key)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
		int count = inscription.count;
		_icon.sprite = inscription.icon;
		_name.text = inscription.name;
		_stepLevel.Set(key, fullactiveColorChange: true);
		if ((Object)(object)_level != (Object)null)
		{
			string text = "#D7C0AA";
			if (inscription.isMaxStep)
			{
				text = "#fbd53e";
			}
			else if (!inscription.active)
			{
				text = "#9c8160";
			}
			Color color = default(Color);
			ColorUtility.TryParseHtmlString(text, ref color);
			_level.text = count.ToString();
			((Graphic)_level).color = color;
		}
	}
}
