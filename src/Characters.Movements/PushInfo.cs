using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Movements;

[Serializable]
public class PushInfo
{
	[SerializeField]
	internal bool ignoreOtherForce;

	[SerializeField]
	internal bool expireOnGround;

	[SerializeField]
	internal PushForce force1;

	[SerializeField]
	internal Curve curve1;

	[SerializeField]
	internal PushForce force2;

	[SerializeField]
	internal Curve curve2;

	internal (Vector2, Vector2) Evaluate(Transform from, ITarget to)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		return (force1.Evaluate(from, to), force2.Evaluate(from, to));
	}

	internal (Vector2, Vector2) Evaluate(IProjectile from, ITarget to)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		return (force1.Evaluate(from, to), force2.Evaluate(from, to));
	}

	internal (Vector2, Vector2) Evaluate(Character from, ITarget to)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		return (force1.Evaluate(from, to), force2.Evaluate(from, to));
	}

	internal (Vector2, Vector2) EvaluateTimeIndependent(Character from, ITarget to)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = force1.Evaluate(from, to);
		if (curve1.duration > 0f)
		{
			val /= curve1.duration;
		}
		Vector2 val2 = force2.Evaluate(from, to);
		if (curve2.duration > 0f)
		{
			val2 /= curve2.duration;
		}
		return (val, val2);
	}

	public PushInfo()
	{
		ignoreOtherForce = false;
		expireOnGround = false;
	}

	public PushInfo(bool ignoreOtherForce = false, bool expireOnGround = false)
	{
		this.ignoreOtherForce = ignoreOtherForce;
		this.expireOnGround = expireOnGround;
	}

	public PushInfo(PushForce pushForce, Curve curve, bool ignoreOtherForce = false, bool expireOnGround = false)
	{
		this.ignoreOtherForce = ignoreOtherForce;
		this.expireOnGround = expireOnGround;
		force1 = pushForce;
		curve1 = curve;
		force2 = new PushForce();
		curve2 = Curve.empty;
	}

	public PushInfo(PushForce force1, Curve curve1, PushForce force2, Curve curve2, bool ignoreOtherForce = false, bool expireOnGround = false)
	{
		this.ignoreOtherForce = ignoreOtherForce;
		this.expireOnGround = expireOnGround;
		this.force1 = force1;
		this.curve1 = curve1;
		this.force2 = force2;
		this.curve2 = curve2;
	}
}
