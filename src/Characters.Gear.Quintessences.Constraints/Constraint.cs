using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences.Constraints;

public abstract class Constraint : MonoBehaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<Constraint>
	{
	}

	public static readonly Type[] types = new Type[2]
	{
		typeof(EnemyWithinRangeConstraint),
		typeof(EnemyCountConstraint)
	};

	public abstract bool Pass();

	public override string ToString()
	{
		return ExtensionMethods.GetAutoName((object)this);
	}
}
