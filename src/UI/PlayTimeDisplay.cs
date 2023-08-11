using System;
using System.Globalization;
using Data;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class PlayTimeDisplay : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _text;

	private Color _originalColor;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_originalColor = ((Graphic)_text).color;
	}

	private void Update()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		string text = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\ \\:\\ mm\\ \\:\\ ss", CultureInfo.InvariantCulture);
		_text.text = text;
		if ((Object)(object)Map.Instance == (Object)null || Map.Instance.pauseTimer)
		{
			((Graphic)_text).color = Color.grey;
		}
		else
		{
			((Graphic)_text).color = _originalColor;
		}
	}
}
