using System.Collections;
using UnityEngine;

namespace Characters.Operations;

public class ActivateChild : CharacterOperation
{
	[SerializeField]
	private Transform _parent;

	[SerializeField]
	[Information("duration이 0이면 Operation이 끝날 때 Deactivate됨", InformationAttribute.InformationType.Info, false)]
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
			yield return chronometer.WaitForSeconds(_interval);
		}
	}

	private IEnumerator CExpire(Chronometer chronometer)
	{
		yield return chronometer.WaitForSeconds(_duration);
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
