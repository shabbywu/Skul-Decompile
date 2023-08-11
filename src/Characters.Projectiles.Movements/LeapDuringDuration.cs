using Characters.Projectiles.Movements.SubMovements;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class LeapDuringDuration : Movement
{
	[SerializeField]
	private TargetFinder _finder;

	[SerializeField]
	private SubMovement _subMovement;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float _duration = 1f;

	private Vector2 _directionVector;

	private float _distance;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_finder.range != (Object)null)
		{
			_finder.Initialize(projectile);
			Target target = _finder.Find();
			if ((Object)(object)target != (Object)null && (Object)(object)target.character != (Object)null)
			{
				Vector2 val = Vector2.op_Implicit(projectile.transform.position);
				RaycastHit2D val2 = Physics2D.Raycast(Vector2.op_Implicit(((Component)target.character).transform.position), Vector2.down, float.PositiveInfinity, LayerMask.op_Implicit(Layers.groundMask));
				Vector2 val3 = default(Vector2);
				if (RaycastHit2D.op_Implicit(val2))
				{
					val3 = ((RaycastHit2D)(ref val2)).point;
				}
				else if ((Object)(object)target.character.movement.controller.collisionState.lastStandingCollider != (Object)null)
				{
					Bounds bounds = target.character.movement.controller.collisionState.lastStandingCollider.bounds;
					((Vector2)(ref val3))._002Ector(((Component)target).transform.position.x, ((Bounds)(ref bounds)).center.y);
				}
				else
				{
					val3 = Vector2.op_Implicit(((Component)target).transform.position);
				}
				_directionVector = val3 - val;
				direction = Mathf.Atan2(_directionVector.y, _directionVector.x) * 57.29578f;
				_distance = Vector2.Distance(val, val3);
			}
		}
		base.Initialize(projectile, direction);
		_subMovement.Move(projectile);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		float item = ((time > _duration) ? 0f : (_distance / _duration));
		return (((Vector2)(ref _directionVector)).normalized, item);
	}
}
