using TMPro;
using UnityEngine;

namespace UI.Inventory;

public sealed class StatValue : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _valueText;

	[SerializeField]
	private TMP_Text _unit;

	public void Set(string text, bool positive = true, string unit = "")
	{
		if (!string.IsNullOrEmpty(unit) && positive)
		{
			_valueText.text = "+" + text;
		}
		else
		{
			_valueText.text = text;
		}
		_unit.text = unit;
	}
}
