using Data;
using UnityEngine;

namespace Level.Waves;

public sealed class HardModeLevel : Leaf
{
	private enum Compare
	{
		Greater,
		Equal,
		Lower
	}

	[SerializeField]
	private Compare _compare;

	[SerializeField]
	[Range(0f, 10f)]
	private int _level;

	protected override bool Check(EnemyWave wave)
	{
		return _compare switch
		{
			Compare.Equal => _level == GameData.HardmodeProgress.hardmodeLevel, 
			Compare.Greater => _level < GameData.HardmodeProgress.hardmodeLevel, 
			Compare.Lower => _level > GameData.HardmodeProgress.hardmodeLevel, 
			_ => true, 
		};
	}
}
