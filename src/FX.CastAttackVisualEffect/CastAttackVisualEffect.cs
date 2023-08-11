using System;
using Characters;
using UnityEditor;
using UnityEngine;

namespace FX.CastAttackVisualEffect;

public abstract class CastAttackVisualEffect : VisualEffect
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<CastAttackVisualEffect>
	{
		public void Spawn(Vector3 position)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Spawn(position);
			}
		}

		public void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Spawn(owner, collider, origin, direction, distance, raycastHit);
			}
		}

		public void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Spawn(owner, collider, origin, direction, distance, raycastHit, damage, target);
			}
		}
	}

	public static readonly Type[] types = new Type[1] { typeof(SpawnOnHitPoint) };

	public abstract void Spawn(Vector3 position);

	public abstract void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

	public abstract void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target);
}
