using Data;
using Singletons;
using UnityEngine;

namespace Hardmode;

public sealed class HardmodeManager : Singleton<HardmodeManager>
{
	public enum EnemyStep
	{
		Normal,
		A,
		B,
		C
	}

	public int _maxLevel => GameData.HardmodeProgress.maxLevel;

	public int currentLevel => GameData.HardmodeProgress.hardmodeLevel;

	public int clearedLevel => GameData.HardmodeProgress.clearedLevel;

	public bool hardmode => GameData.HardmodeProgress.hardmode;

	protected override void Awake()
	{
		base.Awake();
		((Object)this).name = "HardmodeManager";
	}

	public void SetLevel(int level)
	{
		GameData.HardmodeProgress.hardmodeLevel = level;
	}

	public EnemyStep GetEnemyStep()
	{
		if (!hardmode)
		{
			return EnemyStep.Normal;
		}
		if (currentLevel < 4)
		{
			return EnemyStep.A;
		}
		if (currentLevel < 8)
		{
			return EnemyStep.B;
		}
		return EnemyStep.C;
	}
}
