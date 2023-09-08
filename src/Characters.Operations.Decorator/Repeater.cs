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
		float interval = _interval / runSpeed;
		_repeatCoroutineReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CRepeat());
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
					yield return owner.chronometer.animation.WaitForSeconds(interval);
				}
			}
		}
	}

	public override void Run(Character owner, Character target)
	{
		float interval = _interval / runSpeed;
		_repeatCoroutineReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CRepeat());
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
					yield return owner.chronometer.animation.WaitForSeconds(interval);
				}
			}
		}
	}

	public override void Stop()
	{
		_toRepeat.Stop();
		_repeatCoroutineReference.Stop();
	}
}
