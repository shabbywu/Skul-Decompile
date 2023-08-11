using Data;
using GameResources;
using TMPro;
using UnityEngine;

namespace UI;

public class CurrencyBalanceDisplay : MonoBehaviour
{
	[SerializeField]
	private GameData.Currency.Type _type;

	[SerializeField]
	private TextMeshProUGUI _label;

	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private bool _colored;

	private int _balanceCache;

	private EnumArray<GameData.Currency.Type, string> _colorOpenByCurrency => new EnumArray<GameData.Currency.Type, string>(new string[3]
	{
		Localization.GetLocalizedString(Localization.Key.colorOpenGold),
		Localization.GetLocalizedString(Localization.Key.colorOpenDarkQuartz),
		Localization.GetLocalizedString(Localization.Key.colorOpenBone)
	});

	private string _colorClose => Localization.GetLocalizedString("cc");

	private void Awake()
	{
		UpdateText(force: true);
	}

	private void Update()
	{
		UpdateText(force: false);
	}

	private void UpdateText(bool force)
	{
		int balance = GameData.Currency.currencies[_type].balance;
		if (force || _balanceCache != balance)
		{
			_balanceCache = balance;
			if (_colored)
			{
				((TMP_Text)_text).text = $"{_colorOpenByCurrency[_type]}{balance}{_colorClose}";
			}
			else
			{
				((TMP_Text)_text).text = balance.ToString();
			}
		}
	}

	public void SetType(GameData.Currency.Type type)
	{
		_type = type;
		switch (type)
		{
		case GameData.Currency.Type.Gold:
			((TMP_Text)_label).text = Localization.GetLocalizedString("label/balance/goldBalance");
			break;
		case GameData.Currency.Type.DarkQuartz:
			((TMP_Text)_label).text = Localization.GetLocalizedString("label/balance/darkQuartzBalance");
			break;
		case GameData.Currency.Type.Bone:
			((TMP_Text)_label).text = Localization.GetLocalizedString("label/balance/boneBalance");
			break;
		}
		UpdateText(force: true);
	}
}
