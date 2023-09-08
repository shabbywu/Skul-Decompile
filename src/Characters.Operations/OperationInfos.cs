using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations;

public class OperationInfos : MonoBehaviour
{
	[SerializeField]
	private float _duration;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operationsOnEnd;

	private bool _running;

	private Character _owner;

	public float duration => _duration;

	public Character owner => _owner;

	public bool running => _running;

	public event Action onEnd;

	private void OnDisable()
	{
		if (_running)
		{
			_operations.StopAll();
			_running = false;
			this.onEnd?.Invoke();
		}
	}

	public void Initialize()
	{
		_operations.Initialize();
		_operationsOnEnd.Initialize();
	}

	public void Run(Character owner)
	{
		Run(owner, 1f);
	}

	public void Run(Character owner, float speed = 1f)
	{
		_owner = owner;
		((MonoBehaviour)this).StartCoroutine(CRun(speed));
	}

	private IEnumerator CRun(float speed)
	{
		_running = true;
		int operationIndex = 0;
		float time = 0f;
		OperationInfo[] components = _operations.components;
		while ((_duration == 0f && operationIndex < components.Length) || (_duration > 0f && time < _duration))
		{
			for (time += Chronometer.global.deltaTime * speed; operationIndex < components.Length && time >= components[operationIndex].timeToTrigger; operationIndex++)
			{
				if (((Component)components[operationIndex].operation).gameObject.activeSelf && ((Component)_owner).gameObject.activeSelf)
				{
					components[operationIndex].operation.Run(_owner);
				}
			}
			yield return null;
			if ((Object)(object)_owner == (Object)null || !((Component)_owner).gameObject.activeSelf)
			{
				break;
			}
		}
		Stop();
	}

	public void Stop()
	{
		_operations.StopAll();
		if ((Object)(object)_owner != (Object)null)
		{
			_operationsOnEnd.Run(_owner);
		}
		_running = false;
		this.onEnd?.Invoke();
		_owner = null;
		((Component)this).gameObject.SetActive(false);
	}
}
