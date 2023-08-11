using System;
using UnityEngine;

namespace Characters.Operations;

[Serializable]
public class ChronoInfo
{
	[SerializeField]
	private float _timeScale = 1f;

	[SerializeField]
	[FrameTime]
	private float _duration;

	public void ApplyTo(Character character)
	{
		if (_timeScale != 1f && _duration > 0f)
		{
			((ChronometerBase)character.chronometer.animation).AttachTimeScale((object)this, _timeScale, _duration);
		}
	}

	public void ApplyGlobe()
	{
		if (_timeScale != 1f && _duration > 0f)
		{
			((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, _timeScale, _duration);
		}
	}
}
