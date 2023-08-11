using System;
using System.Collections.ObjectModel;
using Characters;
using Characters.Gear.Weapons;
using CutScenes;
using Data;
using Level;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers;

public class PlayerAchievementTracker : MonoBehaviour
{
	[SerializeField]
	private Character _player;

	private void Awake()
	{
		_player.health.onDied += OnPlayerDied;
		_player.health.onTookDamage += OnTookDamage;
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += TrackMapAchievement;
		_player.playerComponents.inventory.weapon.onChanged += TrackHeadLootAchievement;
		GameData.Currency.heartQuartz.onEarn += HandleOnEarnHeartQuartz;
	}

	private void HandleOnEarnHeartQuartz(int amount)
	{
		if (GameData.Currency.heartQuartz.totalIncome >= 100)
		{
			ExtensionMethods.Set((Type)67);
		}
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= TrackMapAchievement;
		}
	}

	private void OnPlayerDied()
	{
		ExtensionMethods.Set((Type)0);
	}

	private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!(_player.health.currentHealth > 0.0))
		{
			bool flag = (Object)(object)tookDamage.attacker.trap != (Object)null;
			if ((Object)(object)tookDamage.attacker.character != (Object)null && tookDamage.attacker.character.type == Character.Type.Trap)
			{
				flag = true;
			}
			if (flag)
			{
				ExtensionMethods.Set((Type)2);
			}
		}
	}

	private void TrackMapAchievement(Map old, Map @new)
	{
		if (Singleton<Service>.Instance.levelManager.currentChapter.type == Chapter.Type.Castle && GameData.Progress.cutscene.GetData(CutScenes.Key.ending))
		{
			ExtensionMethods.Set((Type)3);
		}
	}

	private void TrackHeadLootAchievement(Weapon old, Weapon @new)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)@new == (Object)null || (int)@new.rarity != 3)
		{
			return;
		}
		ReadOnlyCollection<string> names = EnumValues<Type>.Names;
		int num = -1;
		for (int i = 0; i < names.Count; i++)
		{
			if (((Object)@new).name.IndexOf(names[i], StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			Debug.Log((object)("There is no achievement for Legendary Head " + ((Object)@new).name + "."));
		}
		else
		{
			ExtensionMethods.Set(EnumValues<Type>.Values[num]);
		}
	}
}
