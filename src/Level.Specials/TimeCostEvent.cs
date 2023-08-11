using System;
using UnityEngine;

namespace Level.Specials;

public class TimeCostEvent : CostEvent
{
	[SerializeField]
	private TimeCostEventDisplay _display;

	[NonSerialized]
	public float updateInterval = 0.2f;

	[SerializeField]
	private int _initialCost;

	[SerializeField]
	private int _maxCost;

	[SerializeField]
	private float _costMultipler;

	[SerializeField]
	private float _targetTime;

	private double _currentCost;

	private double _speed;

	private void Awake()
	{
		_currentCost = _initialCost;
	}

	public void UpdateCost()
	{
		double num = _speed * (double)updateInterval;
		if (_currentCost + num >= (double)_maxCost)
		{
			_currentCost = _maxCost;
			return;
		}
		_currentCost += num;
		_display.UpdateDisplay();
	}

	public void AddSpeed(double extraIncrease)
	{
		_speed += extraIncrease;
	}

	public void SetCostSpeed(float chestPrice)
	{
		_speed = chestPrice * _costMultipler / _targetTime;
	}

	public override double GetValue()
	{
		return _currentCost;
	}
}
