using System.Collections;
using UnityEngine;

namespace Level.Waves;

public sealed class TimeRemain : Leaf
{
	[SerializeField]
	private EnemyWave _target;

	[SerializeField]
	private float _time;

	private bool _spawnable;

	private void Awake()
	{
		if ((Object)(object)_target == (Object)null)
		{
			CheckTime();
		}
		else
		{
			_target.onSpawn += CheckTime;
		}
	}

	protected override bool Check(EnemyWave wave)
	{
		return _spawnable;
	}

	private void CheckTime()
	{
		((MonoBehaviour)this).StartCoroutine("CCheckTime");
	}

	private IEnumerator CCheckTime()
	{
		yield return Chronometer.global.WaitForSeconds(_time);
		_spawnable = true;
	}
}
