using System.Collections;
using UnityEngine;

namespace Characters.Operations;

public class Guard : CharacterOperation
{
	[SerializeField]
	private Characters.Guard _guard;

	[SerializeField]
	private float _duration;

	public override void Run(Character owner)
	{
		_guard.Initialize(owner);
		_guard.GuardUp();
		if (_duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CExpire(owner));
		}
	}

	private IEnumerator CExpire(Character owner)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _duration);
		Stop();
	}

	public override void Stop()
	{
		_guard.GuardDown();
	}
}
