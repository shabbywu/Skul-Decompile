using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class ActivateLaser : CharacterOperation
{
	[SerializeField]
	private Laser _laser;

	[SerializeField]
	private CustomAngle _direction;

	[SerializeField]
	private float _duration;

	public override void Run(Character owner)
	{
		_laser.Activate(owner, _direction.value);
		if (_duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CRun(owner));
		}
	}

	private IEnumerator CRun(Character owner)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _duration);
		_laser.Deactivate();
	}

	public override void Stop()
	{
		base.Stop();
		((MonoBehaviour)this).StopAllCoroutines();
		_laser.Deactivate();
	}
}
