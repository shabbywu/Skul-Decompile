using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Weapons.GrimReaper;

public sealed class GrimReaperSoul : MonoBehaviour
{
	public enum RotateMethod
	{
		Constant,
		Lerp,
		Slerp
	}

	private GrimReaperHarvestPassive _grimReaperHarvestPassive;

	private GrimReaperPassive _grimReperPassive;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onGain;

	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	private float _delay;

	[SerializeField]
	private RotateMethod _rotateMethod;

	[SerializeField]
	private float _rotateSpeed = 2f;

	[SerializeField]
	private float _startRotationRange = 45f;

	[SerializeField]
	private float _startSpeed;

	[SerializeField]
	private Curve _speedIncreasing;

	private Character _target;

	private Vector2 _directionVector;

	private float _direction;

	private Quaternion _rotation;

	private float _currentRotateSpeed;

	private float _elapsed;

	public void Spawn(Vector3 postion, GrimReaperHarvestPassive grimReaperHarvestPassive)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		GrimReaperSoul component = ((Component)_poolObject.Spawn(postion, true)).GetComponent<GrimReaperSoul>();
		component._grimReperPassive = null;
		component._grimReaperHarvestPassive = grimReaperHarvestPassive;
		component.MoveToGrimReaper();
	}

	public void Spawn(Vector3 postion, GrimReaperPassive grimReaperPassive)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		GrimReaperSoul component = ((Component)_poolObject.Spawn(postion, true)).GetComponent<GrimReaperSoul>();
		component._grimReaperHarvestPassive = null;
		component._grimReperPassive = grimReaperPassive;
		component.MoveToGrimReaper();
	}

	private void MoveToGrimReaper()
	{
		_onGain.Initialize();
		((MonoBehaviour)this).StartCoroutine(CMoveToGrimReaper());
	}

	private IEnumerator CMoveToGrimReaper()
	{
		_target = ((_grimReperPassive == null) ? _grimReaperHarvestPassive.owner : _grimReperPassive.owner);
		GrimReaperSoul grimReaperSoul = this;
		Vector3 val = ((Component)this).transform.position - ((Component)_target).transform.position;
		grimReaperSoul._directionVector = Vector2.op_Implicit(((Vector3)(ref val)).normalized);
		_directionVector = Vector2.op_Implicit(Quaternion.AngleAxis(Random.Range(0f - _startRotationRange, _startRotationRange), Vector3.forward) * Vector2.op_Implicit(_directionVector));
		_direction = Mathf.Atan2(_directionVector.y, _directionVector.x) * 57.29578f;
		_rotation = Quaternion.Euler(0f, 0f, _direction);
		_currentRotateSpeed = _rotateSpeed;
		_elapsed = 0f;
		while (true)
		{
			yield return null;
			if ((Object)(object)_target == (Object)null)
			{
				Despawn();
			}
			float deltaTime = ((ChronometerBase)Chronometer.global).deltaTime;
			_elapsed += deltaTime;
			if (_elapsed >= _delay)
			{
				UpdateDirection(deltaTime);
				_currentRotateSpeed += deltaTime * 4f;
				Vector3 position = ((Component)this).transform.position;
				Bounds bounds = ((Collider2D)_target.collider).bounds;
				if (Vector2.SqrMagnitude(Vector2.op_Implicit(position - ((Bounds)(ref bounds)).center)) < 1f)
				{
					break;
				}
			}
			float num = _startSpeed + _speedIncreasing.Evaluate(_elapsed) * deltaTime;
			((Component)this).transform.Translate(Vector2.op_Implicit(_directionVector * num), (Space)0);
		}
		PickedUp();
	}

	private void PickedUp()
	{
		if (_grimReperPassive != null)
		{
			((MonoBehaviour)_grimReperPassive.owner).StartCoroutine(_onGain.CRun(_grimReperPassive.owner));
			_grimReperPassive.AddStack();
		}
		else
		{
			((MonoBehaviour)_grimReaperHarvestPassive.owner).StartCoroutine(_onGain.CRun(_grimReaperHarvestPassive.owner));
			_grimReaperHarvestPassive.AddStack();
		}
		Despawn();
	}

	private void Despawn()
	{
		_poolObject.Despawn();
	}

	private void UpdateDirection(float deltaTime)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_target == (Object)null))
		{
			Bounds bounds = ((Collider2D)_target.collider).bounds;
			Vector3 val = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			switch (_rotateMethod)
			{
			case RotateMethod.Constant:
				_rotation = Quaternion.RotateTowards(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _currentRotateSpeed * 100f * deltaTime);
				break;
			case RotateMethod.Lerp:
				_rotation = Quaternion.Lerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _currentRotateSpeed * deltaTime);
				break;
			case RotateMethod.Slerp:
				_rotation = Quaternion.Slerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _currentRotateSpeed * deltaTime);
				break;
			}
			_direction = ((Quaternion)(ref _rotation)).eulerAngles.z;
			_directionVector = Vector2.op_Implicit(_rotation * Vector3.right);
		}
	}
}
