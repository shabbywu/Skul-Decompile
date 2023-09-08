using Characters.Gear;
using Characters.Gear.Upgrades;
using Data;
using Hardmode.Darktech;
using Platforms;
using UnityEngine;

namespace Hardmode;

public sealed class UnlockUpgrade : MonoBehaviour
{
	[SerializeField]
	private DarktechData.Type _type;

	[SerializeField]
	private UpgradeObject[] _upgradeObjects;

	[SerializeField]
	private bool _unlockAcheivementOnActivate;

	private void OnEnable()
	{
		string typeName = Gear.Type.Upgrade.ToString();
		if (GameData.HardmodeProgress.unlocked.GetData(_type))
		{
			UpgradeObject[] upgradeObjects = _upgradeObjects;
			foreach (UpgradeObject upgradeObject in upgradeObjects)
			{
				GameData.Gear.SetUnlocked(typeName, ((Object)upgradeObject).name, value: true);
			}
			if (_unlockAcheivementOnActivate)
			{
				ExtensionMethods.Set((Type)73);
			}
		}
	}
}
