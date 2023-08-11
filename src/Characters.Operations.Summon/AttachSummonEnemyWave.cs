using System;
using Level;

namespace Characters.Operations.Summon;

[Serializable]
public class AttachSummonEnemyWave : IBDCharacterSetting
{
	public void ApplyTo(Character character)
	{
		Map.Instance.waveContainer.AttachToSummonEnemyWave(character);
	}
}
