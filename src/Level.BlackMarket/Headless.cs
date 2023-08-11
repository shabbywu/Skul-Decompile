using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using UnityEngine;

namespace Level.BlackMarket;

public class Headless : Npc
{
	private const int _randomSeed = 548136436;

	protected static readonly int _activateWithHeadHash = Animator.StringToHash("ActivateWithHead");

	[SerializeField]
	private Transform _slot;

	[SerializeField]
	private SpriteRenderer _headSlot;

	[SerializeField]
	private Animator _headSlotAnimator;

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private GameObject _talk;

	private Weapon _displayedGear;

	private Random _random;

	public string submitLine => ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray("npc/headless/submit/line"));

	private void OnDisable()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		if ((Object)(object)player != (Object)null)
		{
			player.playerComponents.inventory.weapon.onChanged -= OnWeaponChanged;
		}
	}

	private void OnWeaponChanged(Weapon old, Weapon @new)
	{
		if (!((Object)(object)@new != (Object)(object)_displayedGear))
		{
			@new.destructible = true;
			_lineText.Run(submitLine);
			if ((Object)(object)old == (Object)null)
			{
				WeaponInventory weapon = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
				old = weapon.next;
				weapon.Unequip(old);
			}
			((Renderer)_headSlot).enabled = true;
			_headSlot.sprite = old.dropped.spriteRenderer.sprite;
			_animator.Play(_activateWithHeadHash, 0, 0f);
			_headSlotAnimator.Play(Npc._activateHash, 0, 0f);
			old.destructible = false;
			Object.Destroy((Object)(object)((Component)old).gameObject);
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.onChanged -= OnWeaponChanged;
		}
	}

	private void Start()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 548136436 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		if (MMMaths.PercentChance(_random, currentChapter.currentStage.marketSettings.headlessPossibility))
		{
			Activate();
		}
		else
		{
			Deactivate();
		}
	}

	private IEnumerator CDropGear()
	{
		Rarity rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings.headlessHeadPossibilities.Evaluate(_random);
		WeaponReference weaponToTake = Singleton<Service>.Instance.gearManager.GetWeaponToTake(_random, rarity);
		WeaponInventory weapon2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
		while (weapon2.weapons.Where((Weapon weapon) => (Object)(object)weapon != (Object)null).Count() <= 1 && weaponToTake.name.Equals("BombSkul", StringComparison.OrdinalIgnoreCase))
		{
			weaponToTake = Singleton<Service>.Instance.gearManager.GetWeaponToTake(_random, rarity);
		}
		WeaponRequest request = weaponToTake.LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		_displayedGear = levelManager.DropWeapon(request, _slot.position);
		_displayedGear.destructible = false;
		_displayedGear.dropped.dropMovement.Stop();
		_displayedGear.dropped.dropMovement.Float();
	}

	protected override void OnActivate()
	{
		((Component)_lineText).gameObject.SetActive(true);
		_talk.SetActive(true);
		((Renderer)_headSlot).enabled = false;
		((MonoBehaviour)this).StartCoroutine(CDropGear());
		Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.onChanged += OnWeaponChanged;
	}

	protected override void OnDeactivate()
	{
		((Component)_lineText).gameObject.SetActive(false);
		((Renderer)_headSlot).enabled = false;
	}
}
