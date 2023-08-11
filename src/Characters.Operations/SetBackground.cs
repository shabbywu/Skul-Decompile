using Level;
using Scenes;
using UnityEngine;

namespace Characters.Operations;

public sealed class SetBackground : Operation
{
	[SerializeField]
	private ParallaxBackground _background;

	public override void Run()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Map instance = Map.Instance;
		Scene<GameBase>.instance.SetBackground(_background, instance.playerOrigin.y - instance.backgroundOrigin.y);
	}
}
