using Singletons;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class DarktechReward : MonoBehaviour
{
	[SerializeField]
	private DarktechData.Type _type;

	private void Start()
	{
		Singleton<DarktechManager>.Instance.UnlockDarktech(_type);
	}
}
