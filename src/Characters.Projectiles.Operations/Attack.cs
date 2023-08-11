using Characters.Operations;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class Attack : CharacterHitOperation
{
	[SerializeField]
	protected HitInfo _hitInfo = new HitInfo(Damage.AttackType.Ranged);

	[SerializeField]
	protected ChronoInfo _chrono;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Damage damage = projectile.owner.stat.GetDamage(projectile.baseDamage, ((RaycastHit2D)(ref raycastHit)).point, _hitInfo);
		projectile.owner.Attack(character, ref damage);
		_chrono.ApplyTo(character);
	}
}
