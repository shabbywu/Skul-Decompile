using System;
using Services;
using Singletons;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnActivateMapReward : Trigger
{
	public override void Attach(Character character)
	{
		Singleton<Service>.Instance.levelManager.onActivateMapReward += base.Invoke;
	}

	public override void Detach()
	{
		Singleton<Service>.Instance.levelManager.onActivateMapReward -= base.Invoke;
	}
}
