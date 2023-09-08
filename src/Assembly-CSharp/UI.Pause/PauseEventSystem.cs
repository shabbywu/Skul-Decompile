using System.Collections.Generic;
using UnityEngine;

namespace UI.Pause;

public class PauseEventSystem : MonoBehaviour
{
	[PauseEvent.Subcomponent]
	[SerializeField]
	private PauseEvent _baseEvent;

	private PauseEvent _empty;

	private Stack<PauseEvent> _events;

	private void Awake()
	{
		_events = new Stack<PauseEvent>();
		_empty = ((Component)this).gameObject.AddComponent<Empty>();
		_events.Push(_baseEvent);
	}

	public void Run()
	{
		if (_events.Count == 0)
		{
			Debug.LogError((object)"Panel이 없습니다.");
		}
		else
		{
			_events.Peek().Invoke();
		}
	}

	public void PushEvent(PauseEvent type)
	{
		_events.Push(type);
	}

	public void PopEvent()
	{
		_events.Pop();
	}

	public void PushEmpty()
	{
		PushEvent(_empty);
	}
}
