using System;
using Characters;
using Characters.Gear.Items;
using Characters.Player;
using Singletons;
using UnityEngine;

namespace Level;

public class DroppedItem : InteractiveObject
{
	[NonSerialized]
	public Item item;

	[SerializeField]
	private PoolObject _effect;

	[SerializeField]
	[GetComponent]
	private DropMovement _dropMovement;

	[SerializeField]
	private AudioClip _clip;

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
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_clip != (Object)null)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_clip, ((Component)this).transform.position);
		}
		ItemInventory itemInventory = character.playerComponents.inventory.item;
		if (!itemInventory.TryEquip(item))
		{
			itemInventory.EquipAt(item, 0);
		}
		_effect.Spawn(((Component)this).transform.position, true);
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
