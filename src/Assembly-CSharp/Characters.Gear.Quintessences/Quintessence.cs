using System;
using Characters.Abilities;
using Characters.Cooldowns;
using Characters.Gear.Quintessences.Constraints;
using Characters.Gear.Quintessences.Effects;
using Characters.Player;
using Data;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Quintessences;

public class Quintessence : Gear
{
	private class Assets
	{
		internal static EffectInfo destryoEssence = new EffectInfo(CommonResource.instance.destroyEssence);
	}

	[SerializeField]
	private CooldownSerializer _cooldown;

	[SerializeField]
	[Constraint.Subcomponent]
	private Constraint.Subcomponents _constraints;

	[QuintessenceEffect.Subcomponent]
	[SerializeField]
	private QuintessenceEffect.Subcomponents _passive;

	[SerializeField]
	[QuintessenceEffect.Subcomponent]
	private QuintessenceEffect.Subcomponents _active;

	public override Type type => Type.Quintessence;

	public CooldownSerializer cooldown => _cooldown;

	public Constraint.Subcomponents constraints => _constraints;

	protected override string _prefix => "quintessence";

	public string activeName => Localization.GetLocalizedString(base._keyBase + "/active/name");

	public string activeDescription => Localization.GetLocalizedString(base._keyBase + "/active/desc");

	public Sprite hudIcon => GearResource.instance.GetQuintessenceHudIcon(((Object)this).name) ?? base.icon;

	public override int currencyByDiscard => base.rarity switch
	{
		Rarity.Common => 3, 
		Rarity.Rare => 5, 
		Rarity.Unique => 10, 
		Rarity.Legendary => 15, 
		_ => 0, 
	};

	public static string currencySpriteKey => "<sprite name=\"Others/Heart_Icon\">";

	public static string currencyTextColorCode => "AF00C5";

	public event Action onUse;

	protected override void OnLoot(Character character)
	{
		QuintessenceInventory quintessence = character.playerComponents.inventory.quintessence;
		if (!quintessence.TryEquip(this))
		{
			quintessence.EquipAt(this, 0);
		}
	}

	public void SetOwner(Character character)
	{
		base.owner = character;
		_passive?.Invoke(this);
	}

	protected override void Awake()
	{
		base.Awake();
		Singleton<Service>.Instance.gearManager.RegisterEssenceInstance(this);
		_cooldown.Serialize();
	}

	private void OnDestroy()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		if (Service.quitting)
		{
			return;
		}
		Singleton<Service>.Instance.gearManager.UnregisterEssenceInstance(this);
		_onDiscard?.Invoke(this);
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if (!((Object)(object)levelManager.player == (Object)null) && levelManager.player.liveAndActive && destructible)
		{
			int num = currencyByDiscard;
			levelManager.player.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.EssenceSpirit, num);
			Collider2D component = ((Component)base.dropped).GetComponent<Collider2D>();
			if ((Object)(object)component == (Object)null)
			{
				Assets.destryoEssence.Spawn(((Component)this).transform.position);
			}
			else
			{
				EffectInfo destryoEssence = Assets.destryoEssence;
				Bounds bounds = component.bounds;
				destryoEssence.Spawn(((Bounds)(ref bounds)).center);
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, ((Component)this).transform.position);
			GameData.Progress.encounterEssenceCount++;
		}
	}

	protected override void OnEquipped()
	{
		base.OnEquipped();
		if (_cooldown.type == CooldownSerializer.Type.Time)
		{
			_cooldown.time.GetCooldownSpeed = base.owner.stat.GetQuintessenceCooldownSpeed;
		}
	}

	public void Use()
	{
		if (_constraints.components.Pass() && _cooldown.Consume())
		{
			_active?.Invoke(this);
			this.onUse?.Invoke();
		}
	}

	public void ChangeOnInventory(Quintessence item)
	{
		if (base.state == State.Dropped)
		{
			Debug.LogError((object)("Tried change essence " + ((Object)((Component)this).gameObject).name + " but it's not on inventory"));
			return;
		}
		base.owner.playerComponents.inventory.quintessence.Change(this, item);
		item.owner = base.owner;
		item.state = State.Equipped;
	}

	public Quintessence Instantiate()
	{
		Quintessence quintessence = Object.Instantiate<Quintessence>(this);
		((Object)quintessence).name = ((Object)this).name;
		quintessence.Initialize();
		return quintessence;
	}
}
