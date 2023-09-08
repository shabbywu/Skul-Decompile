using System;
using Characters.Gear.Upgrades;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Data.Hardmode.Upgrades;

[CreateAssetMenu(fileName = "UpgradeShopSettings", menuName = "ScriptableObjects/UpgradeShopSettings")]
public sealed class UpgradeShopSettings : ScriptableObject
{
	[Serializable]
	private class CostByChapter
	{
		[SerializeField]
		private Chapter.Type _chapter;

		[SerializeField]
		private int _cost;

		public bool TryTakeCost(Chapter.Type type, ref int cost)
		{
			if (type != _chapter)
			{
				return false;
			}
			cost = _cost;
			return true;
		}
	}

	private const string harmode = "HardmodeSetting";

	[SerializeField]
	private CostByChapter[] _removeCosts;

	[SerializeField]
	private CostByChapter[] _removeRiskyCosts;

	private static UpgradeShopSettings _instance;

	public static UpgradeShopSettings instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<UpgradeShopSettings>("HardmodeSetting/UpgradeShopSettings");
			}
			return _instance;
		}
	}

	public int GetRemoveCost(UpgradeObject.Type type)
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		int cost = 0;
		if (type == UpgradeObject.Type.Cursed)
		{
			CostByChapter[] removeRiskyCosts = _removeRiskyCosts;
			for (int i = 0; i < removeRiskyCosts.Length; i++)
			{
				if (removeRiskyCosts[i].TryTakeCost(currentChapter.type, ref cost))
				{
					return cost;
				}
			}
		}
		else
		{
			CostByChapter[] removeRiskyCosts = _removeCosts;
			for (int i = 0; i < removeRiskyCosts.Length; i++)
			{
				if (removeRiskyCosts[i].TryTakeCost(currentChapter.type, ref cost))
				{
					return cost;
				}
			}
		}
		return cost;
	}
}
