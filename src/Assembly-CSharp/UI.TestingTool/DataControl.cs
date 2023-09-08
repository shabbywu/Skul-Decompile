using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Gear;
using Characters.Gear.Upgrades;
using CutScenes;
using Data;
using GameResources;
using Level;
using Level.Npc;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool;

public sealed class DataControl : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static UnityAction _003C_003E9__10_0;

		internal void _003CAwake_003Eb__10_0()
		{
			GameData.Save.instance.ResetRandomSeed();
		}
	}

	[SerializeField]
	private Button _resetSeed;

	[SerializeField]
	private Button _firstClear;

	[SerializeField]
	private Button _resetProgress;

	[SerializeField]
	private Button _randomItem;

	[SerializeField]
	private Button _unlockAllItems;

	[SerializeField]
	private Button _unlockAllUpgrades;

	[SerializeField]
	private Button _lockAllUpgrades;

	[SerializeField]
	private Button _itemReset;

	[SerializeField]
	private Button _upgradeReset;

	[SerializeField]
	private Button _allGearReset;

	private void Awake()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		ButtonClickedEvent onClick = _resetSeed.onClick;
		object obj = _003C_003Ec._003C_003E9__10_0;
		if (obj == null)
		{
			UnityAction val = delegate
			{
				GameData.Save.instance.ResetRandomSeed();
			};
			_003C_003Ec._003C_003E9__10_0 = val;
			obj = (object)val;
		}
		((UnityEvent)onClick).AddListener((UnityAction)obj);
		((UnityEvent)_firstClear.onClick).AddListener(new UnityAction(ClearFirst));
		((UnityEvent)_resetProgress.onClick).AddListener((UnityAction)delegate
		{
			GameData.Generic.ResetAll();
			GameData.Currency.ResetAll();
			GameData.Progress.ResetAll();
			GameData.Gear.ResetAll();
			GameData.Save.instance.ResetAll();
			GameData.HardmodeProgress.ResetAll();
			levelManager.player.playerComponents.savableAbilityManager.ResetAll();
		});
		((UnityEvent)_randomItem.onClick).AddListener(new UnityAction(EquipRandomItem));
		((UnityEvent)_unlockAllItems.onClick).AddListener(new UnityAction(UnlockAllItems));
		((UnityEvent)_unlockAllUpgrades.onClick).AddListener(new UnityAction(UnlockAllUpgrades));
		((UnityEvent)_lockAllUpgrades.onClick).AddListener(new UnityAction(LockAllUpgrades));
		((UnityEvent)_itemReset.onClick).AddListener((UnityAction)delegate
		{
			levelManager.player.playerComponents.inventory.item.RemoveAll();
		});
		((UnityEvent)_upgradeReset.onClick).AddListener((UnityAction)delegate
		{
			levelManager.player.playerComponents.inventory.upgrade.RemoveAll();
		});
		((UnityEvent)_allGearReset.onClick).AddListener((UnityAction)delegate
		{
			levelManager.player.playerComponents.inventory.weapon.LoseAll();
			levelManager.player.playerComponents.inventory.quintessence.Remove(0);
			levelManager.player.playerComponents.inventory.item.RemoveAll();
			levelManager.player.playerComponents.inventory.upgrade.RemoveAll();
		});
	}

	private void UnlockAllItems()
	{
		string typeName = Gear.Type.Item.ToString();
		foreach (ItemReference item in GearResource.instance.items)
		{
			if (item.needUnlock)
			{
				GameData.Gear.SetUnlocked(typeName, item.name, value: true);
			}
		}
		typeName = Gear.Type.Weapon.ToString();
		foreach (WeaponReference weapon in GearResource.instance.weapons)
		{
			if (weapon.needUnlock)
			{
				GameData.Gear.SetUnlocked(typeName, weapon.name, value: true);
			}
		}
		typeName = Gear.Type.Quintessence.ToString();
		foreach (EssenceReference essence in GearResource.instance.essences)
		{
			if (essence.needUnlock)
			{
				GameData.Gear.SetUnlocked(typeName, essence.name, value: true);
			}
		}
	}

	private void UnlockAllUpgrades()
	{
		string typeName = Gear.Type.Upgrade.ToString();
		foreach (UpgradeResource.Reference upgradeReference in UpgradeResource.instance.upgradeReferences)
		{
			GameData.Gear.SetUnlocked(typeName, upgradeReference.name, value: true);
		}
	}

	private void LockAllUpgrades()
	{
		string typeName = Gear.Type.Upgrade.ToString();
		foreach (UpgradeResource.Reference upgradeReference in UpgradeResource.instance.upgradeReferences)
		{
			if (upgradeReference.needUnlock)
			{
				GameData.Gear.SetUnlocked(typeName, upgradeReference.name, value: false);
				GameData.Gear.SetFounded(typeName, upgradeReference.name, value: false);
			}
		}
	}

	private void ClearFirst()
	{
		GameData.Generic.tutorial.End();
		GameData.Progress.cutscene.SetData(Key.rookieHero, value: true);
		GameData.Progress.cutscene.SetData(Key.veterantHero, value: true);
		GameData.Progress.cutscene.SetData(Key.yggdrasill_Outro, value: true);
		GameData.Progress.cutscene.SetData(Key.leiana_Intro, value: true);
		GameData.Progress.cutscene.SetData(Key.leiana_Outro, value: true);
		GameData.Progress.cutscene.SetData(Key.chimera_Intro, value: true);
		GameData.Progress.cutscene.SetData(Key.chimera_Outro, value: true);
		GameData.Progress.cutscene.SetData(Key.pope_Intro, value: true);
		GameData.Progress.cutscene.SetData(Key.pope2Phase_Intro, value: true);
		GameData.Progress.cutscene.SetData(Key.pope_Outro, value: true);
		GameData.Progress.cutscene.SetData(Key.firstHero_Intro, value: true);
		GameData.Progress.cutscene.SetData(Key.firstHero2Phase_Intro, value: true);
		GameData.Progress.cutscene.SetData(Key.firstHero3Phase_Intro, value: true);
		GameData.Progress.cutscene.SetData(Key.firstHero_Outro, value: true);
		GameData.Progress.cutscene.SetData(Key.ending, value: true);
		GameData.Progress.cutscene.SetData(Key.masteryTutorial, value: true);
		GameData.Progress.cutscene.SetData(Key.strangeCat, value: true);
		GameData.Progress.cutscene.SetData(Key.arachne, value: true);
		GameData.Progress.skulstory.SetDataAll(value: true);
		GameData.Progress.SetRescued(NpcType.Fox, value: true);
		GameData.Progress.SetRescued(NpcType.Ogre, value: true);
		GameData.Progress.SetRescued(NpcType.Druid, value: true);
		GameData.Progress.SetRescued(NpcType.DeathKnight, value: true);
		GameData.Progress.arachneTutorial = true;
		GameData.Generic.normalEnding = true;
	}

	private void EquipRandomItem()
	{
		((MonoBehaviour)this).StartCoroutine(CEquip());
	}

	private IEnumerator CEquip()
	{
		ItemReference itemReference = GearResource.instance.items.Random();
		ItemRequest request = itemReference.LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		levelManager.DropItem(request, ((Component)levelManager.player).transform.position);
	}
}
