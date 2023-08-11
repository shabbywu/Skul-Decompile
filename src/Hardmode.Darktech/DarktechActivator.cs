using Data;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class DarktechActivator : MonoBehaviour
{
	[SerializeField]
	private DarktechData _info;

	[SerializeField]
	private GameObject _target;

	private void Awake()
	{
		if (!GameData.HardmodeProgress.hardmode)
		{
			_target.SetActive(false);
		}
		else if (Singleton<DarktechManager>.Instance.IsUnlocked(_info))
		{
			_target.SetActive(true);
		}
		else
		{
			_target.SetActive(false);
		}
	}
}
