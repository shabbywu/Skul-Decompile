using Characters;
using Characters.Operations;
using Level;
using UnityEngine;

public class AttachToSummonEnemyWave : CharacterOperation
{
	[SerializeField]
	private Character _character;

	public override void Run(Character owner)
	{
		Map.Instance.waveContainer.AttachToSummonEnemyWave(_character);
	}
}
