using System;
using Level;

namespace Characters.Operations.Summon;

[Serializable]
public class ContainInWave : IBDCharacterSetting
{
	public void ApplyTo(Character character)
	{
		Map.Instance.waveContainer.Attach(character);
	}
}
