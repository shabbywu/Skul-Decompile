using UnityEditor;
using UnityEngine;

namespace Level.Waves;

public class Selector : Decorator
{
	[UnityEditor.Subcomponent(typeof(SpawnConditionInfo))]
	[SerializeField]
	private SpawnConditionInfo.Subcomponents _conditions;

	protected override bool Check(EnemyWave wave)
	{
		SpawnConditionInfo[] components = _conditions.components;
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].IsSatisfied(wave))
			{
				return true;
			}
		}
		return false;
	}
}
