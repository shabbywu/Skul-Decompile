using System;
using Characters;
using Characters.Movements;
using UnityEditor;
using UnityEngine;

namespace FX.SmashAttackVisualEffect;

public abstract class SmashAttackVisualEffect : VisualEffect
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<SmashAttackVisualEffect>
	{
		public void Spawn(Character owner, Push push, RaycastHit2D raycastHit, Movement.CollisionDirection direction, Damage damage, ITarget target)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Spawn(owner, push, raycastHit, direction, damage, target);
			}
		}
	}

	public static readonly Type[] types = new Type[1] { typeof(SpawnOnHitPoint) };

	public abstract void Spawn(Character owner, Push push, RaycastHit2D raycastHit, Movement.CollisionDirection direction, Damage damage, ITarget target);
}
