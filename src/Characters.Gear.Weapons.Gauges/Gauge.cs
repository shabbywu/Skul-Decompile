using System;
using UnityEngine;

namespace Characters.Gear.Weapons.Gauges;

public abstract class Gauge : MonoBehaviour
{
	[Serializable]
	public class GaugeInfo
	{
		[SerializeField]
		[Range(0f, 1f)]
		internal float proportion;

		[SerializeField]
		internal Color baseColor;

		[SerializeField]
		internal bool useChargedColor;

		[SerializeField]
		internal Color chargedColor;
	}

	public abstract float gaugePercent { get; }

	public abstract string displayText { get; }

	public abstract Color barColor { get; }

	public abstract bool secondBar { get; }

	public abstract Color secondBarColor { get; }

	public abstract Color textColor { get; }

	public abstract GaugeInfo defaultBarGaugeColor { get; }

	public abstract GaugeInfo secondBarGaugeColor { get; }
}
