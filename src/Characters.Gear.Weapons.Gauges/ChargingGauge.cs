using Characters.Actions;
using UnityEngine;

namespace Characters.Gear.Weapons.Gauges;

public class ChargingGauge : Gauge
{
	[SerializeField]
	private Color _defaultBarColor = Color.black;

	[Space]
	[SerializeField]
	private bool _secondBar;

	[SerializeField]
	private Color _secondBarColor;

	[SerializeField]
	[Space]
	private GaugeInfo _baseBarGaugeColor;

	[SerializeField]
	private GaugeInfo _scoundBarGaugeColor;

	[SerializeField]
	protected Color _textColor = Color.white;

	private ChargeAction _currentChargeAction;

	[SerializeField]
	[Space]
	private ChargeAction[] _chargeActions;

	public override float gaugePercent
	{
		get
		{
			if (!((Object)(object)_currentChargeAction == (Object)null))
			{
				return _currentChargeAction.chargingPercent;
			}
			return 0f;
		}
	}

	public override string displayText => string.Empty;

	public override Color barColor => _defaultBarColor;

	public override bool secondBar => _secondBar;

	public override Color secondBarColor => _secondBarColor;

	public override Color textColor => _textColor;

	public override GaugeInfo defaultBarGaugeColor => _baseBarGaugeColor;

	public override GaugeInfo secondBarGaugeColor => _scoundBarGaugeColor;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_baseBarGaugeColor.baseColor = _defaultBarColor;
		_scoundBarGaugeColor.baseColor = _secondBarColor;
		ChargeAction[] chargeActions = _chargeActions;
		foreach (ChargeAction chargeAction in chargeActions)
		{
			chargeAction.onStart += delegate
			{
				_currentChargeAction = chargeAction;
			};
		}
	}
}
