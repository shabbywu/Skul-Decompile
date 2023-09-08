using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public abstract class Movement : MonoBehaviour
{
	public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(allowCustom: true, Movement.types)
		{
		}
	}

	public static readonly Type[] types = new Type[9]
	{
		typeof(Simple),
		typeof(Ease2),
		typeof(Trajectory),
		typeof(TrajectoryToPoint),
		typeof(Homing),
		typeof(Missile),
		typeof(Ground),
		typeof(Spiral),
		typeof(ReadyAndFire)
	};

	public IProjectile projectile { get; private set; }

	public float direction { get; set; }

	public Vector2 directionVector { get; set; }

	public virtual void Initialize(IProjectile projectile, float direction)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		this.projectile = projectile;
		this.direction = direction;
		float num = direction * ((float)Math.PI / 180f);
		directionVector = new Vector2(Mathf.Cos(num), Mathf.Sin(num));
	}

	public abstract (Vector2 direction, float speed) GetSpeed(float time, float deltaTime);
}
