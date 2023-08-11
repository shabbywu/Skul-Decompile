using System;
using Characters;
using Characters.Projectiles;
using UnityEditor;
using UnityEngine;

namespace FX.ProjectileAttackVisualEffect;

public abstract class ProjectileAttackVisualEffect : VisualEffect
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<ProjectileAttackVisualEffect>
	{
		public void SpawnDespawn(IProjectile projectile)
		{
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].SpawnDespawn(projectile);
			}
		}

		public void SpawnExpire(IProjectile projectile)
		{
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].SpawnExpire(projectile);
			}
		}

		public void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Spawn(projectile, origin, direction, distance, raycastHit);
			}
		}

		public void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Spawn(projectile, origin, direction, distance, raycastHit, damage, target);
			}
		}
	}

	public static readonly Type[] types = new Type[1] { typeof(SpawnOnHitPoint) };

	public abstract void SpawnDespawn(IProjectile projectile);

	public abstract void SpawnExpire(IProjectile projectile);

	public abstract void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

	public abstract void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target);
}
