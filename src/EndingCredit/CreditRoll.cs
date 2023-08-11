using System.Collections;
using Services;
using Singletons;
using UI.Pause;
using UnityEngine;

namespace EndingCredit;

public class CreditRoll : MonoBehaviour
{
	[PauseEvent.Subcomponent]
	[SerializeField]
	private PauseEvent _pauseEvent;

	[SerializeField]
	private PauseEventSystem _pauseEventSystem;

	[SerializeField]
	private Input _input;

	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _destination;

	[SerializeField]
	private Transform _lastSupporterList;

	[SerializeField]
	private float _delay;

	private bool _resetGameScene;

	public bool active => ((Component)this).gameObject.activeSelf;

	public IEnumerator CRun(bool resetGameScene = true)
	{
		_resetGameScene = resetGameScene;
		_pauseEventSystem.PushEvent(_pauseEvent);
		Vector3 val = ((Component)_destination).transform.position - ((Component)_lastSupporterList).transform.position;
		while (val.y > 0f)
		{
			yield return null;
			((Component)_target).transform.Translate(Vector2.op_Implicit(_input.speed * ((ChronometerBase)Chronometer.global).deltaTime * Vector2.up));
			val = ((Component)_destination).transform.position - ((Component)_lastSupporterList).transform.position;
		}
		yield return Chronometer.global.WaitForSeconds(_delay);
		yield return CLoadScene();
	}

	public IEnumerator CLoadScene()
	{
		yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
		yield return Chronometer.global.WaitForSeconds(2f);
		Hide();
		if (_resetGameScene)
		{
			Singleton<Service>.Instance.ResetGameScene();
		}
	}

	public void Show()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void Hide()
	{
		((Component)this).gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		Singleton<Service>.Instance.fadeInOut.FadeIn();
		Hide();
	}
}
