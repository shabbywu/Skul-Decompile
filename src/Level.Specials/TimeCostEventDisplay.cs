using System.Collections;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level.Specials;

public class TimeCostEventDisplay : MonoBehaviour
{
	[SerializeField]
	private TimeCostEvent _costEvent;

	[SerializeField]
	private TextMeshPro _text;

	[SerializeField]
	private InteractiveObject _reward;

	private const string soldOutString = "----------";

	private string _soldOutStringcache;

	private void Awake()
	{
		UpdateText((int)_costEvent.GetValue());
	}

	private void Update()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_costEvent == (Object)null))
		{
			if (!_reward.activated && _soldOutStringcache != "----------")
			{
				((Graphic)_text).color = Color.white;
				((TMP_Text)_text).text = "----------";
			}
			else
			{
				UpdateTextColor((int)_costEvent.GetValue());
			}
		}
	}

	public void UpdateDisplay()
	{
		((MonoBehaviour)this).StartCoroutine("CAnimate");
	}

	private IEnumerator CAnimate()
	{
		float elapsed = 0f;
		int start = int.Parse(((TMP_Text)_text).text);
		int dest = (int)_costEvent.GetValue();
		while (elapsed < _costEvent.updateInterval && _reward.activated)
		{
			int value = (int)Mathf.Lerp((float)start, (float)dest, elapsed / _costEvent.updateInterval);
			UpdateText(value);
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			yield return null;
		}
		UpdateText(dest);
	}

	private void UpdateText(int value)
	{
		((TMP_Text)_text).text = value.ToString();
	}

	private void UpdateTextColor(int cost)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_text).color = (GameData.Currency.gold.Has(cost) ? Color.white : Color.red);
	}
}
