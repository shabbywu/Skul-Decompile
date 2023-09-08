using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Gear.Upgrades;

[Serializable]
public class UpgradeInfo
{
	[Serializable]
	private class UpgradeByLevel
	{
		[SerializeField]
		internal UpgradeObject upgradeObject;
	}

	[SerializeField]
	private List<UpgradeObject> _upgradeByLevels;

	public string name => upgradeObject.displayName;

	public int maxLevel => upgradeObject.maxLevel;

	public UpgradeObject upgradeObject => upgradeObject;
}
