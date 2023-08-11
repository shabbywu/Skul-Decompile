using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Decorator;

public class Repeater2 : CharacterOperation
{
	[SerializeField]
	private ReorderableFloatArray _timesToTrigger = new ReorderableFloatArray(new float[1]);

	[SerializeField]
	[Subcomponent]
	private Subcomponents _toRepeat;

	private CoroutineReference _repeatCoroutineReference;

	private void Awake()
	{
		Array.Sort(((ReorderableArray<float>)(object)_timesToTrigger).values);
	}

	public override void Initialize()
	{
		_toRepeat.Initialize();
	}

	internal IEnumerator CRun(Character owner, Character target)
	{
		int operationIndex = 0;
		float time = 0f;
		float[] timesToTrigger = ((ReorderableArray<float>)(object)_timesToTrigger).values;
		while (operationIndex < timesToTrigger.Length)
		{
			for (; operationIndex < timesToTrigger.Length && time >= timesToTrigger[operationIndex]; operationIndex++)
			{
				_toRepeat.Run(owner, target);
			}
			yield return null;
			time += ((ChronometerBase)owner.chronometer.animation).deltaTime * runSpeed;
		}
	}

	public override void Run(Character owner)
	{
		Run(owner, owner);
	}

	public override void Run(Character owner, Character target)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		_repeatCoroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRun(owner, target));
	}

	public override void Stop()
	{
		_toRepeat.Stop();
		((CoroutineReference)(ref _repeatCoroutineReference)).Stop();
	}
}
