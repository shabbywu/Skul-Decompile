using Characters;
using Characters.Projectiles;
using UnityEngine;

namespace Level.Traps;

public class Apple : DestructibleObject
{
	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private Collider2D _collider;

	public override Collider2D collider => _collider;

	public override void Hit(Character from, ref Damage damage, Vector2 force)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (damage.amount != 0.0)
		{
			float direction = Mathf.Atan2(force.y, force.x) * 57.29578f + Random.Range(-10f, 10f);
			float num = Mathf.Clamp(((Vector2)(ref force)).magnitude, 2f, 6f);
			((Component)_projectile.reusable.Spawn(((Component)this).transform.position, true)).GetComponent<Projectile>().Fire(from, num, direction, flipX: false, flipY: false, num);
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}
}
