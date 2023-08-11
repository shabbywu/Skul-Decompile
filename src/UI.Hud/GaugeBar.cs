using System;
using Characters.Gear.Weapons.Gauges;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud;

public class GaugeBar : MonoBehaviour
{
	[Serializable]
	private class Bar
	{
		[SerializeField]
		private Image _mainBar;

		[SerializeField]
		private Vector3 _defaultMainBarScale = Vector3.one;

		private Vector3 _mainScale = Vector3.one;

		public Image mainBar => _mainBar;

		public Vector3 defaultMainBarScale => _defaultMainBarScale;

		public void Reset()
		{
			_mainScale.x = 0f;
		}

		public void SetColor(Color color)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Graphic)_mainBar).color = color;
		}

		public void Lerp(float targetPercent)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			_mainScale.x = Mathf.Lerp(_mainScale.x, targetPercent, 0.25f);
			((Component)_mainBar).transform.localScale = Vector3.Scale(_mainScale, _defaultMainBarScale);
		}

		public void SetActive(bool value)
		{
			((Behaviour)_mainBar).enabled = value;
		}
	}

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private TMP_Text _displayText;

	[SerializeField]
	private Bar bar1;

	[SerializeField]
	private Bar bar2;

	private Gauge _gauge;

	public Gauge gauge
	{
		get
		{
			return _gauge;
		}
		set
		{
			_gauge = value;
			Update();
			((Component)this).gameObject.SetActive((Object)(object)_gauge != (Object)null);
		}
	}

	private void OnEnable()
	{
		bar1.Reset();
		bar2.Reset();
	}

	private void Update()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)gauge == (Object)null)
		{
			if (((Component)this).gameObject.activeSelf)
			{
				((Component)this).gameObject.SetActive(false);
			}
			return;
		}
		float num = math.clamp(gauge.gaugePercent, 0f, 1f);
		if (!gauge.secondBar)
		{
			Gauge.GaugeInfo defaultBarGaugeColor = gauge.defaultBarGaugeColor;
			if (defaultBarGaugeColor.useChargedColor && num >= 1f)
			{
				bar1.SetColor(defaultBarGaugeColor.chargedColor);
			}
			else
			{
				bar1.SetColor(defaultBarGaugeColor.baseColor);
			}
			bar2.SetActive(value: false);
			bar1.Lerp(num);
		}
		else
		{
			Gauge.GaugeInfo defaultBarGaugeColor2 = gauge.defaultBarGaugeColor;
			float proportion = gauge.defaultBarGaugeColor.proportion;
			Gauge.GaugeInfo secondBarGaugeColor = gauge.secondBarGaugeColor;
			float proportion2 = gauge.secondBarGaugeColor.proportion;
			if (num < proportion)
			{
				bar2.SetActive(value: false);
				bar1.SetColor(defaultBarGaugeColor2.baseColor);
				bar1.Lerp(num / proportion);
			}
			else
			{
				if (defaultBarGaugeColor2.useChargedColor)
				{
					bar1.SetColor(defaultBarGaugeColor2.chargedColor);
				}
				else
				{
					bar1.SetColor(defaultBarGaugeColor2.baseColor);
				}
				if (secondBarGaugeColor.useChargedColor && num >= 1f)
				{
					bar2.SetColor(secondBarGaugeColor.chargedColor);
				}
				else
				{
					bar2.SetColor(secondBarGaugeColor.baseColor);
				}
				bar2.SetActive(value: true);
				bar1.Lerp(1f);
				bar2.Lerp(Mathf.Clamp((num - proportion) / proportion2, 0f, 1f));
			}
		}
		((Graphic)_displayText).color = gauge.textColor;
		_displayText.text = gauge.displayText;
	}
}
