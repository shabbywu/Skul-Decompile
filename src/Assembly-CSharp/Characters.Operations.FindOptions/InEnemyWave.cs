using System;
using System.Collections.Generic;
using Level;

namespace Characters.Operations.FindOptions;

[Serializable]
public class InEnemyWave : IScope
{
	public List<Character> GetEnemyList()
	{
		return Map.Instance.waveContainer.GetAllEnemies();
	}
}
