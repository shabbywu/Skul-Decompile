using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace Level.Traps;

public class Compressor : ControlableTrap
{
	[SerializeField]
	private Character _character;

	[Range(0f, 1f)]
	[SerializeField]
	private float _startTiming;

	[SerializeField]
	private float _interval;

	[SerializeField]
	private Action _action;

	private IEnumerator _coroutine;

	private void Awake()
	{
		_coroutine = CRun();
	}

	public override void Activate()
	{
		((MonoBehaviour)this).StartCoroutine(_coroutine);
	}

	private IEnumerator CRun()
	{
		yield return Chronometer.global.WaitForSeconds(_interval * _startTiming);
		while (true)
		{
			_action.TryStart();
			yield return Chronometer.global.WaitForSeconds(_interval);
		}
	}

	public override void Deactivate()
	{
		((MonoBehaviour)this).StopCoroutine(_coroutine);
	}
}
