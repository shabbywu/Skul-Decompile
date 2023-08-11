using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events;

public class SetFadeColor : Event
{
	[SerializeField]
	private Color _color;

	public override void Run()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Singleton<Service>.Instance.fadeInOut.SetFadeColor(_color);
	}
}
