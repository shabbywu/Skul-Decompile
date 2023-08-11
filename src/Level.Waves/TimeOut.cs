using System.Collections;
using UnityEngine;

namespace Level.Waves;

public sealed class TimeOut : Leaf
{
	[SerializeField]
	private float _timeOut;

	private float _remainTime;

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine("CTimeOut");
	}

	protected override bool Check(EnemyWave wave)
	{
		return _remainTime < 0f;
	}

	private IEnumerator CTimeOut()
	{
		for (_remainTime = _timeOut; _remainTime > 0f; _remainTime -= ((ChronometerBase)Chronometer.global).deltaTime)
		{
			yield return null;
		}
	}
}
