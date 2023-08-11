using System;
using Characters;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Level;

public class DroppedWeapon : InteractiveObject
{
	[NonSerialized]
	public Weapon weapon;

	[SerializeField]
	private PoolObject _effect;

	[GetComponent]
	[SerializeField]
	private DropMovement _dropMovement;

	protected override void Awake()
	{
		base.Awake();
		_dropMovement.onGround += Activate;
	}

	private void OnEnable()
	{
		Deactivate();
	}

	public override void InteractWith(Character character)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		character.playerComponents.inventory.weapon.Equip(weapon);
		_effect.Spawn(((Component)this).transform.position, true);
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
