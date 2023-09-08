using System.Collections;
using EndingCredit;
using Level;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public sealed class ShowHardmodeEndingCredit : CRunnable
{
	[SerializeField]
	private float _delay;

	private CreditRoll _creditRoll;

	public override IEnumerator CRun()
	{
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		levelManager.player.playerComponents.inventory.item.RemoveAll();
		levelManager.player.playerComponents.inventory.quintessence.RemoveAll();
		_creditRoll = Scene<GameBase>.instance.uiManager.endingCredit;
		_creditRoll.Show();
		yield return Chronometer.global.WaitForSeconds(_delay);
		((MonoBehaviour)this).StartCoroutine(_creditRoll.CRun(resetGameScene: false));
		while (_creditRoll.active)
		{
			yield return null;
		}
	}

	private void OnDisable()
	{
		if ((Object)(object)_creditRoll != (Object)null)
		{
			_creditRoll.Hide();
		}
	}
}
