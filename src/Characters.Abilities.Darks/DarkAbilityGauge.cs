using UnityEngine;

namespace Characters.Abilities.Darks;

public sealed class DarkAbilityGauge : MonoBehaviour
{
	public delegate void onChangedDelegate(float oldValue, float newValue);

	[SerializeField]
	private float _initialValue;

	[SerializeField]
	private float _maxValue;

	private float _currentValue;

	public float gaugePercent => _currentValue / _maxValue;

	public float currentValue => _currentValue;

	public event onChangedDelegate onChanged;

	private void Awake()
	{
		_currentValue = _initialValue;
	}

	public void Set(float maxValue, float initialValue)
	{
		_maxValue = maxValue;
		_initialValue = initialValue;
	}

	public bool Has(float amount)
	{
		return _currentValue >= amount;
	}

	public void Clear()
	{
		Set(0f);
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
		this.onChanged?.Invoke(oldValue, _currentValue);
	}
}
