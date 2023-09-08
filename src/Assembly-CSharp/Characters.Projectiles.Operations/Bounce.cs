using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class Bounce : HitOperation
{
	[SerializeField]
	private LayerMask _terrainLayer = Layers.terrainMaskForProjectile;

	[SerializeField]
	private EffectInfo _terrainLeftHitEffect;

	[SerializeField]
	private EffectInfo _terrainRightHitEffect;

	[SerializeField]
	private EffectInfo _terrainTopHitEffect;

	[SerializeField]
	private EffectInfo _terrainBottomHitEffect;

	public Collider2D lastCollision { get; set; }

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)((RaycastHit2D)(ref raycastHit)).collider == (Object)(object)lastCollision)
		{
			return;
		}
		lastCollision = ((RaycastHit2D)(ref raycastHit)).collider;
		Vector2 point = ((RaycastHit2D)(ref raycastHit)).point;
		float speed = projectile.speed;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(0f - projectile.direction.x, projectile.direction.y);
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(projectile.direction.x, 0f - projectile.direction.y);
		if (RaycastHit2D.op_Implicit(Physics2D.Raycast(point, val, speed, LayerMask.op_Implicit(_terrainLayer))))
		{
			if (val2.y > 0f)
			{
				_terrainBottomHitEffect.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), projectile.owner);
			}
			else
			{
				_terrainTopHitEffect.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), projectile.owner);
			}
			projectile.direction = val2;
		}
		else if (RaycastHit2D.op_Implicit(Physics2D.Raycast(point, val2, speed, LayerMask.op_Implicit(_terrainLayer))))
		{
			projectile.direction = val;
			if (val2.x > 0f)
			{
				_terrainRightHitEffect.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), projectile.owner);
			}
			else
			{
				_terrainLeftHitEffect.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), projectile.owner);
			}
		}
		else
		{
			projectile.direction = new Vector2(0f - projectile.direction.x, 0f - projectile.direction.y);
		}
	}
}
