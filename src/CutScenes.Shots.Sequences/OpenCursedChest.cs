using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Sequences;

public sealed class OpenCursedChest : Event
{
	private Character _player;

	private void Start()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
	}

	public override void Run()
	{
		Prisoner2 component = ((Component)((Component)_player).GetComponent<WeaponInventory>().polymorphOrCurrent).GetComponent<Prisoner2>();
		if (!((Object)(object)component == (Object)null))
		{
			((MonoBehaviour)this).StartCoroutine(component.COpenCursedChest());
		}
	}
}
