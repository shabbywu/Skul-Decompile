using UnityEngine;

namespace Characters.Projectiles.Operations;

public class ActivateObject : HitOperation
{
	[SerializeField]
	private GameObject _target;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Vector2 point = ((RaycastHit2D)(ref raycastHit)).point;
		Object.Destroy((Object)(object)Object.Instantiate<GameObject>(_target, Vector2.op_Implicit(point), Quaternion.identity), 10f);
	}
}
