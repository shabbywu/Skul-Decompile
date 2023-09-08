using System;
using UnityEngine;

[Serializable]
public class CustomAngle
{
	[Serializable]
	public class Reorderable : ReorderableArray<CustomAngle>
	{
		public Reorderable()
		{
			values = new CustomAngle[0];
		}

		public Reorderable(params CustomAngle[] values)
		{
			base.values = values;
		}
	}

	private enum Type
	{
		Constant,
		RandomBetweenTwoConstants,
		RandomWithinCurve
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private float _value;

	[SerializeField]
	private float _maxValue;

	[SerializeField]
	private AnimationCurve _curve;

	public float value
	{
		get
		{
			switch (_type)
			{
			case Type.Constant:
				return _value;
			case Type.RandomBetweenTwoConstants:
				if (_value < _maxValue)
				{
					return Random.Range(_value, _maxValue);
				}
				return Random.Range(_maxValue, _value);
			case Type.RandomWithinCurve:
				return _value * _curve.Evaluate((float)Random.Range(0, _curve.length));
			default:
				return 0f;
			}
		}
	}

	public CustomAngle(float @default)
	{
		_type = Type.Constant;
		_value = @default;
	}

	public CustomAngle(float min, float max)
	{
		_type = Type.RandomBetweenTwoConstants;
		_value = min;
		_maxValue = max;
	}
}
