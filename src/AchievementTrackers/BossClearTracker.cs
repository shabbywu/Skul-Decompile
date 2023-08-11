using Characters;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers;

public class BossClearTracker : MonoBehaviour
{
	[SerializeField]
	private Type _normalAchievement;

	[SerializeField]
	private Type _perfectAchievement;

	[SerializeField]
	private bool _onlyPerfect;

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
		if (!Service.quitting)
		{
			if ((Object)(object)Singleton<Service>.Instance.levelManager.player != (Object)null)
			{
				Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= OnTookDamage;
			}
			_boss.health.onDied -= OnTargetDied;
		}
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		_boss.health.onDied -= OnTargetDied;
		if (!_onlyPerfect)
		{
			ExtensionMethods.Set(_normalAchievement);
		}
		if (!_tookDamage)
		{
			ExtensionMethods.Set(_perfectAchievement);
		}
	}
}
