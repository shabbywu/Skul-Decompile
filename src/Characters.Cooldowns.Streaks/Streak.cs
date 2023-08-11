using System.Collections;
using UnityEngine;

namespace Characters.Cooldowns.Streaks;

public class Streak : IStreak
{
	private float _remainTime;

	private CoroutineReference _update;

	public int count { get; set; }

	public float timeout { get; set; }

	public int remains { get; private set; }

	public float remainPercent => _remainTime / timeout;

	public Streak(int count, float timeout)
	{
		this.count = count;
		this.timeout = timeout;
	}

	public bool Consume()
	{
		if (remains > 0)
		{
			remains--;
			return true;
		}
		return false;
	}

	public void Start()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (count != 0)
		{
			((CoroutineReference)(ref _update)).Stop();
			_update = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)CoroutineProxy.instance, CUpdate());
		}
	}

	private IEnumerator CUpdate()
	{
		remains = count;
		_remainTime = timeout;
		Global chronometer = Chronometer.global;
		while (_remainTime > 0f)
		{
			yield return null;
			_remainTime -= ((ChronometerBase)chronometer).deltaTime;
		}
		remains = 0;
	}

	public void Expire()
	{
		((CoroutineReference)(ref _update)).Stop();
		remains = 0;
		_remainTime = 0f;
	}
}
