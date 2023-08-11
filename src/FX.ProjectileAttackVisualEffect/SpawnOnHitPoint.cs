using Characters;
using Characters.Projectiles;
using UnityEngine;

namespace FX.ProjectileAttackVisualEffect;

public class SpawnOnHitPoint : ProjectileAttackVisualEffect
{
	[SerializeField]
	private Transform _spawnPosition;

	[Header("Spawn map")]
	[SerializeField]
	private bool _spawnOnDespawn;

	[SerializeField]
	private bool _spawnOnExpire = true;

	[SerializeField]
	private bool _spawnOnTerrainHit = true;

	[SerializeField]
	private bool _spawnOnCharacterHit = true;

	[SerializeField]
	private bool _spawnOnDamageableHit = true;

	[Header("Effects")]
	[SerializeField]
	private EffectInfo _normal;

	[SerializeField]
	private EffectInfo _critical;

	private void Awake()
	{
		if ((Object)(object)_critical.effect == (Object)null)
		{
			_critical = _normal;
		}
	}

	public override void SpawnDespawn(IProjectile projectile)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (_spawnOnDespawn)
		{
			Vector3 position = (((Object)(object)_spawnPosition == (Object)null) ? projectile.transform.position : _spawnPosition.position);
			_normal.Spawn(position);
		}
	}

	public override void SpawnExpire(IProjectile projectile)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (_spawnOnExpire)
		{
			Vector3 position = (((Object)(object)_spawnPosition == (Object)null) ? projectile.transform.position : _spawnPosition.position);
			_normal.Spawn(position);
		}
	}

	public override void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (_spawnOnTerrainHit)
		{
			Vector3 localScale = ((Component)_normal.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), projectile.owner)).transform.localScale;
			((Vector3)(ref localScale)).Scale(projectile.transform.localScale);
		}
	}

	public override void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if ((_spawnOnCharacterHit && (Object)(object)target.character != (Object)null) || (_spawnOnDamageableHit && (Object)(object)target.damageable != (Object)null))
		{
			EffectPoolInstance effectPoolInstance = (damage.critical ? _critical : _normal).Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), projectile.owner);
			if ((Object)(object)effectPoolInstance != (Object)null)
			{
				Vector3 localScale = ((Component)effectPoolInstance).transform.localScale;
				((Vector3)(ref localScale)).Scale(projectile.transform.localScale);
			}
		}
	}
}
