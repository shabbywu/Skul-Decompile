using Characters;
using Characters.Movements;
using UnityEngine;

namespace FX.SmashAttackVisualEffect;

public class SpawnOnHitPoint : SmashAttackVisualEffect
{
	[SerializeField]
	private bool _referSmashDirection;

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

	public override void Spawn(Character owner, Push push, RaycastHit2D raycastHit, Movement.CollisionDirection direction, Damage damage, ITarget target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		Bounds bounds = ((Collider2D)owner.collider).bounds;
		Vector3 min = ((Bounds)(ref bounds)).min;
		bounds = ((Collider2D)owner.collider).bounds;
		Vector3 max = ((Bounds)(ref bounds)).max;
		switch (direction)
		{
		case Movement.CollisionDirection.Above:
			zero.x = Random.Range(min.x, max.x);
			zero.y = max.y;
			break;
		case Movement.CollisionDirection.Below:
			zero.x = Random.Range(min.x, max.x);
			zero.y = min.y;
			break;
		case Movement.CollisionDirection.Left:
			zero.x = min.x;
			zero.y = Random.Range(min.y, max.y);
			break;
		case Movement.CollisionDirection.Right:
			zero.x = max.x;
			zero.y = Random.Range(min.y, max.y);
			break;
		}
		EffectInfo obj = (damage.critical ? _critical : _normal);
		float extraAngle = (_referSmashDirection ? (Mathf.Atan2(push.direction.y, push.direction.x) * 57.29578f) : 0f);
		Vector3 val = (Vector3)((owner.lookingDirection == Character.LookingDirection.Right) ? Vector3.one : new Vector3(-1f, 1f, 1f));
		Vector3 localScale = ((Component)obj.Spawn(zero, extraAngle)).transform.localScale;
		((Vector3)(ref localScale)).Scale(val);
	}
}
