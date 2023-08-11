using System.Collections;
using EndingCredit;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public class ShowEndingCredit : CRunnable
{
	[SerializeField]
	private float _delay;

	private CreditRoll _creditRoll;

	public override IEnumerator CRun()
	{
		Singleton<Service>.Instance.levelManager.DestroyPlayer();
		_creditRoll = Scene<GameBase>.instance.uiManager.endingCredit;
		_creditRoll.Show();
		yield return Chronometer.global.WaitForSeconds(_delay);
		((MonoBehaviour)this).StartCoroutine(_creditRoll.CRun());
	}

	private void OnDisable()
	{
		if ((Object)(object)_creditRoll != (Object)null)
		{
			_creditRoll.Hide();
		}
	}
}
