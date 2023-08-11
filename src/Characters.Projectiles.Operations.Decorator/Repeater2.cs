using System;
using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public class Repeater2 : Operation
{
	[SerializeField]
	private ReorderableFloatArray _timesToTrigger = new ReorderableFloatArray(new float[1]);

	[SerializeField]
	[Subcomponent]
	private Subcomponents _toRepeat;

	private void Awake()
	{
		Array.Sort(((ReorderableArray<float>)(object)_timesToTrigger).values);
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
				_toRepeat.Run(projectile);
			}
			yield return null;
			time += ((ChronometerBase)projectile.owner.chronometer.projectile).deltaTime;
		}
	}

	public override void Run(IProjectile projectile)
	{
		((MonoBehaviour)this).StartCoroutine(CRun(projectile));
	}
}
