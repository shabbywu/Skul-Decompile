using Services;
using Singletons;

namespace CutScenes.Shots.Events;

public class ResetGameScene : Event
{
	public override void Run()
	{
		Singleton<Service>.Instance.ResetGameScene();
	}
}
