using System;
using UnityEngine;

[Serializable]
public class CustomFloat
{
	[Serializable]
	public class Reorderable : ReorderableArray<CustomFloat>
	{
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

	public float value => _type switch
	{
		Type.Constant => _value, 
		Type.RandomBetweenTwoConstants => Random.Range(_value, _maxValue), 
		Type.RandomWithinCurve => _value * _curve.Evaluate((float)Random.Range(0, _curve.length)), 
		_ => 0f, 
	};

	public CustomFloat(float @default)
	{
		Set(@default);
	}

	public CustomFloat(float min, float max)
	{
		Set(min, max);
	}

	public void Set(float value)
	{
		_value = value;
	}

	public void Set(float min, float max)
	{
		_type = Type.RandomBetweenTwoConstants;
		_value = min;
		_maxValue = max;
	}

	public float GetAverage()
	{
		switch (_type)
		{
		case Type.Constant:
			return _value;
		case Type.RandomBetweenTwoConstants:
			return (_value + _maxValue) / 2f;
		case Type.RandomWithinCurve:
		{
			float num = 10f;
			float num2 = 0f;
			for (int i = 0; (float)i <= num; i++)
			{
				num2 += _curve.Evaluate((float)(_curve.length * i) / num);
			}
			return _value * num2 / num;
		}
		default:
			return 0f;
		}
	}
}
