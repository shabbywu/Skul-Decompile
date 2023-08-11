using PhysicsUtils;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Ground : Movement
{
	private class TerrainCheckCaster
	{
		private RayCaster _groundLeft;

		private RayCaster _groundRight;

		private RayCaster _wallLeft;

		private RayCaster _wallRight;

		internal TerrainCheckCaster()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			_groundLeft = new RayCaster
			{
				direction = Vector2.down
			};
			_groundRight = new RayCaster
			{
				direction = Vector2.down
			};
			_wallLeft = new RayCaster
			{
				direction = Vector2.left
			};
			_wallRight = new RayCaster
			{
				direction = Vector2.right
			};
		}

		internal (RaycastHit2D groundLeft, RaycastHit2D groundRight, RaycastHit2D wallLeft, RaycastHit2D wallRight) Cast(Bounds bounds, float groundDistance, float wallDistance, LayerMask layerMask)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			((ContactFilter2D)(ref ((Caster)_groundLeft).contactFilter)).SetLayerMask(Layers.footholdMask);
			((ContactFilter2D)(ref ((Caster)_groundRight).contactFilter)).SetLayerMask(Layers.footholdMask);
			Vector2 origin = default(Vector2);
			((Vector2)(ref origin))._002Ector(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).min.y);
			Vector2 origin2 = default(Vector2);
			((Vector2)(ref origin2))._002Ector(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).min.y);
			((Caster)_groundLeft).origin = origin;
			((Caster)_groundLeft).distance = groundDistance;
			((Caster)_groundRight).origin = origin2;
			((Caster)_groundRight).distance = groundDistance;
			((ContactFilter2D)(ref ((Caster)_wallLeft).contactFilter)).SetLayerMask(layerMask);
			((ContactFilter2D)(ref ((Caster)_wallRight).contactFilter)).SetLayerMask(layerMask);
			((Caster)_wallLeft).origin = origin;
			((Caster)_wallLeft).distance = wallDistance;
			((Caster)_wallRight).origin = origin2;
			((Caster)_wallRight).distance = wallDistance;
			return (((Caster)_groundLeft).SingleCast(), ((Caster)_groundRight).SingleCast(), ((Caster)_wallLeft).SingleCast(), ((Caster)_wallRight).SingleCast());
		}
	}

	public enum Action
	{
		Hold,
		Despawn,
		Return,
		Cotinue
	}

	private const float offset = 1f / 32f;

	private TerrainCheckCaster _caster;

	[SerializeField]
	private Action _onFaceClif;

	[SerializeField]
	private Action _onFaceWall = Action.Return;

	private Action _state;

	[SerializeField]
	private float _startSpeed;

	[SerializeField]
	private float _targetSpeed;

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private float _easingTime;

	[SerializeField]
	private float _gravity;

	[SerializeField]
	private bool _flipXByFacingDirection = true;

	[SerializeField]
	private bool _followOwner;

	[SerializeField]
	private LayerMask _terrainLayer = Layers.terrainMask;

	private float _ySpeed;

	private bool _grounded;

	private Character _owner;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize(projectile, direction);
		_ySpeed = 0f;
		_state = Action.Cotinue;
		_grounded = false;
		_caster = new TerrainCheckCaster();
		_owner = projectile.owner;
		if (_followOwner)
		{
			base.direction = ((!(((Component)_owner).transform.position.x > projectile.transform.position.x)) ? 180 : 0);
			base.directionVector = ((((Component)_owner).transform.position.x > projectile.transform.position.x) ? Vector2.right : Vector2.left);
		}
		SetScaleByFacingDirection();
	}

	private void SetScaleByFacingDirection()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (_flipXByFacingDirection)
		{
			Vector3 localScale = base.projectile.transform.localScale;
			localScale.x = Mathf.Abs(localScale.x) * (float)((base.directionVector.x > 0f) ? 1 : (-1));
			base.projectile.transform.localScale = localScale;
		}
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = (_startSpeed + (_targetSpeed - _startSpeed) * _curve.Evaluate(time / _easingTime)) * base.directionVector * base.projectile.speedMultiplier;
		_ySpeed -= _gravity * deltaTime;
		val.y += _ySpeed;
		((Behaviour)base.projectile.collider).enabled = true;
		Bounds bounds = base.projectile.collider.bounds;
		((Behaviour)base.projectile.collider).enabled = false;
		Vector3 val2 = Vector2.op_Implicit(val * deltaTime);
		((Bounds)(ref bounds)).center = ((Bounds)(ref bounds)).center + val2;
		(RaycastHit2D, RaycastHit2D, RaycastHit2D, RaycastHit2D) tuple = _caster.Cast(bounds, Mathf.Abs(val2.y) + 0.0625f, Mathf.Abs(val2.x) + 0.0625f, _terrainLayer);
		if (RaycastHit2D.op_Implicit(tuple.Item1) && RaycastHit2D.op_Implicit(tuple.Item2))
		{
			float num = Mathf.Min(((RaycastHit2D)(ref tuple.Item1)).distance, ((RaycastHit2D)(ref tuple.Item2)).distance);
			val.y = 0f - Mathf.Max(-1f / 32f, num - 1f / 32f);
			_ySpeed = 0f;
			_grounded = true;
		}
		else if (RaycastHit2D.op_Implicit(tuple.Item1))
		{
			val.y = 0f - Mathf.Max(-1f / 32f, ((RaycastHit2D)(ref tuple.Item1)).distance - 1f / 32f);
			_ySpeed = 0f;
			_grounded = true;
		}
		else if (RaycastHit2D.op_Implicit(tuple.Item2))
		{
			val.y = 0f - Mathf.Max(-1f / 32f, ((RaycastHit2D)(ref tuple.Item2)).distance - 1f / 32f);
			_ySpeed = 0f;
			_grounded = true;
		}
		if (RaycastHit2D.op_Implicit(tuple.Item3) || RaycastHit2D.op_Implicit(tuple.Item4))
		{
			switch (_onFaceWall)
			{
			case Action.Hold:
				_state = Action.Hold;
				break;
			case Action.Return:
				base.direction -= 180f;
				base.directionVector = new Vector2(0f - base.directionVector.x, base.directionVector.y);
				val.x *= -1f;
				SetScaleByFacingDirection();
				break;
			case Action.Despawn:
				base.projectile.Despawn();
				break;
			}
		}
		else if (_grounded && (!RaycastHit2D.op_Implicit(tuple.Item1) || !RaycastHit2D.op_Implicit(tuple.Item2)))
		{
			switch (_onFaceClif)
			{
			case Action.Hold:
				_state = Action.Hold;
				break;
			case Action.Return:
				base.direction -= 180f;
				base.directionVector = new Vector2(0f - base.directionVector.x, base.directionVector.y);
				val.x *= -1f;
				SetScaleByFacingDirection();
				break;
			case Action.Despawn:
				base.projectile.Despawn();
				break;
			}
		}
		else if (_followOwner)
		{
			base.direction = ((!(((Component)_owner).transform.position.x > base.projectile.transform.position.x)) ? 180 : 0);
			base.directionVector = ((((Component)_owner).transform.position.x > base.projectile.transform.position.x) ? Vector2.right : Vector2.left);
			if (Mathf.Abs(((Component)_owner).transform.position.x - base.projectile.transform.position.x) < 0.5f)
			{
				_state = Action.Hold;
			}
			else
			{
				_state = Action.Cotinue;
			}
			SetScaleByFacingDirection();
		}
		_grounded = false;
		if (_state == Action.Hold)
		{
			return (base.directionVector, 0f);
		}
		return (((Vector2)(ref val)).normalized, ((Vector2)(ref val)).magnitude);
	}
}
