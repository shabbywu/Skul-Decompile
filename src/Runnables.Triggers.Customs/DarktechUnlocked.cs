using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers.Customs;

public sealed class DarktechUnlocked : Trigger
{
	[SerializeField]
	private DarktechData _darktech;

	protected override bool Check()
	{
		return Singleton<DarktechManager>.Instance.IsUnlocked(_darktech);
	}
}
