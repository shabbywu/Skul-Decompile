using UnityEngine;

namespace Characters.Projectiles.Operations;

public class KnockbackTo : CharacterHitOperation
{
	[Header("Destination")]
	[SerializeField]
	private Collider2D _targetPlace;

	[SerializeField]
	private Transform _targetPoint;

	[Header("Force")]
	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private bool _ignoreOtherForce = true;

	[SerializeField]
	private bool _expireOnGround;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = ((!((Object)(object)_targetPlace != (Object)null)) ? Vector2.op_Implicit(_targetPoint.position) : MMMaths.RandomPointWithinBounds(_targetPlace.bounds));
		Vector2 force = val - Vector2.op_Implicit(((Component)target).transform.position);
		target.movement.push.ApplyKnockback(projectile.owner, force, _curve, _ignoreOtherForce, _expireOnGround);
	}
}
