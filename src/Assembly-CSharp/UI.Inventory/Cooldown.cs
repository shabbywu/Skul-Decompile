using Characters.Cooldowns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory;

public class Cooldown : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TextMeshProUGUI _text;

	public void Set(CooldownSerializer cooldown)
	{
		float num = ((cooldown.type != CooldownSerializer.Type.Time) ? 0f : cooldown.time.cooldownTime);
		if ((Object)(object)_icon != (Object)null)
		{
			((Behaviour)_icon).enabled = true;
		}
		((Behaviour)_text).enabled = true;
		((TMP_Text)_text).text = num.ToString();
	}
}
