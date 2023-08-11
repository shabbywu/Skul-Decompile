using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events;

public class PlaySkulAnimation : Event
{
	private enum Type
	{
		EndPose,
		IntroIdle,
		IntroWalk
	}

	[SerializeField]
	private Type _type = Type.IntroIdle;

	private Character _player;

	public override void Run()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		if (!((Object)(object)_player == (Object)null))
		{
			Skul component = ((Component)((Component)_player).GetComponent<WeaponInventory>().polymorphOrCurrent).GetComponent<Skul>();
			switch (_type)
			{
			case Type.EndPose:
				component.endPose.TryStart();
				break;
			case Type.IntroIdle:
				component.introIdle.TryStart();
				break;
			case Type.IntroWalk:
				component.introWalk.TryStart();
				break;
			}
		}
	}
}
