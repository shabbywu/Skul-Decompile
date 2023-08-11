using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Sequences;

public class GetScroll : Sequence
{
	private Character _player;

	private void Start()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
	}

	public override IEnumerator CRun()
	{
		WeaponInventory component = ((Component)_player).GetComponent<WeaponInventory>();
		Skul skul = ((Component)component.polymorphOrCurrent).GetComponent<Skul>();
		skul.getScroll.TryStart();
		while (skul.getScroll.running)
		{
			yield return null;
		}
	}
}
