using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

public class PlaytimeTimer : MonoBehaviour
{
	private float _remainTime = 1f;

	private void Update()
	{
		if (!Singleton<Service>.Instance.fadeInOut.fading && (!((Object)(object)Map.Instance != (Object)null) || !Map.Instance.pauseTimer))
		{
			_remainTime -= Time.deltaTime;
			if (_remainTime < 0f)
			{
				_remainTime += 1f;
				GameData.Progress.playTime++;
			}
		}
	}
}
