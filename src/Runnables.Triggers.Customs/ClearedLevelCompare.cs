using Data;
using UnityEngine;

namespace Runnables.Triggers.Customs;

public sealed class ClearedLevelCompare : Trigger
{
	[SerializeField]
	private int _compareLevel;

	[SerializeField]
	private ExtensionMethods.CompareOperation _operation;

	protected override bool Check()
	{
		return GameData.HardmodeProgress.clearedLevel.Compare(_compareLevel, _operation);
	}
}
