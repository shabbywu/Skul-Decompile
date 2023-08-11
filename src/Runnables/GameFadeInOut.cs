using Scenes;
using UnityEngine;

namespace Runnables;

public sealed class GameFadeInOut : Runnable
{
	[SerializeField]
	private bool _fadeOut;

	[SerializeField]
	private float _speed;

	public override void Run()
	{
		GameBase instance = Scene<GameBase>.instance;
		if (_fadeOut)
		{
			instance.gameFadeInOut.FadeOut();
		}
		else
		{
			instance.gameFadeInOut.FadeIn();
		}
	}
}
