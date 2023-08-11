using System.Collections;
using UnityEngine;

namespace Characters.Operations.Decorator;

public class Repeater : CharacterOperation
{
	[SerializeField]
	private int _times;

	[SerializeField]
	private float _interval;

	[SerializeField]
	private bool _timeIndependant;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation _toRepeat;

	private CoroutineReference _repeatCoroutineReference;

	public override void Initialize()
	{
		_toRepeat.Initialize();
	}

	private void Awake()
	{
		if (_times == 0)
		{
			_times = int.MaxValue;
		}
	}

	public override void Run(Character owner)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		float interval = _interval / runSpeed;
		_repeatCoroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRepeat());
		IEnumerator CRepeat()
		{
			for (int i = 0; i < _times; i++)
			{
				_toRepeat.Stop();
				_toRepeat.Run(owner);
				if (_timeIndependant)
				{
					yield return Chronometer.global.WaitForSeconds(interval);
				}
				else
				{
					yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.animation, interval);
				}
			}
		}
	}

	public override void Run(Character owner, Character target)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		float interval = _interval / runSpeed;
		_repeatCoroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRepeat());
		IEnumerator CRepeat()
		{
			for (int i = 0; i < _times; i++)
			{
				_toRepeat.Stop();
				_toRepeat.Run(owner, target);
				if (_timeIndependant)
				{
					yield return Chronometer.global.WaitForSeconds(interval);
				}
				else
				{
					yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.animation, interval);
				}
			}
		}
	}

	public override void Stop()
	{
		_toRepeat.Stop();
		((CoroutineReference)(ref _repeatCoroutineReference)).Stop();
	}
}
