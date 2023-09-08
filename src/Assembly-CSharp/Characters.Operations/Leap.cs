using System.Collections;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class Leap : CharacterOperation
{
	[SerializeField]
	private AIController _aiController;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private float _duration;

	private void Awake()
	{
		if ((Object)(object)_target != (Object)null)
		{
			((Component)_target).transform.parent = null;
		}
	}

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CMoveToTarget(owner));
	}

	public override void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private IEnumerator CMoveToTarget(Character owner)
	{
		float destination = (((Object)(object)_target == (Object)null) ? ((Component)_aiController.target).transform.position.x : ((Component)_target).transform.position.x);
		float source = ((Component)owner).transform.position.x;
		float elapsed = 0f;
		while (true)
		{
			yield return null;
			float num = Mathf.Lerp(source, destination, elapsed / _duration);
			owner.movement.force.x = num - ((Component)owner).transform.position.x;
			if (!owner.stunedOrFreezed)
			{
				if (elapsed > _duration)
				{
					break;
				}
				elapsed += owner.chronometer.master.deltaTime;
			}
		}
	}
}
