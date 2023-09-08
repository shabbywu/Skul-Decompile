using System;
using UnityEngine;

[Serializable]
public class Curve
{
	public static readonly Curve empty = new Curve(new AnimationCurve(), 0f, 0f);

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private float _valueMultiplier;

	[FrameTime]
	[SerializeField]
	private float _durationMultiplier;

	public float duration
	{
		get
		{
			return _durationMultiplier;
		}
		set
		{
			_durationMultiplier = value;
		}
	}

	public float valueMultiplier => _valueMultiplier;

	public Curve()
	{
		_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		_valueMultiplier = 1f;
		_durationMultiplier = 1f;
	}

	public Curve(AnimationCurve curve, float valueMultiplier = 1f, float durationMultiplier = 1f)
	{
		_curve = curve;
		_valueMultiplier = valueMultiplier;
		_durationMultiplier = durationMultiplier;
	}

	public float Evaluate(float time)
	{
		if (_durationMultiplier == 0f)
		{
			if (_curve.length == 0)
			{
				return 0f;
			}
			return ((Keyframe)(ref _curve.keys[_curve.length - 1])).value * _valueMultiplier;
		}
		return _curve.Evaluate(time / _durationMultiplier) * _valueMultiplier;
	}
}
