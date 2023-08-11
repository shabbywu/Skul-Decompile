using System.Collections;
using UnityEngine;

namespace Characters.Operations.Health;

public class Invulnerable : CharacterOperation
{
	[SerializeField]
	[FrameTime]
	private float _duration;

	private CoroutineReference _runReference;

	private Character _owner;

	public override void Run(Character owner)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		_owner = owner;
		((CoroutineReference)(ref _runReference)).Stop();
		_runReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRun(owner));
	}

	private IEnumerator CRun(Character character)
	{
		if (_duration == 0f)
		{
			_duration = 2.1474836E+09f;
		}
		character.invulnerable.Attach((object)this);
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)character.chronometer.master, _duration);
		character.invulnerable.Detach((object)this);
	}

	public override void Stop()
	{
		if ((Object)(object)_owner != (Object)null)
		{
			_owner.invulnerable.Detach((object)this);
		}
	}
}
