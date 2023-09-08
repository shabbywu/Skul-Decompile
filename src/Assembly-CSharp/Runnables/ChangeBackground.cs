using Level;
using Scenes;
using UnityEngine;

namespace Runnables;

public sealed class ChangeBackground : Runnable
{
	[SerializeField]
	private ParallaxBackground _background;

	public override void Run()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Map instance = Map.Instance;
		Scene<GameBase>.instance.ChangeBackgroundWithFade(_background, instance.playerOrigin.y - instance.backgroundOrigin.y);
	}
}
