using System;
using Runnables.Triggers.Customs;
using UnityEditor;
using UnityEngine;

namespace Runnables.Triggers;

public abstract class Trigger : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	public static readonly Type[] types = new Type[27]
	{
		typeof(Always),
		typeof(ActivatedSkulStory),
		typeof(ByPosition),
		typeof(CageOnDestroyed),
		typeof(CharacterDied),
		typeof(EnterZone),
		typeof(EqualsState),
		typeof(EqualWeaponName),
		typeof(HasCurrency),
		typeof(Inverter),
		typeof(MapRewardActivated),
		typeof(StoppedEnemyContainer),
		typeof(Sequence),
		typeof(Timer),
		typeof(WaveOnSpawn),
		typeof(WaveOnClear),
		typeof(PlayedCutScene),
		typeof(PlayedSkulStory),
		typeof(PlayedTutorial),
		typeof(CharacterActionRunning),
		typeof(HasBDVariable),
		typeof(ClearedLevelCompare),
		typeof(HardmodeLevelCompare),
		typeof(DarktechUnlocked),
		typeof(DarktechActivated),
		typeof(Mode),
		typeof(NormalEnding)
	};

	protected abstract bool Check();

	public bool IsSatisfied()
	{
		return Check();
	}
}
