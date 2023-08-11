using System;
using System.Collections;
using Characters;
using Characters.Operations;
using Characters.Operations.Attack;
using Characters.Utils;
using UnityEngine;

namespace Level.Traps;

public sealed class RotateLaser : MonoBehaviour
{
	[Serializable]
	private class Rotate
	{
		[SerializeField]
		internal float delay;

		[SerializeField]
		internal float amount;

		[SerializeField]
		internal float delta;
	}

	[SerializeField]
	private SweepAttack[] _attackOperations;

	[SerializeField]
	private Transform _body;

	[SerializeField]
	private Rotate _rotate;

	[SerializeField]
	private float _loopTime;

	[SerializeField]
	private AnimationClip _endAnimation;

	private Character _owner;

	private HitHistoryManager _hitHistoryManager;

	private int _direction;

	private float _speed;

	private void Awake()
	{
		_hitHistoryManager = new HitHistoryManager(99999);
		SweepAttack[] attackOperations = _attackOperations;
		foreach (SweepAttack obj in attackOperations)
		{
			obj.Initialize();
			obj.collisionDetector.hits = _hitHistoryManager;
		}
	}

	private void OnDestroy()
	{
		_endAnimation = null;
	}

	public void Activate(OperationInfos operationInfos)
	{
		_owner = operationInfos.owner;
		((Component)this).gameObject.SetActive(true);
	}

	public void Activate(Character owner)
	{
		_owner = owner;
		((Component)this).gameObject.SetActive(true);
	}

	private void OnEnable()
	{
		ResetSetting();
		((MonoBehaviour)this).StartCoroutine(CStart(_owner.chronometer.master));
	}

	private IEnumerator CStart(Chronometer chronometer)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _rotate.delay);
		((MonoBehaviour)this).StartCoroutine(CLoop(chronometer));
	}

	private IEnumerator CLoop(Chronometer chronometer)
	{
		SweepAttack[] attackOperations = _attackOperations;
		for (int i = 0; i < attackOperations.Length; i++)
		{
			attackOperations[i].Run(_owner);
		}
		float elapsed = 0f;
		while (elapsed < _loopTime)
		{
			if (_speed < _rotate.amount)
			{
				_speed += _rotate.delta;
			}
			((Component)_body).transform.Rotate(Vector3.forward, (float)_direction * _speed * ((ChronometerBase)chronometer).deltaTime);
			elapsed += ((ChronometerBase)chronometer).deltaTime;
			yield return null;
		}
		((MonoBehaviour)this).StartCoroutine(CEnd(chronometer));
	}

	private IEnumerator CEnd(Chronometer chronometer)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _endAnimation.length);
		Hide();
	}

	private void ResetSetting()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_speed = 0f;
		_body.rotation = Quaternion.identity;
		_direction = (MMMaths.RandomBool() ? 1 : (-1));
		_hitHistoryManager.Clear();
	}

	private void Hide()
	{
		((Component)this).gameObject.SetActive(false);
	}
}
