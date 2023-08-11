using Data;
using TMPro;
using UnityEngine;

public class HardmodeLevelHUD : MonoBehaviour
{
	[SerializeField]
	private GameObject _display;

	[SerializeField]
	private TextMeshProUGUI _text;

	private bool _isHardmode;

	private int _cachedLevel = -1;

	private void Awake()
	{
		_isHardmode = GameData.HardmodeProgress.hardmode;
		_display.gameObject.SetActive(_isHardmode);
	}

	private void Update()
	{
		if (GameData.HardmodeProgress.hardmode && _cachedLevel != GameData.HardmodeProgress.hardmodeLevel)
		{
			_cachedLevel = GameData.HardmodeProgress.hardmodeLevel;
			((TMP_Text)_text).text = GameData.HardmodeProgress.hardmodeLevel.ToString();
		}
		if (!_isHardmode && GameData.HardmodeProgress.hardmode)
		{
			_isHardmode = true;
			_display.gameObject.SetActive(true);
		}
		else if (_isHardmode && !GameData.HardmodeProgress.hardmode)
		{
			_isHardmode = false;
			_display.gameObject.SetActive(false);
		}
	}
}
