using System.Collections;
using UnityEngine;

namespace Characters.Operations.Health;

public sealed class Cinematic : CharacterOperation
{
	[FrameTime]
	[SerializeField]
	private float _duration;

	private CoroutineReference _runReference;

	private Character _owner;

	public override void Run(Character owner)
	{
		_owner = owner;
		_runReference.Stop();
		_runReference = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(CRun(owner));
	}

	private IEnumerator CRun(Character character)
	{
		if (_duration == 0f)
		{
			_duration = 2.1474836E+09f;
		}
		character.cinematic.Attach(this);
		yield return character.chronometer.master.WaitForSeconds(_duration);
		character.cinematic.Detach(this);
	}

	public override void Stop()
	{
		if ((Object)(object)_owner != (Object)null)
		{
			_owner.cinematic.Detach(this);
		}
	}
}
