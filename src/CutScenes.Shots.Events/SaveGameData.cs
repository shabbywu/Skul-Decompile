using Data;
using Platforms;
using Singletons;

namespace CutScenes.Shots.Events;

public class SaveGameData : Event
{
	public override void Run()
	{
		GameData.Currency.SaveAll();
		GameData.Progress.SaveAll();
		PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
	}
}
