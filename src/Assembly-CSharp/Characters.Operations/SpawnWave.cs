using BehaviorDesigner.Runtime;
using Level;
using UnityEngine;

namespace Characters.Operations;

public class SpawnWave : CharacterOperation
{
	private readonly string TargetBDWaveKeyVariable = "WaveKey";

	[SerializeField]
	private bool _effectOn;

	public override void Run(Character owner)
	{
		EnemyWaveContainer waveContainer = Map.Instance.waveContainer;
		string text = (string)((Component)owner).GetComponent<BehaviorDesignerCommunicator>().GetVariable(TargetBDWaveKeyVariable).GetValue();
		EnemyWave[] enemyWaves = waveContainer.enemyWaves;
		foreach (EnemyWave enemyWave in enemyWaves)
		{
			string[] keys = enemyWave.keys;
			foreach (string text2 in keys)
			{
				if (text == text2)
				{
					enemyWave.Spawn(_effectOn);
					return;
				}
			}
		}
		Debug.LogError((object)(((Object)owner).name + "의 BehaviorDesignerCommunicator의 웨이브 Key값에 해당되는 EnemyWave key : " + text + "를 찾을 수 없습니다."));
	}
}
