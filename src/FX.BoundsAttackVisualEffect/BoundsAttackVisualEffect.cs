using System;
using Characters;
using UnityEditor;
using UnityEngine;

namespace FX.BoundsAttackVisualEffect;

public abstract class BoundsAttackVisualEffect : VisualEffect
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<BoundsAttackVisualEffect>
	{
		public void Spawn(Character owner, Bounds bounds, in Damage damage, ITarget target)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Spawn(owner, bounds, in damage, target);
			}
		}
	}

	public static readonly Type[] types = new Type[1] { typeof(RandomWithinIntersect) };

	public abstract void Spawn(Character owner, Bounds bounds, in Damage damage, ITarget target);
}
