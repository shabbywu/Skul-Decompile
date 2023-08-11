using Characters;
using UnityEngine;

namespace FX.CastAttackVisualEffect;

public class SpawnOnHitPoint : CastAttackVisualEffect
{
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

	public override void Spawn(Vector3 position)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_normal.Spawn(position);
	}

	public override void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		((Component)_normal.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), owner)).transform.localScale = (Vector3)((owner.lookingDirection == Character.LookingDirection.Right) ? Vector3.one : new Vector3(-1f, 1f, 1f));
	}

	public override void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		(damage.critical ? _critical : _normal).Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), owner);
	}
}
