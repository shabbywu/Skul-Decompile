using System.Collections;
using UnityEngine;

namespace Characters.Operations;

public class ActivateGameObjectOperation : CharacterOperation
{
	[SerializeField]
	private GameObject _gameObject;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private bool _deactivate;

	[SerializeField]
	private Transform _activatedPosition;

	private CoroutineReference _stopCoroutineReference;

	public override void Run(Character owner)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_gameObject == (Object)null)
		{
			_gameObject = ((Component)owner).gameObject;
		}
		if (_deactivate)
		{
			_gameObject.SetActive(false);
			return;
		}
		if ((Object)(object)_activatedPosition != (Object)null)
		{
			_gameObject.transform.position = _activatedPosition.position;
		}
		_gameObject.SetActive(true);
		RuntimeAnimatorController component = _gameObject.GetComponent<RuntimeAnimatorController>();
		if ((Object)(object)component != (Object)null && _duration == 0f)
		{
			_duration = component.animationClips[0].length;
		}
		if (_duration > 0f)
		{
			_stopCoroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CStop(owner.chronometer.animation));
		}
	}

	private IEnumerator CStop(Chronometer chronometer)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _duration);
		Stop();
	}

	public override void Stop()
	{
		((CoroutineReference)(ref _stopCoroutineReference)).Stop();
		if ((Object)(object)_gameObject != (Object)null)
		{
			_gameObject.SetActive(false);
		}
	}
}
