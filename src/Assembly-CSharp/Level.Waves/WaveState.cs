using UnityEngine;

namespace Level.Waves;

public class WaveState : Leaf
{
	[SerializeField]
	private EnemyWave _target;

	[SerializeField]
	private Wave.State _state;

	protected override bool Check(EnemyWave wave)
	{
		return _target.state == _state;
	}
}
