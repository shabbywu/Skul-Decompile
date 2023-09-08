using System.Collections;
using UnityEngine;

namespace Runnables.Triggers;

public class Timer : Trigger
{
	[SerializeField]
	private bool _onPreTime;

	[SerializeField]
	private float _time;

	[SerializeField]
	private bool _range;

	[SerializeField]
	[MinMaxSlider(0f, 100f)]
	private Vector2 _timeRange;

	private bool _running;

	private void Awake()
	{
		if (_onPreTime)
		{
			_running = true;
			((MonoBehaviour)this).StartCoroutine(CRun());
		}
	}

	protected override bool Check()
	{
		if (_running)
		{
			return false;
		}
		((MonoBehaviour)this).StartCoroutine(CRun());
		return true;
	}

	private IEnumerator CRun()
	{
		_running = true;
		float seconds = (_range ? Random.Range(_timeRange.x, _timeRange.y) : _time);
		yield return Chronometer.global.WaitForSeconds(seconds);
		_running = false;
	}
}
