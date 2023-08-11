using BehaviorDesigner.Runtime;
using FX;
using Level;
using UnityEngine;

namespace Characters.Operations.Fx;

public class SpawnEffectAtEnemiesInEnemyWave : CharacterOperation
{
	private readonly string TargetBDWaveKeyVariable = "WaveKey";

	[SerializeField]
	private EffectInfo _info;

	public override void Run(Character owner)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		EnemyWaveContainer waveContainer = Map.Instance.waveContainer;
		string text = (string)((Component)owner).GetComponent<BehaviorDesignerCommunicator>().GetVariable(TargetBDWaveKeyVariable).GetValue();
		EnemyWave[] enemyWaves = waveContainer.enemyWaves;
		foreach (EnemyWave enemyWave in enemyWaves)
		{
			string[] keys = enemyWave.keys;
			foreach (string text2 in keys)
			{
				if (!(text == text2))
				{
					continue;
				}
				{
					foreach (Character character in enemyWave.characters)
					{
						_info.Spawn(((Component)character).transform.position);
					}
					return;
				}
			}
		}
		Debug.LogError((object)(((Object)owner).name + "의 BehaviorDesignerCommunicator의 웨이브 Key값에 해당되는 EnemyWave key : " + text + "를 찾을 수 없습니다."));
	}

	public override void Stop()
	{
		_info.DespawnChildren();
	}
}
