using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class LeapToPlayer : Movement
{
	[SerializeField]
	private float _duration = 1f;

	private Vector2 _directionVector;

	private float _distance;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(((Component)player).transform.position.x, projectile.transform.position.y);
		Vector2 val2 = Vector2.op_Implicit(projectile.transform.position);
		_directionVector = val - val2;
		_distance = Mathf.Abs(val2.x - val.x);
		base.Initialize(projectile, direction);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		float item = ((time > _duration) ? 0f : _distance);
		return (((Vector2)(ref _directionVector)).normalized, item);
	}
}
