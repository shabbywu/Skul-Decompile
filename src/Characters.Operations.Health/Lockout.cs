using System.Collections;
using UnityEngine;

namespace Characters.Operations.Health;

public sealed class Lockout : CharacterOperation
{
	[SerializeField]
	[FrameTime]
	private float _duration;

	private CoroutineReference _runReference;

	private Character _owner;

	public override void Run(Character owner)
	{
		_owner = owner;
		_runReference.Stop();
		_runReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CRun(owner));
	}

	private IEnumerator CRun(Character character)
	{
		if (_duration == 0f)
		{
			_duration = 2.1474836E+09f;
		}
		character.status.unstoppable.Attach(this);
		yield return character.chronometer.master.WaitForSeconds(_duration);
		character.status.unstoppable.Detach(this);
	}

	public override void Stop()
	{
		if ((Object)(object)_owner != (Object)null)
		{
			_owner.status.unstoppable.Detach(this);
		}
	}
}
