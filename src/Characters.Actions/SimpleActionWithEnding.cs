using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class SimpleActionWithEnding : SimpleAction
{
	[Header("Ending")]
	[SerializeField]
	private bool _endingOpreationsOnEnd = true;

	[SerializeField]
	private bool _endingOpreationsOnCancel = true;

	[SerializeField]
	private float _endingOperationDuration;

	[SerializeField]
	[Space]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _endingOperations;

	protected override void Awake()
	{
		base.Awake();
		if (_endingOpreationsOnEnd)
		{
			_onEnd = (System.Action)Delegate.Combine(_onEnd, new System.Action(Run));
		}
		if (_endingOpreationsOnCancel)
		{
			_onCancel = (System.Action)Delegate.Combine(_onCancel, new System.Action(Run));
		}
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		_endingOperations.Initialize();
	}

	private void Run()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private IEnumerator CRun()
	{
		int operationIndex = 0;
		float time = 0f;
		OperationInfo[] components = ((SubcomponentArray<OperationInfo>)_endingOperations).components;
		while ((_endingOperationDuration == 0f && operationIndex < components.Length) || (_endingOperationDuration > 0f && time < _endingOperationDuration))
		{
			for (time += ((ChronometerBase)Chronometer.global).deltaTime; operationIndex < components.Length && time >= components[operationIndex].timeToTrigger; operationIndex++)
			{
				components[operationIndex].operation.Run(base.owner);
			}
			yield return null;
			if ((Object)(object)base.owner == (Object)null || !((Component)base.owner).gameObject.activeSelf)
			{
				break;
			}
		}
		_endingOperations.StopAll();
	}
}
