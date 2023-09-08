using GameResources;
using Hardmode;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public sealed class TextLocalizerByMode : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private string _normalKey;

	[SerializeField]
	private string _hardKey;

	[SerializeField]
	private bool _colorChange;

	[SerializeField]
	private Color _normalColor;

	[SerializeField]
	private Color _hardColor;

	private void OnEnable()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		bool hardmode = Singleton<HardmodeManager>.Instance.hardmode;
		if (_colorChange)
		{
			((Graphic)_text).color = (hardmode ? _hardColor : _normalColor);
		}
		if ((hardmode || !string.IsNullOrWhiteSpace(_normalKey)) && (!hardmode || !string.IsNullOrWhiteSpace(_hardKey)))
		{
			_text.text = Localization.GetLocalizedString(hardmode ? _hardKey : _normalKey);
		}
	}
}
