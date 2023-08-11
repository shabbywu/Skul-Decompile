using System.Collections;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class Leap2 : CharacterOperation
{
	[SerializeField]
	private AIController _aiController;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private float _checkDistance = 0.5f;

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
		while (true)
		{
			float num = destination - source;
			Vector2 val = ((num > 0f) ? Vector2.right : Vector2.left);
			owner.movement.MoveHorizontal(val);
			if (num > 0f)
			{
				if (((Component)owner).transform.position.x + val.x * _checkDistance > destination)
				{
					break;
				}
			}
			else if (((Component)owner).transform.position.x + val.x * _checkDistance < destination)
			{
				break;
			}
			yield return null;
		}
	}
}
