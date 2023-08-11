using Data;

namespace Runnables;

public sealed class ActivateEnding : Runnable
{
	public override void Run()
	{
		GameData.Generic.normalEnding = true;
		GameData.Generic.SaveAll();
	}
}
