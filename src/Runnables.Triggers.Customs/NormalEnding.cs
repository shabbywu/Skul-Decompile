using Data;

namespace Runnables.Triggers.Customs;

public class NormalEnding : Trigger
{
	protected override bool Check()
	{
		return GameData.Generic.normalEnding;
	}
}
