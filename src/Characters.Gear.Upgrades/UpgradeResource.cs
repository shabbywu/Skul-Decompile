using System;
using System.Collections.Generic;
using System.Text;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Gear.Upgrades;

public class UpgradeResource : ScriptableObject
{
	[Serializable]
	public class Reference : IEquatable<UpgradeObject>, IComparable<Reference>
	{
		public static string prefix = "Upgrade";

		public AssetReference assetReference;

		public string name;

		public string guid;

		public Sprite icon;

		public Sprite thumbnail;

		public int[] prices;

		public int maxLevel;

		public UpgradeObject.Type type;

		public bool needUnlock;

		public bool removable;

		public UpgradeObject.ValueForDescription[] valuesForDescription;

		public int orderInShop;

		private string _key => name;

		private string _keyBase => prefix + "/" + _key;

		public string displayNameKey => _keyBase + "/name";

		public string displayName => Localization.GetLocalizedString(_keyBase + "/name");

		public string flavor => Localization.GetLocalizedString(_keyBase + "/flavor");

		public string description => Localization.GetLocalizedString(_keyBase + "/desc");

		public int price
		{
			get
			{
				if (prices != null && prices.Length != 0)
				{
					return prices[Mathf.Min(maxLevel - 1, GetCurrentLevel())];
				}
				return 0;
			}
		}

		public static string curseText => Localization.GetLocalizedString("label/upgrade/type/cursed");

		public bool Equals(UpgradeObject other)
		{
			return other.reference.Equals(this);
		}

		public UpgradeObject Instantiate()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			AsyncOperationHandle<GameObject> handle = assetReference.LoadAssetAsync<GameObject>();
			GameObject obj = Object.Instantiate<GameObject>(handle.WaitForCompletion());
			ReleaseAddressableHandleOnDestroy.Reserve(obj, handle);
			return obj.GetComponent<UpgradeObject>();
		}

		public int GetCurrentLevel()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if ((Object)(object)player == (Object)null)
			{
				return 0;
			}
			UpgradeInventory upgrade = player.playerComponents.inventory.upgrade;
			int num = upgrade.IndexOf(this);
			if (num == -1)
			{
				return 0;
			}
			return upgrade.upgrades[num].level;
		}

		public int GetNextLevel()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if ((Object)(object)player == (Object)null)
			{
				return 0;
			}
			UpgradeInventory upgrade = player.playerComponents.inventory.upgrade;
			int num = upgrade.IndexOf(this);
			if (num == -1)
			{
				return 1;
			}
			return Mathf.Min(upgrade.upgrades[num].level + 1, upgrade.upgrades[num].maxLevel);
		}

		public string GetDescription(int targetLevel, string activateColor = "F158FF", string deactivateColor = "3C285F")
		{
			if (valuesForDescription.Length != 0)
			{
				if (valuesForDescription[0].values.Length == 0)
				{
					return description;
				}
				return valuesForDescription[0].argsCount switch
				{
					1 => string.Format(description, GetDescriptionArgs(targetLevel, 0, activateColor, deactivateColor)), 
					2 => string.Format(description, GetDescriptionArgs(targetLevel, 0, activateColor, deactivateColor), GetDescriptionArgs(targetLevel, 1, activateColor, deactivateColor)), 
					_ => description, 
				};
			}
			return description;
		}

		public string GetCurrentDescription(string activateColor, string deactivateColor)
		{
			return GetDescription(GetCurrentLevel(), activateColor, deactivateColor);
		}

		public string GetNextLevelDescription(string activateColor = "F158FF", string deactivateColor = "3C285F")
		{
			return GetDescription(GetNextLevel(), activateColor, deactivateColor);
		}

		private string GetDescriptionArgs(int targetLevel, int argIndex, string activateColor, string deactivateColor)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<color=#" + deactivateColor + ">");
			for (int i = 1; i <= valuesForDescription.Length; i++)
			{
				if (targetLevel == i)
				{
					stringBuilder.Append("<color=#" + activateColor + ">");
				}
				stringBuilder.Append(valuesForDescription[i - 1][argIndex]);
				if (targetLevel == i)
				{
					stringBuilder.Append("</color>");
				}
				if (i < valuesForDescription.Length)
				{
					stringBuilder.Append('/');
				}
			}
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}

		public int CompareTo(Reference other)
		{
			if (maxLevel > other.maxLevel)
			{
				return 1;
			}
			if (maxLevel < other.maxLevel)
			{
				return -1;
			}
			return 0;
		}
	}

	private const string harmodeSettingKey = "HardmodeSetting";

	[SerializeField]
	private List<Reference> _upgradeReferences;

	[Space]
	[SerializeField]
	private Sprite[] _upgradeIcons;

	private Dictionary<string, Sprite> _upgradeIconDictionary;

	[SerializeField]
	private Sprite[] _upgradeThumbnails;

	private Dictionary<string, Sprite> _upgradeThumbnailDictionary;

	private static UpgradeResource _instance;

	public Dictionary<string, Sprite> upgradeIconDictionary => _upgradeIconDictionary;

	public Dictionary<string, Sprite> upgradeThumbnailDictionary => _upgradeThumbnailDictionary;

	public List<Reference> upgradeReferences => _upgradeReferences;

	public static UpgradeResource instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<UpgradeResource>("HardmodeSetting/UpgradeResource");
				_instance.Initialize();
			}
			return _instance;
		}
	}

	public Sprite GetIcon(string name)
	{
		_upgradeIconDictionary.TryGetValue(name, out var value);
		return value;
	}

	public Sprite GetThumbnail(string name)
	{
		_upgradeThumbnailDictionary.TryGetValue(name, out var value);
		return value;
	}

	public void Initialize()
	{
	}
}
