using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters;

[Serializable]
public class PushForce
{
	public enum Method
	{
		OwnerDirection,
		OutsideCenter,
		Constant,
		LastSmashedDirection
	}

	public enum DirectionComputingMethod
	{
		XOnly,
		YOnly,
		Both
	}

	[SerializeField]
	private Method _method;

	[SerializeField]
	private CustomFloat _angle;

	[SerializeField]
	private CustomFloat _power;

	[SerializeField]
	private DirectionComputingMethod _directionMethod;

	[SerializeField]
	private Transform _centerTransform;

	[SerializeField]
	private Collider2D _centerCollider;

	private Vector2? _force;

	public CustomFloat angle
	{
		get
		{
			return _angle;
		}
		set
		{
			_angle = value;
		}
	}

	public CustomFloat power
	{
		get
		{
			return _power;
		}
		set
		{
			_power = value;
		}
	}

	public PushForce()
	{
		_method = Method.OwnerDirection;
		_angle = new CustomFloat(0f);
		_power = new CustomFloat(0f);
		_directionMethod = DirectionComputingMethod.XOnly;
	}

	private Vector2 EvaluateDirection(Vector2 originalDirection)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		switch (_directionMethod)
		{
		case DirectionComputingMethod.XOnly:
			originalDirection.y = 0f;
			break;
		case DirectionComputingMethod.YOnly:
			originalDirection.x = 0f;
			break;
		}
		return ((Vector2)(ref originalDirection)).normalized;
	}

	private Vector2 EvaluateOutsideCenter(Transform from, ITarget to, Vector2 force)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val;
		if ((Object)(object)_centerTransform != (Object)null)
		{
			val = _centerTransform.position;
		}
		else if ((Object)(object)_centerCollider != (Object)null)
		{
			Bounds bounds = _centerCollider.bounds;
			val = ((Bounds)(ref bounds)).center;
		}
		else
		{
			val = ((Component)from).transform.position;
		}
		Vector2 val2 = EvaluateDirection(Vector2.op_Implicit(to.transform.position - val));
		return new Vector2(force.x * val2.x - force.y * val2.y, force.x * val2.y + force.y * val2.x);
	}

	private Vector2 RotateByDirection(Vector2 direction, Vector2 value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(value.x * direction.x - value.y * direction.y, value.x * direction.y + value.y * direction.x);
	}

	public Vector2 Evaluate(Transform from, ITarget to)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		float value = _angle.value;
		Vector2 val = new Vector2(Mathf.Cos(value * ((float)Math.PI / 180f)), Mathf.Sin(value * ((float)Math.PI / 180f))) * _power.value;
		switch (_method)
		{
		case Method.OwnerDirection:
		{
			bool flag = false;
			if (from.lossyScale.x < 0f)
			{
				flag = !flag;
			}
			Quaternion rotation = from.rotation;
			if (((Quaternion)(ref rotation)).eulerAngles.z == 180f)
			{
				flag = !flag;
			}
			if (flag)
			{
				val.x *= -1f;
			}
			break;
		}
		case Method.OutsideCenter:
			val = EvaluateOutsideCenter(((Component)from).transform, to, val);
			break;
		case Method.LastSmashedDirection:
			if ((Object)(object)to.character != (Object)null)
			{
				val = RotateByDirection(EvaluateDirection(to.character.movement.push.direction), val);
			}
			break;
		}
		return val;
	}

	public Vector2 Evaluate(IProjectile from, ITarget to)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		float value = _angle.value;
		Vector2 val = new Vector2(Mathf.Cos(value * ((float)Math.PI / 180f)), Mathf.Sin(value * ((float)Math.PI / 180f))) * _power.value;
		switch (_method)
		{
		case Method.OwnerDirection:
		{
			Vector2 direction = from.direction;
			((Vector2)(ref val))._002Ector(val.x * direction.x - val.y * direction.y, val.x * direction.y + val.y * direction.x);
			break;
		}
		case Method.OutsideCenter:
			val = EvaluateOutsideCenter(from.transform, to, val);
			break;
		case Method.LastSmashedDirection:
			if ((Object)(object)to.character != (Object)null)
			{
				val = RotateByDirection(EvaluateDirection(to.character.movement.push.direction), val);
			}
			break;
		}
		return val;
	}

	public Vector2 Evaluate(Character from, ITarget to)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		float value = _angle.value;
		Vector2 val = new Vector2(Mathf.Cos(value * ((float)Math.PI / 180f)), Mathf.Sin(value * ((float)Math.PI / 180f))) * _power.value;
		switch (_method)
		{
		case Method.OwnerDirection:
			if (from.lookingDirection != 0)
			{
				val.x *= -1f;
			}
			break;
		case Method.OutsideCenter:
			val = EvaluateOutsideCenter(((Component)from).transform, to, val);
			break;
		case Method.LastSmashedDirection:
			if ((Object)(object)to.character != (Object)null)
			{
				val = RotateByDirection(EvaluateDirection(to.character.movement.push.direction), val);
			}
			break;
		}
		return val;
	}
}
