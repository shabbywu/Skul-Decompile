using Characters.Gear.Upgrades;
using UnityEngine;

namespace Level;

public sealed class HardmodeService : MonoBehaviour
{
	[SerializeField]
	private UpgradeShop _upgradeShop;

	private void Awake()
	{
		((Component)this).transform.parent = null;
	}
}
