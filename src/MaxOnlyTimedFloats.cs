using System.Collections.Generic;
using UnityEngine;

public class MaxOnlyTimedFloats
{
	private abstract class Runner
	{
		public readonly object key;

		protected float _timeToExpire;

		public abstract float value { get; }

		public bool TakeTime(float time)
		{
			return (_timeToExpire -= time) <= 0f;
		}

		public Runner(object key, float duration)
		{
			this.key = key;
			_timeToExpire = duration;
		}
	}

	private class CurveRunner : Runner
	{
		private readonly Curve _curve;

		public override float value => _curve.Evaluate(_timeToExpire);

		public CurveRunner(object key, Curve curve)
			: base(key, curve.duration)
		{
			_curve = curve;
		}
	}

	private class ConstantRunner : Runner
	{
		private readonly float _value;

		public override float value => _value;

		public ConstantRunner(object key, float value, float duration)
			: base(key, duration)
		{
			_value = value;
		}
	}

	private readonly List<Runner> _runners = new List<Runner>();

	public float value { get; protected set; }

	public void Attach(object key, float value, float duration)
	{
		_runners.Add(new ConstantRunner(key, value, duration));
	}

	public void Attach(object key, Curve curve)
	{
		_runners.Add(new CurveRunner(key, curve));
	}

	public void Detach(object key)
	{
		for (int i = 0; i < _runners.Count; i++)
		{
			if (_runners[i].key == key)
			{
				_runners.RemoveAt(i);
				break;
			}
		}
	}

	public void Update()
	{
		Update(Time.deltaTime);
	}

	public void Update(float deltaTime)
	{
		value = 0f;
		for (int num = _runners.Count - 1; num >= 0; num--)
		{
			Runner runner = _runners[num];
			if (runner.value > value)
			{
				value = runner.value;
			}
			if (runner.TakeTime(deltaTime))
			{
				_runners.RemoveAt(num);
			}
		}
	}

	public void Clear()
	{
		_runners.Clear();
		value = 0f;
	}
}
