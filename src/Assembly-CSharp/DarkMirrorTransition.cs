using Scenes;
using UI.Pause;
using UnityEngine;

public class DarkMirrorTransition : MonoBehaviour
{
	[PauseEvent.Subcomponent]
	[SerializeField]
	private PauseEvent _pauseEvent;

	[SerializeField]
	private PauseEventSystem _pauseEventSystem;

	public void PushEmptyPauseEvent()
	{
		if ((Object)(object)_pauseEventSystem == (Object)null)
		{
			_pauseEventSystem = Scene<GameBase>.instance.uiManager.pauseEventSystem;
		}
		_pauseEventSystem.PushEvent(_pauseEvent);
	}

	public void PopPauseEvent()
	{
		if ((Object)(object)_pauseEventSystem == (Object)null)
		{
			_pauseEventSystem = Scene<GameBase>.instance.uiManager.pauseEventSystem;
		}
		_pauseEventSystem.PopEvent();
	}
}
