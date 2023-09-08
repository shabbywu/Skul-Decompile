using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class NpcName : MonoBehaviour
{
	[SerializeField]
	private TextHolderSizer _sizer;

	[SerializeField]
	private Image _textField;

	[SerializeField]
	private TextMeshProUGUI _text;

	public string text
	{
		get
		{
			return ((TMP_Text)_text).text;
		}
		set
		{
			((TMP_Text)_text).text = value;
			if (string.IsNullOrWhiteSpace(text))
			{
				((Component)this).gameObject.SetActive(false);
				return;
			}
			((Component)this).gameObject.SetActive(true);
			_sizer.UpdateSize();
		}
	}
}
