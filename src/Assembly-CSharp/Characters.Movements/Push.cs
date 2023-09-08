using Characters.Projectiles;
using UnityEngine;

namespace Characters.Movements;

public sealed class Push
{
	public delegate void OnSmashEndDelegate(Push push, Character from, Character to, SmashEndType endType, RaycastHit2D? raycastHit, Movement.CollisionDirection direction);

	public enum SmashEndType
	{
		Expire,
		Collide,
		Cancel
	}

	private readonly Character _owner;

	private Character _from;

	private (Vector2, Vector2) _force;

	private (Curve, Curve) _curve;

	private float _time;

	private (float, float) _amountBefore;

	public bool ignoreOtherForce { get; private set; }

	public bool expireOnGround { get; private set; }

	public bool smash { get; private set; }

	public bool expired { get; private set; } = true;


	public Vector2 direction { get; private set; }

	public Vector2 totalForce { get; private set; }

	public float duration { get; private set; }

	public event OnSmashEndDelegate onEnd;

	internal Push(Character owner)
	{
		_owner = owner;
	}

	private void Intialize()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		_time = 0f;
		_amountBefore = (0f, 0f);
		Vector2 val = _force.Item1 + _force.Item2;
		direction = ((Vector2)(ref val)).normalized;
		totalForce = _force.Item1 * _curve.Item1.valueMultiplier + _force.Item2 * _curve.Item2.valueMultiplier;
		expired = false;
		duration = Mathf.Max(_curve.Item1.duration, _curve.Item2.duration);
	}

	public bool ApplySmash(Character from, Vector2 force, Curve curve, bool ignoreOtherForce = false, bool expireOnGround = false, OnSmashEndDelegate onEnd = null)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (smash && !expired)
		{
			return false;
		}
		_from = from;
		_force = (force, Vector2.zero);
		_curve = (curve, Curve.empty);
		this.ignoreOtherForce = ignoreOtherForce;
		this.expireOnGround = expireOnGround;
		this.onEnd = onEnd;
		smash = true;
		Intialize();
		return true;
	}

	public void ApplySmash(Character from, PushInfo info, OnSmashEndDelegate onEnd = null)
	{
		_from = from;
		_force = info.Evaluate(from, new TargetStruct(_owner));
		_curve = (info.curve1, info.curve2);
		ignoreOtherForce = info.ignoreOtherForce;
		expireOnGround = info.expireOnGround;
		this.onEnd = onEnd;
		smash = true;
		Intialize();
	}

	public void ApplySmash(Character from, Transform forceFrom, PushInfo info, OnSmashEndDelegate onEnd = null)
	{
		_from = from;
		_force = info.Evaluate(forceFrom, new TargetStruct(_owner));
		_curve = (info.curve1, info.curve2);
		ignoreOtherForce = info.ignoreOtherForce;
		expireOnGround = info.expireOnGround;
		this.onEnd = onEnd;
		smash = true;
		Intialize();
	}

	public bool ApplyKnockback(Character from, Vector2 force, Curve curve, bool ignoreOtherForce = false, bool expireOnGround = false)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (smash && !expired)
		{
			return false;
		}
		_from = from;
		_force = (force, Vector2.zero);
		_curve = (curve, Curve.empty);
		this.ignoreOtherForce = ignoreOtherForce;
		this.expireOnGround = expireOnGround;
		smash = false;
		Intialize();
		return true;
	}

	public bool ApplyKnockback(Transform from, PushInfo info)
	{
		if (smash && !expired)
		{
			return false;
		}
		_from = null;
		_force = info.Evaluate(from, new TargetStruct(_owner));
		_curve = (info.curve1, info.curve2);
		ignoreOtherForce = info.ignoreOtherForce;
		expireOnGround = info.expireOnGround;
		smash = false;
		Intialize();
		return true;
	}

	public bool ApplyKnockback(IProjectile from, PushInfo info)
	{
		if (smash && !expired)
		{
			return false;
		}
		_from = from.owner;
		_force = info.Evaluate(from, new TargetStruct(_owner));
		_curve = (info.curve1, info.curve2);
		ignoreOtherForce = info.ignoreOtherForce;
		expireOnGround = info.expireOnGround;
		smash = false;
		Intialize();
		return true;
	}

	public bool ApplyKnockback(Character from, PushInfo info)
	{
		if (smash && !expired)
		{
			return false;
		}
		_from = from;
		_force = info.Evaluate(from, new TargetStruct(_owner));
		_curve = (info.curve1, info.curve2);
		ignoreOtherForce = info.ignoreOtherForce;
		expireOnGround = info.expireOnGround;
		smash = false;
		Intialize();
		return true;
	}

	private bool UpdateForce(ref Vector2 forceVector, Vector2 force, Curve curve, ref float amountBefore)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		float num = curve.Evaluate(_time);
		forceVector += force * (num - amountBefore);
		amountBefore = num;
		if (!(_time > curve.duration))
		{
			return curve.duration == 0f;
		}
		return true;
	}

	internal void Update(out Vector2 vector, float deltaTime)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		vector = Vector2.zero;
		if (duration == 0f)
		{
			vector = _force.Item1 + _force.Item2;
			expired = true;
			this.onEnd?.Invoke(this, _from, _owner, SmashEndType.Expire, null, Movement.CollisionDirection.None);
			return;
		}
		bool num = UpdateForce(ref vector, _force.Item1, _curve.Item1, ref _amountBefore.Item1);
		bool flag = UpdateForce(ref vector, _force.Item2, _curve.Item2, ref _amountBefore.Item2);
		_time += deltaTime;
		if (num && flag)
		{
			expired = true;
			this.onEnd?.Invoke(this, _from, _owner, SmashEndType.Expire, null, Movement.CollisionDirection.None);
		}
	}

	internal void CollideWith(RaycastHit2D raycastHit, Movement.CollisionDirection direction)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (!expired)
		{
			expired = true;
			this.onEnd?.Invoke(this, _from, _owner, SmashEndType.Collide, raycastHit, direction);
		}
	}

	internal void Expire()
	{
		expired = true;
	}
}
