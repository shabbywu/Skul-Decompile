using System;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Movements;

public class Movement : MonoBehaviour
{
	[Serializable]
	public class Config
	{
		public enum Type
		{
			Static,
			Walking,
			Flying,
			AcceleratingFlying,
			AcceleratingWalking,
			Bidimensional
		}

		public static readonly Config @static = new Config(Type.Static);

		[SerializeField]
		internal Type type = Type.Walking;

		[SerializeField]
		internal bool keepMove;

		[SerializeField]
		internal bool snapToGround;

		[SerializeField]
		internal bool lockLookingDirection;

		[SerializeField]
		internal float gravity = -40f;

		[SerializeField]
		internal float maxFallSpeed = 25f;

		[SerializeField]
		internal float acceleration = 2f;

		[SerializeField]
		internal float friction = 0.95f;

		[SerializeField]
		internal bool ignoreGravity;

		[SerializeField]
		internal bool ignorePush;

		public Config()
		{
		}

		public Config(Type type)
		{
			this.type = type;
		}
	}

	public enum CollisionDirection
	{
		None,
		Above,
		Below,
		Left,
		Right
	}

	public enum JumpType
	{
		GroundJump,
		AirJump,
		DownJump
	}

	public delegate void onJumpDelegate(JumpType jumpType, float jumpHeight);

	public readonly SumInt airJumpCount = new SumInt(1);

	public readonly TrueOnlyLogicalSumList ignoreGravity = new TrueOnlyLogicalSumList();

	[NonSerialized]
	public int currentAirJumpCount;

	[NonSerialized]
	public bool binded;

	[NonSerialized]
	public TrueOnlyLogicalSumList blocked = new TrueOnlyLogicalSumList();

	[NonSerialized]
	public Vector2 move;

	[NonSerialized]
	public Vector2 force;

	[NonSerialized]
	public bool moveBackward;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private Config _config;

	[SerializeField]
	[GetComponent]
	private CharacterController2D _controller;

	private Vector2 _moved;

	private Vector2 _velocity;

	private static readonly NonAllocCaster _belowCaster;

	public readonly PriorityList<Config> configs = new PriorityList<Config>();

	public Push push;

	private float speed => _character.stat.GetInterpolatedMovementSpeed();

	private float knockbackMultiplier => (float)_character.stat.GetFinal(Stat.Kind.KnockbackResistance);

	public Config baseConfig => _config;

	public Config config => configs[0];

	public Vector2 lastDirection { get; private set; }

	public CharacterController2D controller => _controller;

	public Character owner => _character;

	public Vector2 moved => _moved;

	public Vector2 velocity => _velocity;

	public float verticalVelocity
	{
		get
		{
			return _velocity.y;
		}
		set
		{
			_velocity.y = value;
		}
	}

	public bool isGrounded { get; private set; }

	public event Action onGrounded;

	public event Action onFall;

	public event onJumpDelegate onJump;

	public event Action<Vector2> onMoved;

	static Movement()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_belowCaster = new NonAllocCaster(15);
	}

	protected virtual void Awake()
	{
		push = new Push(_character);
		configs.Add(int.MinValue, _config);
		_controller = ((Component)this).GetComponent<CharacterController2D>();
		_controller.collisionState.aboveCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			OnControllerCollide(hit, CollisionDirection.Above);
		};
		_controller.collisionState.belowCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			OnControllerCollide(hit, CollisionDirection.Below);
		};
		_controller.collisionState.leftCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			OnControllerCollide(hit, CollisionDirection.Left);
		};
		_controller.collisionState.rightCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			OnControllerCollide(hit, CollisionDirection.Right);
		};
		Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += FindClosestBelowGround;
		currentAirJumpCount = 0;
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= FindClosestBelowGround;
		}
	}

	private void FindClosestBelowGround()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.down, float.PositiveInfinity, LayerMask.op_Implicit(Layers.groundMask));
		if (RaycastHit2D.op_Implicit(val))
		{
			controller.collisionState.lastStandingCollider = ((RaycastHit2D)(ref val)).collider;
		}
	}

	private void Start()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (config.snapToGround)
		{
			_controller.Move(new Vector2(0f, -50f));
		}
	}

	private void OnControllerCollide(RaycastHit2D raycastHit, CollisionDirection direction)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (push.smash && !push.expired)
		{
			push.CollideWith(raycastHit, direction);
		}
	}

	private bool HandlePush(float deltaTime)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		if (_config.ignorePush)
		{
			return false;
		}
		if (!push.expired)
		{
			push.Update(out var vector, deltaTime);
			_controller.ignoreAbovePlatform = !push.smash;
			vector *= knockbackMultiplier;
			if (push.ignoreOtherForce)
			{
				_moved = _controller.Move(vector);
				_velocity = Vector2.zero;
				return true;
			}
			force += vector;
			return false;
		}
		_controller.ignoreAbovePlatform = true;
		return false;
	}

	private Vector2 HandleMove(float deltaTime)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.zero;
		if (HandlePush(deltaTime))
		{
			return zero;
		}
		float num = (blocked.value ? 0f : speed);
		zero = move * num;
		_character.animationController.parameter.walk = zero.x != 0f;
		_character.animationController.parameter.movementSpeed = (moveBackward ? (0f - num) : num) * 0.25f;
		if (!config.lockLookingDirection)
		{
			if (moveBackward)
			{
				if (move.x > 0f)
				{
					_character.lookingDirection = Character.LookingDirection.Left;
				}
				else if (move.x < 0f)
				{
					_character.lookingDirection = Character.LookingDirection.Right;
				}
			}
			else if (move.x > 0f)
			{
				_character.lookingDirection = Character.LookingDirection.Right;
			}
			else if (move.x < 0f)
			{
				_character.lookingDirection = Character.LookingDirection.Left;
			}
		}
		if (_controller.isGrounded && _velocity.y < 0f)
		{
			_velocity.y = 0f;
		}
		switch (config.type)
		{
		case Config.Type.Walking:
			_velocity.x = zero.x;
			AddGravity(deltaTime);
			break;
		case Config.Type.Flying:
			_velocity = zero;
			break;
		case Config.Type.AcceleratingFlying:
			_velocity *= 1f - config.friction * deltaTime;
			_velocity += zero * config.acceleration * deltaTime;
			AddGravity(deltaTime);
			break;
		case Config.Type.AcceleratingWalking:
			_velocity.x *= 1f - config.friction * deltaTime;
			_velocity.x += zero.x * config.acceleration * deltaTime;
			if (Mathf.Abs(_velocity.x) > num)
			{
				_velocity.x = num * Mathf.Sign(_velocity.x);
			}
			AddGravity(deltaTime);
			break;
		case Config.Type.Bidimensional:
			_velocity *= 1f - config.friction * deltaTime;
			_velocity += zero * config.acceleration * deltaTime;
			AddGravity(deltaTime);
			break;
		}
		zero = _velocity * deltaTime + force;
		_moved = _controller.Move(zero);
		_velocity = _moved - force;
		if (zero.x > 0f != _velocity.x > 0f)
		{
			_velocity.x = 0f;
		}
		if (zero.y > 0f != _velocity.y > 0f)
		{
			_velocity.y = 0f;
		}
		_character.animationController.parameter.ySpeed = _velocity.y;
		if (deltaTime > 0f)
		{
			_velocity /= deltaTime;
			if ((config.type == Config.Type.AcceleratingFlying || config.type == Config.Type.Bidimensional) && ((Vector2)(ref _velocity)).sqrMagnitude > num * num)
			{
				_velocity = ((Vector2)(ref _velocity)).normalized * num;
			}
			_controller.velocity = Vector2.op_Implicit(_velocity);
		}
		this.onMoved?.Invoke(_moved);
		return zero;
	}

	private void AddGravity(float deltaTime)
	{
		if (!ignoreGravity.value && !config.ignoreGravity)
		{
			_velocity.y += config.gravity * deltaTime;
		}
		if (_velocity.y < 0f - config.maxFallSpeed)
		{
			_velocity.y = 0f - config.maxFallSpeed;
		}
	}

	protected virtual void LateUpdate()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		Config config = this.config;
		if (config.type == Config.Type.Static)
		{
			_character.animationController.parameter.grounded = true;
			_character.animationController.parameter.walk = false;
			_character.animationController.parameter.ySpeed = 0f;
			return;
		}
		float num = _character.chronometer.animation.DeltaTime();
		if (num == 0f)
		{
			return;
		}
		if (num > 0.1f)
		{
			num = 0.1f;
		}
		_controller.UpdateBounds();
		bool flag = isGrounded;
		_moved = Vector2.zero;
		Vector2 val = HandleMove(num);
		if (config.type == Config.Type.Flying || config.type == Config.Type.AcceleratingFlying || config.type == Config.Type.Bidimensional)
		{
			_character.animationController.parameter.grounded = true;
		}
		else
		{
			_character.animationController.parameter.grounded = _controller.isGrounded;
		}
		force = Vector2.zero;
		if (!config.keepMove)
		{
			move = Vector2.zero;
		}
		if (val.y <= 0f && _controller.collisionState.below)
		{
			isGrounded = true;
			if (!flag)
			{
				this.onGrounded?.Invoke();
				currentAirJumpCount = 0;
				if (_velocity.y <= 0f && !push.expired && push.expireOnGround)
				{
					push.Expire();
				}
			}
		}
		else
		{
			if (isGrounded && this.onFall != null)
			{
				this.onFall();
			}
			isGrounded = false;
		}
	}

	public void MoveHorizontal(Vector2 normalizedDirection)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (config.keepMove)
		{
			if (normalizedDirection == Vector2.zero)
			{
				return;
			}
			if (normalizedDirection.x > 0f)
			{
				normalizedDirection.x = 1f;
			}
			if (normalizedDirection.x < 0f)
			{
				normalizedDirection.x = -1f;
			}
		}
		move = normalizedDirection;
		lastDirection = move;
	}

	public void MoveVertical(Vector2 normalizedDirection)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (config.type != Config.Type.Bidimensional)
		{
			return;
		}
		if (config.keepMove)
		{
			if (normalizedDirection == Vector2.zero)
			{
				return;
			}
			if (normalizedDirection.y > 0f)
			{
				normalizedDirection.y = 1f;
			}
			if (normalizedDirection.y < 0f)
			{
				normalizedDirection.y = -1f;
			}
		}
		move.y = normalizedDirection.y;
		lastDirection = move;
	}

	public void Move(float angle)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		move.x = Mathf.Cos(angle);
		move.y = Mathf.Sin(angle);
		lastDirection = move;
	}

	public void Move(Vector2 direction)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		move = direction;
		lastDirection = direction;
	}

	public void MoveTo(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2(position.x - ((Component)this).transform.position.x, position.y - ((Component)this).transform.position.y);
		MoveHorizontal(((Vector2)(ref val)).normalized);
	}

	public void Jump(float jumpHeight)
	{
		if (jumpHeight > float.Epsilon)
		{
			_velocity.y = Mathf.Sqrt(2f * jumpHeight * (0f - config.gravity));
		}
		this.onJump?.Invoke((!isGrounded) ? JumpType.AirJump : JumpType.GroundJump, jumpHeight);
	}

	public void JumpDown()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		bool ignorePlatform = _controller.ignorePlatform;
		_controller.ignorePlatform = true;
		_controller.Move(Vector2.op_Implicit(new Vector3(0f, -0.1f, 0f)));
		_controller.ignorePlatform = ignorePlatform;
		this.onJump?.Invoke(JumpType.DownJump, 0f);
	}

	public bool TryBelowRayCast(LayerMask mask, out RaycastHit2D point, float distance)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _belowCaster.contactFilter)).SetLayerMask(mask);
		_belowCaster.RayCast(Vector2.op_Implicit(((Component)owner).transform.position), Vector2.down, distance);
		ReadonlyBoundedList<RaycastHit2D> results = _belowCaster.results;
		point = default(RaycastHit2D);
		if (results.Count < 0)
		{
			return false;
		}
		int index = 0;
		RaycastHit2D val = results[0];
		float num = ((RaycastHit2D)(ref val)).distance;
		for (int i = 1; i < results.Count; i++)
		{
			val = results[i];
			float distance2 = ((RaycastHit2D)(ref val)).distance;
			if (distance2 < num)
			{
				num = distance2;
				index = i;
			}
		}
		point = results[index];
		return true;
	}

	public bool TryGetClosestBelowCollider(out Collider2D collider, LayerMask layerMask, float distance = 100f)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _belowCaster.contactFilter)).SetLayerMask(layerMask);
		NonAllocCaster belowCaster = _belowCaster;
		Vector2 val = Vector2.op_Implicit(((Component)owner).transform.position);
		Bounds bounds = ((Collider2D)owner.collider).bounds;
		ReadonlyBoundedList<RaycastHit2D> results = belowCaster.BoxCast(val, Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f, Vector2.down, distance).results;
		if (results.Count <= 0)
		{
			collider = null;
			return false;
		}
		int index = 0;
		RaycastHit2D val2 = results[0];
		float num = ((RaycastHit2D)(ref val2)).distance;
		for (int i = 1; i < results.Count; i++)
		{
			val2 = results[i];
			float distance2 = ((RaycastHit2D)(ref val2)).distance;
			if (distance2 < num)
			{
				num = distance2;
				index = i;
			}
		}
		val2 = results[index];
		collider = ((RaycastHit2D)(ref val2)).collider;
		return true;
	}

	public void TurnOnEdge(ref Vector2 direction)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return;
		}
		float num = velocity.x * _character.chronometer.master.deltaTime;
		Bounds bounds = ((Collider2D)_character.collider).bounds;
		float num2 = ((Bounds)(ref bounds)).max.x + num + 0.5f;
		bounds = lastStandingCollider.bounds;
		if (num2 >= ((Bounds)(ref bounds)).max.x)
		{
			direction = Vector2.left;
			return;
		}
		bounds = ((Collider2D)_character.collider).bounds;
		float num3 = ((Bounds)(ref bounds)).min.x + num - 0.5f;
		bounds = lastStandingCollider.bounds;
		if (num3 <= ((Bounds)(ref bounds)).min.x)
		{
			direction = Vector2.right;
		}
	}
}
