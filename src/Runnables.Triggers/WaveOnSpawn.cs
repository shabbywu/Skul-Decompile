using Level;
using UnityEngine;

namespace Runnables.Triggers;

public class WaveOnSpawn : Trigger
{
	[SerializeField]
	private EnemyWave _wave;

	private bool _result;

	private void Start()
	{
		_wave.onSpawn += delegate
		{
			_result = true;
		};
	}

	protected override bool Check()
	{
		return _result;
	}
}
