using UnityEngine;

namespace Level.Waves;

public sealed class RemainEnemies : Leaf
{
	[SerializeField]
	private EnemyWave _wave;

	[SerializeField]
	private int _remains;

	protected override bool Check(EnemyWave wave)
	{
		return _wave.remains <= _remains;
	}
}
