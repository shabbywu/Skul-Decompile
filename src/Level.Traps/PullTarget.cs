using System.Collections;
using Characters.AI;
using Characters.Movements;
using UnityEngine;

namespace Level.Traps;

public class PullTarget : Trap
{
	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private PushInfo _pushInfo = new PushInfo(ignoreOtherForce: false, expireOnGround: false);

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private IEnumerator CRun()
	{
		while (true)
		{
			yield return null;
			if (!((Object)(object)_controller.target == (Object)null) && ((ChronometerBase)Chronometer.global).timeScale != 0f)
			{
				_controller.target.movement.push.ApplyKnockback(_controller.character, _pushInfo);
			}
		}
	}
}
