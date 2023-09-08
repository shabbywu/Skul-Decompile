using UnityEngine;

namespace CutScenes.Shots;

public sealed class EventInfos : Shot
{
	[Event.Subcomponent]
	[SerializeField]
	private Event.Subcomponents _events;

	private Shot _next;

	public override void Run()
	{
		_events.Run();
		if ((Object)(object)_next != (Object)null)
		{
			_next.Run();
		}
	}

	public override void SetNext(Shot next)
	{
		_next = next;
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
