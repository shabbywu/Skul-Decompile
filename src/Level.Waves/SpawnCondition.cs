using System;
using UnityEditor;
using UnityEngine;

namespace Level.Waves;

public abstract class SpawnCondition : MonoBehaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public static readonly Type[] types = new Type[9]
		{
			typeof(Always),
			typeof(EnterZone),
			typeof(HardModeLevel),
			typeof(RemainEnemies),
			typeof(ReturnFail),
			typeof(Sequence),
			typeof(Selector),
			typeof(TimeOut),
			typeof(TimeRemain)
		};

		public SubcomponentAttribute(bool allowCustom = true)
			: base(allowCustom, types)
		{
		}
	}

	[SerializeField]
	private bool _inverter;

	protected abstract bool Check(EnemyWave wave);

	public bool IsSatisfied(EnemyWave wave)
	{
		return _inverter ^ Check(wave);
	}
}
