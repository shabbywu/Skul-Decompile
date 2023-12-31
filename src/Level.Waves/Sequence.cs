using UnityEditor;
using UnityEngine;

namespace Level.Waves;

public class Sequence : Decorator
{
	[SerializeField]
	[UnityEditor.Subcomponent(typeof(SpawnConditionInfo))]
	private SpawnConditionInfo.Subcomponents _conditions;

	protected override bool Check(EnemyWave wave)
	{
		SpawnConditionInfo[] components = _conditions.components;
		for (int i = 0; i < components.Length; i++)
		{
			if (!components[i].IsSatisfied(wave))
			{
				return false;
			}
		}
		return true;
	}
}
