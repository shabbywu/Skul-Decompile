using Characters;
using Data;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers;

public sealed class DarkHearoClearTracker : MonoBehaviour
{
	[SerializeField]
	private Type _perfectAchievement;

	[SerializeField]
	private Character _boss;

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
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_boss.health.onDied -= OnTargetDied;
		if (!_tookDamage)
		{
			ExtensionMethods.Set(_perfectAchievement);
		}
		switch (GameData.HardmodeProgress.hardmodeLevel)
		{
		case 1:
			ExtensionMethods.Set((Type)76);
			break;
		case 2:
			ExtensionMethods.Set((Type)77);
			break;
		case 3:
			ExtensionMethods.Set((Type)78);
			break;
		case 4:
			ExtensionMethods.Set((Type)79);
			break;
		case 5:
			ExtensionMethods.Set((Type)80);
			break;
		case 6:
			ExtensionMethods.Set((Type)81);
			break;
		case 7:
			ExtensionMethods.Set((Type)82);
			break;
		case 8:
			ExtensionMethods.Set((Type)83);
			break;
		case 9:
			ExtensionMethods.Set((Type)84);
			break;
		case 10:
			ExtensionMethods.Set((Type)85);
			break;
		}
	}
}
