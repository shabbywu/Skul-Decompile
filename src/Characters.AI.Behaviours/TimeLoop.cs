using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class TimeLoop : Decorator
{
	[SerializeField]
	private int _time;

	[Subcomponent(true)]
	[SerializeField]
	private Behaviour _behaviour;

	private bool _running;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		((MonoBehaviour)this).StartCoroutine(CExpire());
		while (_running)
		{
			yield return _behaviour.CRun(controller);
		}
		base.result = Result.Success;
	}

	private IEnumerator CExpire()
	{
		_running = true;
		yield return Chronometer.global.WaitForSeconds((float)_time);
		_running = false;
	}
}
