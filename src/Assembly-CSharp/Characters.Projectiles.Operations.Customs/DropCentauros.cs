using Level;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs;

public class DropCentauros : HitOperation
{
	[SerializeField]
	private DroppedCentauros _droppedCentauros;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		PoolObject val = _droppedCentauros.Spawn(((RaycastHit2D)(ref raycastHit)).point);
		if (((RaycastHit2D)(ref raycastHit)).normal.x >= 0f)
		{
			((Component)val).transform.localScale = Vector2.op_Implicit(new Vector2(-1f, 1f));
		}
		else
		{
			((Component)val).transform.localScale = Vector2.op_Implicit(new Vector2(1f, 1f));
		}
	}
}
