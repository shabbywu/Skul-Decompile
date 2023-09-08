using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class RotateLaser : CharacterOperation
{
	[SerializeField]
	private Laser _laser;

	[SerializeField]
	private float _targetDirection;

	[SerializeField]
	private Curve _curve;

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		Vector2 direction = _laser.direction;
		float startDirection = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		float directionDistance = _targetDirection - startDirection;
		float time = 0f;
		while (time < _curve.duration)
		{
			_laser.Activate(owner, startDirection + directionDistance * _curve.Evaluate(time));
			time += owner.chronometer.master.deltaTime;
			yield return null;
		}
		_laser.Activate(owner, _targetDirection);
	}
}
