using Data;
using UnityEngine;

namespace Runnables.Triggers.Customs;

public sealed class Mode : Trigger
{
	private enum Level
	{
		Normal,
		Hard
	}

	[SerializeField]
	private Level _level;

	protected override bool Check()
	{
		if (_level == Level.Hard && GameData.HardmodeProgress.hardmode)
		{
			return true;
		}
		if (_level == Level.Normal && !GameData.HardmodeProgress.hardmode)
		{
			return true;
		}
		return false;
	}
}
