using Data;
using UnityEngine;

namespace Runnables.Triggers.Customs;

public sealed class ClearedLevelCompare : Trigger
{
	[SerializeField]
	private int _compareLevel;

	[SerializeField]
	private CompareOperation _operation;

	protected override bool Check()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return ExtensionMethods.Compare(GameData.HardmodeProgress.clearedLevel, _compareLevel, _operation);
	}
}
