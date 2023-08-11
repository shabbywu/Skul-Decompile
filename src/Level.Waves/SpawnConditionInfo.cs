using System;
using UnityEngine;

namespace Level.Waves;

public sealed class SpawnConditionInfo : MonoBehaviour
{
	[Serializable]
	internal class Subcomponents : SubcomponentArray<SpawnConditionInfo>
	{
	}

	[SpawnCondition.Subcomponent(true)]
	[SerializeField]
	private SpawnCondition _condition;

	[SerializeField]
	private string _tag;

	public bool IsSatisfied(EnemyWave wave)
	{
		return _condition.IsSatisfied(wave);
	}

	public override string ToString()
	{
		if (_tag != null && _tag.Length != 0)
		{
			return _tag;
		}
		return ExtensionMethods.GetAutoName((object)this);
	}
}
