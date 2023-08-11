using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Sequences;

public sealed class GetWeapon : Sequence
{
	private Character _player;

	private const string weaponName = "ChiefGuard";

	private WeaponRequest _request;

	private void Start()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		WeaponReference weaponByName = Singleton<Service>.Instance.gearManager.GetWeaponByName("ChiefGuard");
		_request = weaponByName.LoadAsync();
	}

	public override IEnumerator CRun()
	{
		WeaponInventory inventory = ((Component)_player).GetComponent<WeaponInventory>();
		Skul skul = ((Component)inventory.polymorphOrCurrent).GetComponent<Skul>();
		skul.getSkul.TryStart();
		yield return null;
		((MonoBehaviour)this).StartCoroutine(CGet(skul, inventory));
	}

	private IEnumerator CGet(Skul skul, WeaponInventory inventory)
	{
		while (skul.getSkul.running)
		{
			yield return null;
		}
		while (_request == null || !_request.isDone)
		{
			yield return null;
		}
		Weapon asset = _request.asset;
		inventory.ForceEquip(asset.Instantiate());
	}
}
