using System.Collections;
using UnityEngine;

public class ChronometerTime
{
	public Chronometer chronometer { get; private set; }

	public float time { get; private set; }

	public ChronometerTime(Chronometer chronometer, MonoBehaviour coroutineOwner)
	{
		this.chronometer = chronometer;
		coroutineOwner.StartCoroutine(CTimer());
	}

	private IEnumerator CTimer()
	{
		while (true)
		{
			time += Time.deltaTime * chronometer.timeScale;
			yield return null;
		}
	}
}
