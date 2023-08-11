using System.Collections;
using UnityEngine;

namespace Characters.Operations;

public class ActivateChild : CharacterOperation
{
	[SerializeField]
	private Transform _parent;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _duration;

	[SerializeField]
	private float _interval;

	public override void Run(Character owner)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (_interval == 0f)
		{
			foreach (Transform item in _parent)
			{
				((Component)item).gameObject.SetActive(true);
			}
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CRun(owner.chronometer.master));
		}
		if (_duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CExpire(owner.chronometer.master));
		}
	}

	private IEnumerator CRun(Chronometer chronometer)
	{
		foreach (Transform item in _parent)
		{
			((Component)item).gameObject.SetActive(true);
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _interval);
		}
	}

	private IEnumerator CExpire(Chronometer chronometer)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _duration);
		Stop();
	}

	public override void Stop()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StopAllCoroutines();
		foreach (Transform item in _parent)
		{
			((Component)item).gameObject.SetActive(false);
		}
	}
}
