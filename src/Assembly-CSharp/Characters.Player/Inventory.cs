using System;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using Data;
using Hardmode;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Player;

public class Inventory
{
	public readonly Synergy synergy;

	public readonly WeaponInventory weapon;

	public readonly ItemInventory item;

	public readonly QuintessenceInventory quintessence;

	public readonly UpgradeInventory upgrade;

	private Character _character;

	public event Action onUpdated;

	public event Action onUpdatedKeywordCounts;

	public Inventory(Character character)
	{
		_character = character;
		synergy = ((Component)character).GetComponent<Synergy>();
		weapon = ((Component)character).GetComponent<WeaponInventory>();
		item = ((Component)character).GetComponent<ItemInventory>();
		quintessence = ((Component)character).GetComponent<QuintessenceInventory>();
		upgrade = ((Component)character).GetComponent<UpgradeInventory>();
	}

	public void Initialize()
	{
		synergy.Initialize(_character);
		onUpdated += UpdateSynergy;
		weapon.onChanged += delegate
		{
			this.onUpdated();
		};
		item.onChanged += delegate
		{
			this.onUpdated();
		};
		quintessence.onChanged += delegate
		{
			this.onUpdated();
		};
	}

	public void UpdateSynergy()
	{
		foreach (Inscription inscription in synergy.inscriptions)
		{
			inscription.count = 0;
		}
		foreach (Item item in item.items)
		{
			if (!((Object)(object)item == (Object)null))
			{
				synergy.inscriptions[item.keyword1].count++;
				synergy.inscriptions[item.keyword2].count++;
			}
		}
		foreach (Item item2 in this.item.items)
		{
			if ((Object)(object)item2 == (Object)null)
			{
				continue;
			}
			Item.BonusKeyword[] bonusKeyword = item2.bonusKeyword;
			foreach (Item.BonusKeyword obj in bonusKeyword)
			{
				obj.Evaluate();
				EnumArray<Inscription.Key, int> values = obj.Values;
				foreach (Inscription.Key key in Inscription.keys)
				{
					synergy.inscriptions[key].count += values[key];
				}
			}
		}
		foreach (Inscription.Key key2 in Inscription.keys)
		{
			synergy.inscriptions[key2].count += synergy.inscriptions[key2].bonusCount;
		}
		this.onUpdatedKeywordCounts?.Invoke();
		synergy.UpdateBonus();
	}

	public void Save()
	{
		GameData.Save instance = GameData.Save.instance;
		instance.currentWeapon = ((Object)weapon.current).name;
		instance.currentWeaponSkill1 = weapon.current.GetSkillWithoutSkillChanges(0).key;
		if (weapon.current.currentSkills.Count <= 1)
		{
			instance.currentWeaponSkill2 = string.Empty;
		}
		else
		{
			instance.currentWeaponSkill2 = weapon.current.GetSkillWithoutSkillChanges(1).key;
		}
		IStackable componentInChildren = ((Component)weapon.current).GetComponentInChildren<IStackable>();
		if (componentInChildren == null)
		{
			instance.currentWeaponStack = 0f;
		}
		else
		{
			instance.currentWeaponStack = componentInChildren.stack;
		}
		if ((Object)(object)weapon.next == (Object)null)
		{
			instance.nextWeapon = string.Empty;
			instance.nextWeaponSkill1 = string.Empty;
			instance.nextWeaponSkill1 = string.Empty;
		}
		else
		{
			weapon.next.UnapplyAllSkillChanges();
			instance.nextWeapon = ((Object)weapon.next).name;
			instance.nextWeaponSkill1 = weapon.next.currentSkills[0].key;
			if (weapon.next.currentSkills.Count <= 1)
			{
				instance.nextWeaponSkill2 = string.Empty;
			}
			else
			{
				instance.nextWeaponSkill2 = weapon.next.currentSkills[1].key;
			}
			weapon.next.ApplyAllSkillChanges();
			IStackable componentInChildren2 = ((Component)weapon.next).GetComponentInChildren<IStackable>();
			if (componentInChildren2 == null)
			{
				instance.nextWeaponStack = 0f;
			}
			else
			{
				instance.nextWeaponStack = componentInChildren2.stack;
			}
		}
		if ((Object)(object)this.quintessence.items[0] == (Object)null)
		{
			instance.essence = string.Empty;
		}
		else
		{
			Quintessence quintessence = this.quintessence.items[0];
			instance.essence = ((Object)quintessence).name;
			IStackable componentInChildren3 = ((Component)quintessence).GetComponentInChildren<IStackable>();
			if (componentInChildren3 == null)
			{
				instance.essenceStack = 0f;
			}
			else
			{
				instance.essenceStack = componentInChildren3.stack;
			}
		}
		for (int i = 0; i < instance.items.length; i++)
		{
			Item item = this.item.items[i];
			if ((Object)(object)item == (Object)null)
			{
				instance.items[i] = string.Empty;
				instance.itemStacks[i] = 0f;
				instance.itemKeywords1[i] = 0;
				instance.itemKeywords2[i] = 0;
				continue;
			}
			instance.items[i] = ((Object)item).name;
			IStackable componentInChildren4 = ((Component)item).GetComponentInChildren<IStackable>();
			if (componentInChildren4 == null)
			{
				instance.itemStacks[i] = 0f;
			}
			else
			{
				instance.itemStacks[i] = componentInChildren4.stack;
			}
			instance.itemKeywords1[i] = (int)item.keyword1;
			instance.itemKeywords2[i] = (int)item.keyword2;
		}
		if (!GameData.HardmodeProgress.hardmode)
		{
			return;
		}
		for (int j = 0; j < instance.upgrades.length; j++)
		{
			UpgradeObject upgradeObject = upgrade.upgrades[j];
			if ((Object)(object)upgradeObject == (Object)null)
			{
				instance.upgrades[j] = string.Empty;
				instance.upgradeLevels[j] = 0;
				instance.upgradeStacks[j] = 0f;
				continue;
			}
			instance.upgrades[j] = ((Object)upgradeObject).name;
			instance.upgradeLevels[j] = upgradeObject.level;
			IStackable componentInChildren5 = ((Component)upgradeObject.GetOrigin().GetCurrentAbility()).GetComponentInChildren<IStackable>();
			if (componentInChildren5 == null)
			{
				instance.upgradeStacks[j] = 0f;
			}
			else
			{
				instance.upgradeStacks[j] = componentInChildren5.stack;
			}
		}
	}

	public void LoadFromSave()
	{
		GameData.Save instance = GameData.Save.instance;
		GameResourceLoader instance2 = GameResourceLoader.instance;
		LoadSavedWeaponFromPreloader(instance, instance2);
		LoadSavedEssenceFromPreloader(instance, instance2);
		LoadSavedItemsFromPreloader(instance, instance2);
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			LoadSavedUpgradeFromPreloader(instance);
		}
	}

	private void LoadSavedUpgradeFromPreloader(GameData.Save save)
	{
		for (int i = 0; i < GameData.HardmodeProgress.InscriptionSynthesisEquipment.count; i++)
		{
			int value = GameData.HardmodeProgress.inscriptionSynthesisEquipment[i].value;
			if (value != -1)
			{
				synergy.inscriptions[(Inscription.Key)value].bonusCount += InscriptionSynthesisEquipment.increasement;
			}
		}
		UpdateSynergy();
		for (int j = 0; j < save.upgrades.length; j++)
		{
			string text = save.upgrades[j];
			if (string.IsNullOrEmpty(text))
			{
				continue;
			}
			UpgradeResource.Reference reference = Singleton<UpgradeManager>.Instance.FindByName(text);
			upgrade.EquipAt(reference, j);
			int level = save.upgradeLevels[j];
			UpgradeObject upgradeObject = upgrade.upgrades[j];
			upgradeObject.SetLevel(level);
			UpgradeAbility currentAbility = upgradeObject.GetCurrentAbility();
			if ((Object)(object)currentAbility != (Object)null)
			{
				IStackable componentInChildren = ((Component)currentAbility).GetComponentInChildren<IStackable>();
				if (componentInChildren != null)
				{
					componentInChildren.stack = save.upgradeStacks[j];
				}
			}
		}
	}

	private void LoadSavedWeaponFromPreloader(GameData.Save save, GameResourceLoader preloader)
	{
		weapon.LoseAll();
		Weapon weaponInstance2 = preloader.TakeWeapon2();
		string skill3 = save.nextWeaponSkill1;
		string skill4 = save.nextWeaponSkill2;
		LoadWeapon(weaponInstance2, 1, in skill3, in skill4, save.nextWeaponStack);
		Weapon weaponInstance3 = preloader.TakeWeapon1();
		skill3 = save.currentWeaponSkill1;
		skill4 = save.currentWeaponSkill2;
		LoadWeapon(weaponInstance3, 0, in skill3, in skill4, save.currentWeaponStack);
		void LoadWeapon(Weapon weaponInstance, int index, in string skill1, in string skill2, float stack)
		{
			if (!((Object)(object)weaponInstance == (Object)null))
			{
				weapon.ForceEquipAt(weaponInstance, index);
				if (string.IsNullOrEmpty(skill2))
				{
					weaponInstance.SetSkills(new string[1] { skill1 }, ignoreLevel: false);
				}
				else
				{
					weaponInstance.SetSkills(new string[2] { skill1, skill2 }, ignoreLevel: false);
				}
				if (stack > 0f)
				{
					IStackable componentInChildren = ((Component)weaponInstance).GetComponentInChildren<IStackable>();
					if (componentInChildren != null)
					{
						componentInChildren.stack = stack;
					}
				}
			}
		}
	}

	private void LoadSavedEssenceFromPreloader(GameData.Save save, GameResourceLoader preloader)
	{
		Quintessence quintessence = preloader.TakeEssence();
		if (!((Object)(object)quintessence != (Object)null))
		{
			return;
		}
		this.quintessence.EquipAt(quintessence, 0);
		if (save.essenceStack > 0f)
		{
			IStackable componentInChildren = ((Component)quintessence).GetComponentInChildren<IStackable>();
			if (componentInChildren != null)
			{
				componentInChildren.stack = save.essenceStack;
			}
		}
	}

	private void LoadSavedItemsFromPreloader(GameData.Save save, GameResourceLoader preloader)
	{
		for (int i = 0; i < save.items.length; i++)
		{
			Item item = preloader.TakeItem(i);
			if ((Object)(object)item == (Object)null)
			{
				continue;
			}
			Inscription.Key key = (Inscription.Key)save.itemKeywords1[i];
			Inscription.Key key2 = (Inscription.Key)save.itemKeywords2[i];
			if (key != 0)
			{
				item.keyword1 = key;
			}
			if (key2 != 0)
			{
				item.keyword2 = key2;
			}
			this.item.EquipAt(item, i);
			if (save.itemStacks[i] > 0f)
			{
				IStackable componentInChildren = ((Component)item).GetComponentInChildren<IStackable>();
				if (componentInChildren != null)
				{
					componentInChildren.stack = save.itemStacks[i];
				}
			}
		}
	}
}
