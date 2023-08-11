using Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimeDisplayToggle : MonoBehaviour
{
	[SerializeField]
	private Image _display;

	[SerializeField]
	private Sprite _normalTimerSprite;

	[SerializeField]
	private Sprite _hardmodeTimerSprite;

	[SerializeField]
	private bool _toggle = true;

	private bool _cachedShowTimer;

	private bool _isHardmode;

	private void Awake()
	{
		_isHardmode = GameData.HardmodeProgress.hardmode;
		_display.sprite = (_isHardmode ? _hardmodeTimerSprite : _normalTimerSprite);
		Refresh();
	}

	private void Refresh()
	{
		if (_toggle)
		{
			_cachedShowTimer = GameData.Settings.showTimer;
			((Component)_display).gameObject.SetActive(GameData.Settings.showTimer);
		}
	}

	private void Update()
	{
		if (!_isHardmode && GameData.HardmodeProgress.hardmode)
		{
			_isHardmode = true;
			_display.sprite = _hardmodeTimerSprite;
		}
		else if (_isHardmode && !GameData.HardmodeProgress.hardmode)
		{
			_isHardmode = false;
			_display.sprite = _normalTimerSprite;
		}
		if (_cachedShowTimer != GameData.Settings.showTimer)
		{
			Refresh();
		}
	}

	public void Toggle()
	{
		GameData.Settings.showTimer = !GameData.Settings.showTimer;
		Refresh();
	}
}
