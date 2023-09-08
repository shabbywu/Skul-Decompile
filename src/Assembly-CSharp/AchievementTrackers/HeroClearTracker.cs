using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear.Weapons;
using Data;
using Level.Npc.FieldNpcs;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers;

public class HeroClearTracker : MonoBehaviour
{
	[SerializeField]
	private Type _normalAchievement;

	[SerializeField]
	private Type _perfectAchievement;

	[SerializeField]
	private Character _boss;

	[SerializeField]
	private Weapon _littleBone;

	private bool _tookDamage;

	private void Start()
	{
		Singleton<Service>.Instance.levelManager.player.health.onTookDamage += OnTookDamage;
		_boss.health.onDied += OnTargetDied;
	}

	private void OnDestroy()
	{
		Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= OnTookDamage;
		_boss.health.onDied -= OnTargetDied;
	}

	private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (tookDamage.attackType != 0 && !(damageDealt < 1.0))
		{
			_tookDamage = true;
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= OnTookDamage;
		}
	}

	private void OnTargetDied()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_boss.health.onDied -= OnTargetDied;
		ExtensionMethods.Set(_normalAchievement);
		if (!_tookDamage)
		{
			ExtensionMethods.Set(_perfectAchievement);
		}
		if (((Object)Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.current).name.Equals(((Object)_littleBone).name))
		{
			ExtensionMethods.Set((Type)16);
		}
		if (!GameData.Progress.fieldNpcEncountered.Any((KeyValuePair<NpcType, BoolData> kvp) => kvp.Value.value))
		{
			ExtensionMethods.Set((Type)17);
		}
	}
}
