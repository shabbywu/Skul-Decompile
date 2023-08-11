using System;
using System.Collections;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Level;

public class DropMovement : MonoBehaviour
{
	private const float _droppedGearHorizontalInterval = 1.5f;

	private const float _droppedGearBasicHorizontalSpeed = 0.5f;

	[SerializeField]
	private Transform _graphic;

	[SerializeField]
	private float _minDistanceFromGround = 1f;

	[SerializeField]
	private float _minDistanceFromSides = 1f;

	[SerializeField]
	private float _jumpPower = 15f;

	[SerializeField]
	private float _gravity = 40f;

	[SerializeField]
	private float _maxFallSpeed = 40f;

	[SerializeField]
	private float _floatAmplitude = 0.5f;

	[SerializeField]
	private float _floatFrequency = 1f;

	private float _movedHorizontalDistance;

	[NonSerialized]
	public float horizontalSpeed;

	[NonSerialized]
	public float maxHorizontalDistance;

	private bool _floating = true;

	private Vector2 _speed;

	private RayCaster _aboveCaster;

	private RayCaster _belowCaster;

	private RayCaster _leftCaster;

	private RayCaster _rightCaster;

	private float _cachedMinDistanceFromGround;

	public bool floating
	{
		get
		{
			return _floating;
		}
		set
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			_floating = value;
			_graphic.localPosition = Vector3.zero;
		}
	}

	public event Action onGround;

	public static void SetMultiDropHorizontalInterval(IList<DropMovement> dropMovements)
	{
		if (dropMovements.Count <= 1)
		{
			return;
		}
		int num = dropMovements.Count / 2;
		float num2 = ((dropMovements.Count % 2 == 0) ? 0.5f : 0f);
		if (dropMovements.Count % 2 == 0)
		{
			num--;
		}
		for (int i = 0; i <= num; i++)
		{
			DropMovement dropMovement = dropMovements[i];
			DropMovement dropMovement2 = dropMovements[dropMovements.Count - 1 - i];
			if ((Object)(object)dropMovement == (Object)(object)dropMovement2)
			{
				dropMovement.maxHorizontalDistance = 0f;
				continue;
			}
			float num3 = 1.5f * ((float)(num - i) + num2);
			float num4 = num3;
			dropMovement.horizontalSpeed = 0f - num4;
			dropMovement.maxHorizontalDistance = num3;
			dropMovement2.horizontalSpeed = num4;
			dropMovement2.maxHorizontalDistance = num3;
		}
	}

	public void ResetValue()
	{
		_minDistanceFromGround = _cachedMinDistanceFromGround;
		_floating = true;
	}

	public void SetMultipleRewardMovement(float mindistanceFromSides)
	{
		_cachedMinDistanceFromGround = _minDistanceFromGround;
		_minDistanceFromGround = mindistanceFromSides;
		_floating = false;
	}

	public void Pause()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}

	public void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		this.onGround?.Invoke();
	}

	public void Float()
	{
		((MonoBehaviour)this).StartCoroutine(CFloat());
	}

	private void Awake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		ContactFilter2D contactFilter = default(ContactFilter2D);
		((ContactFilter2D)(ref contactFilter)).SetLayerMask(Layers.groundMask);
		_aboveCaster = new RayCaster
		{
			direction = Vector2.up,
			contactFilter = contactFilter
		};
		_belowCaster = new RayCaster
		{
			direction = Vector2.down,
			contactFilter = contactFilter
		};
		_leftCaster = new RayCaster
		{
			direction = Vector2.left,
			contactFilter = contactFilter
		};
		_rightCaster = new RayCaster
		{
			direction = Vector2.right,
			contactFilter = contactFilter
		};
	}

	private void OnEnable()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_speed = new Vector2(0f, _jumpPower);
		_movedHorizontalDistance = 0f;
		horizontalSpeed = 0f;
		maxHorizontalDistance = 0f;
		((MonoBehaviour)this).StartCoroutine(CMove());
	}

	public void Move()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_speed = new Vector2(0f, _jumpPower);
		_movedHorizontalDistance = 0f;
		horizontalSpeed = 0f;
		maxHorizontalDistance = 0f;
		((MonoBehaviour)this).StopAllCoroutines();
		((MonoBehaviour)this).StartCoroutine(CMove());
	}

	public void Move(float speedX, float sppedY)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_speed = new Vector2(speedX, sppedY);
		_movedHorizontalDistance = 0f;
		horizontalSpeed = speedX;
		maxHorizontalDistance = 2.1474836E+09f;
		((MonoBehaviour)this).StopAllCoroutines();
		((MonoBehaviour)this).StartCoroutine(CMove());
	}

	private IEnumerator CMove()
	{
		yield return null;
		bool moveVertical = true;
		bool moveHorizontal = true;
		while (true)
		{
			float deltaTime = ((ChronometerBase)Chronometer.global).deltaTime;
			if (moveVertical)
			{
				_speed.y -= _gravity * deltaTime;
				if (_speed.y > 0f)
				{
					((Caster)_aboveCaster).origin = Vector2.op_Implicit(((Component)this).transform.position);
					((Caster)_aboveCaster).distance = _minDistanceFromGround + _speed.y * Time.deltaTime;
					if (RaycastHit2D.op_Implicit(((Caster)_aboveCaster).SingleCast()))
					{
						_speed.y = 0f;
					}
				}
				else
				{
					((Caster)_belowCaster).origin = Vector2.op_Implicit(((Component)this).transform.position);
					((Caster)_belowCaster).distance = _minDistanceFromGround - _speed.y * Time.deltaTime;
					((ContactFilter2D)(ref ((Caster)_belowCaster).contactFilter)).SetLayerMask(Layers.groundMask);
					RaycastHit2D val = ((Caster)_belowCaster).SingleCast();
					if (RaycastHit2D.op_Implicit(val))
					{
						((Component)this).transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref val)).point + new Vector2(0f, _minDistanceFromGround));
						_speed.y = 0f;
						moveVertical = false;
						Stop();
						Float();
					}
				}
			}
			if (moveHorizontal)
			{
				((Caster)_leftCaster).origin = Vector2.op_Implicit(((Component)this).transform.position);
				((Caster)_leftCaster).distance = _minDistanceFromSides + Mathf.Abs(_speed.x * Time.deltaTime);
				RaycastHit2D val2 = ((Caster)_leftCaster).SingleCast();
				((Caster)_rightCaster).origin = Vector2.op_Implicit(((Component)this).transform.position);
				((Caster)_rightCaster).distance = _minDistanceFromSides + Mathf.Abs(_speed.x * Time.deltaTime);
				RaycastHit2D val3 = ((Caster)_rightCaster).SingleCast();
				if (RaycastHit2D.op_Implicit(val2) && ((RaycastHit2D)(ref val2)).distance <= _minDistanceFromSides)
				{
					_speed.x += 2f * deltaTime;
				}
				else if (RaycastHit2D.op_Implicit(val3) && ((RaycastHit2D)(ref val3)).distance <= _minDistanceFromSides)
				{
					_speed.x -= 2f * deltaTime;
				}
				else if (_movedHorizontalDistance < maxHorizontalDistance)
				{
					_speed.x = horizontalSpeed;
					_movedHorizontalDistance += Mathf.Abs(_speed.x * deltaTime);
				}
				else
				{
					_speed.x = 0f;
					moveHorizontal = false;
				}
			}
			if (!moveHorizontal && !moveVertical)
			{
				break;
			}
			if (_speed.y < 0f - _maxFallSpeed)
			{
				_speed.y = 0f - _maxFallSpeed;
			}
			((Component)this).transform.Translate(Vector2.op_Implicit(_speed * deltaTime));
			yield return null;
		}
		Stop();
		Float();
	}

	private IEnumerator CFloat()
	{
		float t = 0f;
		while (true)
		{
			yield return null;
			if (_floating)
			{
				Vector3 zero = Vector3.zero;
				t += ((ChronometerBase)Chronometer.global).deltaTime;
				zero.y = Mathf.Sin(t * (float)Math.PI * _floatFrequency) * _floatAmplitude;
				_graphic.localPosition = zero;
			}
		}
	}
}
