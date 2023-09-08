using System;
using Characters;
using Characters.Movements;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Movements;

public class DecorationCharacterMovement : MonoBehaviour
{
	public readonly PriorityList<Movement.Config> configs = new PriorityList<Movement.Config>();

	[NonSerialized]
	public Vector2 move;

	[NonSerialized]
	public bool moveBackward;

	[NonSerialized]
	public Vector2 force;

	[GetComponent]
	[SerializeField]
	private DecorationCharacter _decorationCharacter;

	[GetComponent]
	[SerializeField]
	private CharacterController2D _controller;

	[SerializeField]
	private Movement.Config _config;

	private Vector2 _moved;

	private Vector2 _velocity;

	public CharacterController2D controller => _controller;

	public Movement.Config config => configs[0];

	public bool isGrounded { get; private set; }

	public Vector2 lastDirection { get; private set; }

	private void Awake()
	{
		_controller = ((Component)this).GetComponent<CharacterController2D>();
		_decorationCharacter = ((Component)this).GetComponent<DecorationCharacter>();
		configs.Add(int.MinValue, _config);
		if (config.type == Movement.Config.Type.Static)
		{
			_decorationCharacter.animationController.parameter.grounded = true;
			_decorationCharacter.animationController.parameter.walk = false;
			_decorationCharacter.animationController.parameter.ySpeed = 0f;
		}
	}

	private Vector2 HandleMove(float deltaTime)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.zero;
		float speed = _decorationCharacter.speed;
		zero = move * speed;
		_decorationCharacter.animationController.parameter.walk = zero.x != 0f;
		_decorationCharacter.animationController.parameter.movementSpeed = (moveBackward ? (0f - speed) : speed) * 0.25f;
		if (!config.lockLookingDirection)
		{
			if (moveBackward)
			{
				if (move.x > 0f)
				{
					_decorationCharacter.lookingDirection = Character.LookingDirection.Left;
				}
				else if (move.x < 0f)
				{
					_decorationCharacter.lookingDirection = Character.LookingDirection.Right;
				}
			}
			else if (move.x > 0f)
			{
				_decorationCharacter.lookingDirection = Character.LookingDirection.Right;
			}
			else if (move.x < 0f)
			{
				_decorationCharacter.lookingDirection = Character.LookingDirection.Left;
			}
		}
		if (_controller.isGrounded && _velocity.y < 0f)
		{
			_velocity.y = 0f;
		}
		switch (config.type)
		{
		case Movement.Config.Type.Walking:
			_velocity.x = zero.x;
			AddGravity(deltaTime);
			break;
		case Movement.Config.Type.Flying:
			_velocity = zero;
			break;
		case Movement.Config.Type.AcceleratingFlying:
			_velocity *= 1f - config.friction * deltaTime;
			_velocity += zero * config.acceleration * deltaTime;
			AddGravity(deltaTime);
			break;
		case Movement.Config.Type.AcceleratingWalking:
			_velocity.x *= 1f - config.friction * deltaTime;
			_velocity.x += zero.x * config.acceleration * deltaTime;
			if (Mathf.Abs(_velocity.x) > speed)
			{
				_velocity.x = speed * Mathf.Sign(_velocity.x);
			}
			AddGravity(deltaTime);
			break;
		case Movement.Config.Type.Bidimensional:
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
		_decorationCharacter.animationController.parameter.ySpeed = _velocity.y;
		if (deltaTime > 0f)
		{
			_velocity /= deltaTime;
			if ((config.type == Movement.Config.Type.AcceleratingFlying || config.type == Movement.Config.Type.Bidimensional) && ((Vector2)(ref _velocity)).sqrMagnitude > speed * speed)
			{
				_velocity = ((Vector2)(ref _velocity)).normalized * speed;
			}
			_controller.velocity = Vector2.op_Implicit(_velocity);
		}
		return zero;
	}

	private void AddGravity(float deltaTime)
	{
		if (!config.ignoreGravity)
		{
			_velocity.y += config.gravity * deltaTime;
		}
		if (_velocity.y < 0f - config.maxFallSpeed)
		{
			_velocity.y = 0f - config.maxFallSpeed;
		}
	}

	private void LateUpdate()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		Movement.Config config = this.config;
		if (config.type == Movement.Config.Type.Static)
		{
			return;
		}
		float num = _decorationCharacter.deltaTime;
		if (num != 0f)
		{
			if (num > 0.1f)
			{
				num = 0.1f;
			}
			_controller.UpdateBounds();
			_ = isGrounded;
			_moved = Vector2.zero;
			Vector2 val = HandleMove(num);
			if (config.type == Movement.Config.Type.Flying || config.type == Movement.Config.Type.AcceleratingFlying || config.type == Movement.Config.Type.Bidimensional)
			{
				_decorationCharacter.animationController.parameter.grounded = true;
			}
			else
			{
				_decorationCharacter.animationController.parameter.grounded = _controller.isGrounded;
			}
			force = Vector2.zero;
			if (!config.keepMove)
			{
				move = Vector2.zero;
			}
			if (val.y <= 0f && _controller.collisionState.below)
			{
				isGrounded = true;
			}
			else
			{
				isGrounded = false;
			}
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

	public void Move(float angle)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		move.x = Mathf.Cos(angle);
		move.y = Mathf.Sin(angle);
		lastDirection = move;
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
	}
}
