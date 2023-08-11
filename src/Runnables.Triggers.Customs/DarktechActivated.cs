using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers.Customs;

public sealed class DarktechActivated : Trigger
{
	[SerializeField]
	private DarktechData _darktech;

	protected override bool Check()
	{
		return Singleton<DarktechManager>.Instance.IsActivated(_darktech);
	}
}
