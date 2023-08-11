using Data;
using Platforms;
using Singletons;

namespace CutScenes.Shots.Events;

public class SaveTutorialData : Event
{
	public override void Run()
	{
		GameData.Generic.tutorial.End();
		GameData.Generic.tutorial.Save();
		PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
	}
}
