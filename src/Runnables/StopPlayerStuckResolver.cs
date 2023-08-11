using System.Collections;
using Characters.Movements;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public sealed class StopPlayerStuckResolver : Runnable
{
	[SerializeField]
	private float _duration;

	private float _remainTime;

	private CoroutineReference _cupdateReference;

	private StuckResolver _stuckResolver;

	public override void Run()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (_duration == 0f)
		{
			_remainTime = 2.1474836E+09f;
		}
		else
		{
			_remainTime = _duration;
		}
		_stuckResolver = ((Component)Singleton<Service>.Instance.levelManager.player).GetComponent<StuckResolver>();
		if (!((Object)(object)_stuckResolver == (Object)null))
		{
			((CoroutineReference)(ref _cupdateReference)).Stop();
			_cupdateReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CUpdate());
		}
	}

	private IEnumerator CUpdate()
	{
		_stuckResolver.stop.Attach((object)this);
		while (_remainTime > 0f)
		{
			yield return null;
			_remainTime -= ((ChronometerBase)Chronometer.global).deltaTime;
		}
		_stuckResolver.stop.Detach((object)this);
	}

	private void OnDestroy()
	{
		if (!Service.quitting && !((Object)(object)_stuckResolver == (Object)null))
		{
			_stuckResolver.stop.Detach((object)this);
		}
	}
}
