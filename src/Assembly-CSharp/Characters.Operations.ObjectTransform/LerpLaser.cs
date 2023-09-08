using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class LerpLaser : CharacterOperation
{
	[SerializeField]
	private Laser _laser;

	[SerializeField]
	private CustomAngle _fromDirection;

	[SerializeField]
	private CustomAngle _toDirection;

	[SerializeField]
	private float _fromDirectionTest;

	[SerializeField]
	private float _toDirectionTest;

	[SerializeField]
	private Curve _curve;

	private float _fromValue;

	private float _toValue;

	public override void Run(Character owner)
	{
		_fromValue = _fromDirectionTest;
		_toValue = _toDirectionTest;
		_laser.Activate(owner, _fromValue);
		if (_curve.duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CRun(owner));
		}
	}

	private IEnumerator CRun(Character owner)
	{
		float elapsed = 0f;
		while (elapsed < _curve.duration)
		{
			float direction = Mathf.Lerp(_fromValue, _toValue, _curve.Evaluate(elapsed));
			_laser.Activate(owner, direction);
			elapsed += owner.chronometer.master.deltaTime;
			yield return null;
		}
		_laser.Deactivate();
	}

	public override void Stop()
	{
		base.Stop();
		((MonoBehaviour)this).StopAllCoroutines();
		_laser.Deactivate();
	}
}
