using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public class Repeater3 : Operation
{
	[SerializeField]
	private ReorderableFloatArray _timesToTrigger = new ReorderableFloatArray(new float[1]);

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	private CoroutineReference[] _repeatCoroutineReferences;

	private void Awake()
	{
		Array.Sort(((ReorderableArray<float>)(object)_timesToTrigger).values);
		_repeatCoroutineReferences = (CoroutineReference[])(object)new CoroutineReference[((ReorderableArray<float>)(object)_timesToTrigger).values.Length];
	}

	internal IEnumerator CRun(IProjectile projectile)
	{
		int operationIndex = 0;
		float time = 0f;
		float[] timesToTrigger = ((ReorderableArray<float>)(object)_timesToTrigger).values;
		while (operationIndex < timesToTrigger.Length)
		{
			for (; operationIndex < timesToTrigger.Length && time >= timesToTrigger[operationIndex]; operationIndex++)
			{
				((MonoBehaviour)this).StartCoroutine(_operations.CRun(projectile));
			}
			yield return null;
			time += ((ChronometerBase)projectile.owner.chronometer.projectile).deltaTime;
		}
	}

	public override void Run(IProjectile projectile)
	{
		Run(projectile);
	}
}
