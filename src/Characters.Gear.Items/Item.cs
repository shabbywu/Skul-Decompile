using System;
using Characters.Abilities;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using Data;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Items;

public class Item : Gear
{
	private class Assets
	{
		internal static EffectInfo destroyItem = new EffectInfo(CommonResource.instance.destroyItem);
	}

	[Serializable]
	public class BonusKeyword
	{
		public enum Type
		{
			All,
			Possesion,
			Items,
			Single
		}

		public Type type;

		public int count;

		[Header("Type이 Single일 경우에만 입력")]
		public Inscription.Key keyword;

		private EnumArray<Inscription.Key, int> _keywordBonusCounts = new EnumArray<Inscription.Key, int>();

		private Item _ownerItem;

		public EnumArray<Inscription.Key, int> Values => _keywordBonusCounts;

		public void Initialize(Item ownerItem)
		{
			_ownerItem = ownerItem;
		}

		public void Evaluate()
		{
			Characters.Gear.Synergy.Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
			switch (type)
			{
			case Type.All:
			{
				foreach (Inscription.Key value in Enum.GetValues(typeof(Inscription.Key)))
				{
					_keywordBonusCounts[value] = count;
				}
				break;
			}
			case Type.Possesion:
			{
				foreach (Inscription.Key value2 in Enum.GetValues(typeof(Inscription.Key)))
				{
					if (synergy.inscriptions[value2].count > 0)
					{
						_keywordBonusCounts[value2] = count;
					}
					else
					{
						_keywordBonusCounts[value2] = 0;
					}
				}
				break;
			}
			case Type.Items:
				_keywordBonusCounts[_ownerItem.keyword1] = count;
				_keywordBonusCounts[_ownerItem.keyword2] = count;
				break;
			case Type.Single:
				_keywordBonusCounts[keyword] = count;
				break;
			}
		}

		public void Evaluate(EnumArray<Inscription.Key, int> keywordCounts)
		{
			switch (type)
			{
			case Type.All:
			{
				foreach (Inscription.Key value in Enum.GetValues(typeof(Inscription.Key)))
				{
					EnumArray<Inscription.Key, int> val = keywordCounts;
					Inscription.Key key = value;
					val[key] += count;
				}
				break;
			}
			case Type.Possesion:
			{
				foreach (Inscription.Key value2 in Enum.GetValues(typeof(Inscription.Key)))
				{
					if (keywordCounts[value2] > 0)
					{
						EnumArray<Inscription.Key, int> val = keywordCounts;
						Inscription.Key key = value2;
						val[key] += count;
					}
				}
				break;
			}
			case Type.Items:
			{
				EnumArray<Inscription.Key, int> val = keywordCounts;
				Inscription.Key key = _ownerItem.keyword1;
				val[key] += count;
				val = keywordCounts;
				key = _ownerItem.keyword2;
				val[key] += count;
				break;
			}
			case Type.Single:
			{
				EnumArray<Inscription.Key, int> val = keywordCounts;
				Inscription.Key key = keyword;
				val[key] += count;
				break;
			}
			}
		}

		public void Dispose()
		{
			if (type == Type.Possesion)
			{
				Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item.onChanged -= OnChangedInventory;
			}
		}

		private void OnChangedInventory()
		{
			Evaluate();
		}
	}

	public Inscription.Key keyword1;

	public Inscription.Key keyword2;

	public BonusKeyword[] bonusKeyword;

	[AbilityAttacher.Subcomponent]
	[SerializeField]
	private AbilityAttacher.Subcomponents _abilityAttacher;

	public override Type type => Type.Item;

	public override GameData.Currency.Type currencyTypeByDiscard => GameData.Currency.Type.Gold;

	public override int currencyByDiscard
	{
		get
		{
			if (base.dropped.price <= 0 && destructible)
			{
				return WitchBonus.instance.soul.ancientAlchemy.GetGoldByDiscard(this);
			}
			return 0;
		}
	}

	protected override string _prefix => "item";

	protected override void Awake()
	{
		base.Awake();
		Singleton<Service>.Instance.gearManager.RegisterItemInstance(this);
		BonusKeyword[] array = bonusKeyword;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize(this);
		}
	}

	private void OnDestroy()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected I4, but got Unknown
		if (Service.quitting)
		{
			return;
		}
		Singleton<Service>.Instance.gearManager.UnregisterItemInstance(this);
		_abilityAttacher.StopAttach();
		_onDiscard?.Invoke(this);
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if ((Object)(object)levelManager.player == (Object)null || !levelManager.player.liveAndActive)
		{
			return;
		}
		if (destructible)
		{
			GameData.Progress.encounterItemCount++;
			PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, ((Component)this).transform.position);
			Collider2D component = ((Component)base.dropped).GetComponent<Collider2D>();
			if ((Object)(object)component == (Object)null)
			{
				Assets.destroyItem.Spawn(((Component)this).transform.position);
			}
			else
			{
				EffectInfo destroyItem = Assets.destroyItem;
				Bounds bounds = component.bounds;
				destroyItem.Spawn(((Bounds)(ref bounds)).center);
			}
		}
		if (currencyByDiscard == 0)
		{
			return;
		}
		int count = 1;
		if (currencyByDiscard > 0)
		{
			Rarity val = base.rarity;
			switch ((int)val)
			{
			case 0:
				count = 5;
				break;
			case 1:
				count = 8;
				break;
			case 2:
				count = 15;
				break;
			case 3:
				count = 25;
				break;
			}
		}
		levelManager.DropGold(currencyByDiscard, count);
	}

	protected override void OnLoot(Character character)
	{
		character.playerComponents.inventory.item.TryEquip(this);
	}

	public void SetOwner(Character character)
	{
		base.owner = character;
	}

	protected override void OnEquipped()
	{
		base.OnEquipped();
		BonusKeyword[] array = bonusKeyword;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Evaluate();
		}
		_abilityAttacher.Initialize(base.owner);
		_abilityAttacher.StartAttach();
	}

	protected override void OnDropped()
	{
		base.OnDropped();
		_abilityAttacher.StopAttach();
	}

	public void DiscardOnInventory()
	{
		if (base.state == State.Dropped)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
			return;
		}
		ItemInventory item = base.owner.playerComponents.inventory.item;
		item.Discard(this);
		item.Trim();
		BonusKeyword[] array = bonusKeyword;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Dispose();
		}
	}

	public void RemoveOnInventory()
	{
		if (base.state == State.Dropped)
		{
			destructible = false;
			Object.Destroy((Object)(object)((Component)this).gameObject);
			return;
		}
		ItemInventory item = base.owner.playerComponents.inventory.item;
		item.Remove(this);
		item.Trim();
		BonusKeyword[] array = bonusKeyword;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Dispose();
		}
	}

	public void ChangeOnInventory(Item item)
	{
		if (base.state == State.Dropped)
		{
			Debug.LogError((object)("Tried change item " + ((Object)this).name + " but it's not on inventory"));
			return;
		}
		base.owner.playerComponents.inventory.item.Change(this, item);
		item.owner = base.owner;
		item.state = State.Equipped;
	}

	public void EvaluateBonusKeyword(EnumArray<Inscription.Key, int> keywordCounts)
	{
		BonusKeyword[] array = bonusKeyword;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Evaluate(keywordCounts);
		}
	}

	public Item Instantiate()
	{
		Item item = Object.Instantiate<Item>(this);
		((Object)item).name = ((Object)this).name;
		item.Initialize();
		return item;
	}
}
