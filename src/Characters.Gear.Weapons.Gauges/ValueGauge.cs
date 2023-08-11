using UnityEngine;

namespace Characters.Gear.Weapons.Gauges;

public class ValueGauge : Gauge
{
	public delegate void onChangedDelegate(float oldValue, float newValue);

	[SerializeField]
	private float _initialValue;

	[SerializeField]
	protected Color _defaultBarColor = Color.black;

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

	protected float _currentValue;

	[SerializeField]
	[Space]
	protected float _maxValue;

	[SerializeField]
	protected bool _displayText = true;

	[SerializeField]
	protected Color _textColor = Color.white;

	private string _cachedDisplayText;

	protected virtual string maxValueText => maxValue.ToString();

	public override float gaugePercent => _currentValue / _maxValue;

	public override string displayText => _cachedDisplayText;

	public float currentValue => _currentValue;

	public float maxValue
	{
		get
		{
			return _maxValue;
		}
		set
		{
			_maxValue = value;
		}
	}

	public Color defaultBarColor
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _baseBarGaugeColor.baseColor;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_baseBarGaugeColor.baseColor = value;
		}
	}

	public override Color barColor => _defaultBarColor;

	public override bool secondBar => _secondBar;

	public override Color secondBarColor => _secondBarColor;

	public override Color textColor => _textColor;

	public override GaugeInfo defaultBarGaugeColor => _baseBarGaugeColor;

	public override GaugeInfo secondBarGaugeColor => _scoundBarGaugeColor;

	public event onChangedDelegate onChanged;

	private void Awake()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		_currentValue = _initialValue;
		if (_displayText)
		{
			_cachedDisplayText = $"{(int)_currentValue} / {maxValueText}";
		}
		_baseBarGaugeColor.baseColor = _defaultBarColor;
		_scoundBarGaugeColor.baseColor = _secondBarColor;
	}

	public bool isMax()
	{
		return _currentValue == _maxValue;
	}

	public bool Has(float amount)
	{
		return _currentValue >= amount;
	}

	public void Clear()
	{
		Set(0f);
	}

	public bool Consume(float amount)
	{
		if (!Has(amount))
		{
			return false;
		}
		Set(_currentValue - amount);
		return true;
	}

	public void Add(float amount)
	{
		Set(_currentValue + amount);
	}

	public void FillUp()
	{
		Set(_maxValue);
	}

	public void Set(float value)
	{
		value = Mathf.Clamp(value, 0f, _maxValue);
		float oldValue = _currentValue;
		_currentValue = value;
		if (_displayText)
		{
			_cachedDisplayText = $"{(int)_currentValue} / {maxValueText}";
		}
		this.onChanged?.Invoke(oldValue, _currentValue);
	}
}
