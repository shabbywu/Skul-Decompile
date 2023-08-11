using System;
using Characters.Player;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class UpgradeObject : MonoBehaviour, IComparable<UpgradeObject>
{
	[Serializable]
	public class ValueForDescription
	{
		public string[] values;

		public int argsCount => values.Length;

		public string this[int index] => values[index];
	}

	public enum Type
	{
		Normal,
		Cursed
	}

	[SerializeField]
	private int _orderInShop;

	[SerializeField]
	private Upgrade _upgrade;

	[Header("Description용 변수")]
	[SerializeField]
	private ValueForDescription[] _valuesByLevel;

	[Tooltip("제거 가능한가")]
	[SerializeField]
	private bool _removable = true;

	[SerializeField]
	private bool _needUnlock;

	[SerializeField]
	private int[] _prices;

	[SerializeField]
	private Type _type;

	[SerializeField]
	private UpgradeResource.Reference _reference;

	private int _level;

	public static string prefix = "Upgrade";

	public int orderInShop => _orderInShop;

	public string displayNameKey => _reference.displayNameKey;

	public string displayName => _reference.displayName;

	public string flavor => _reference.flavor;

	public string description => _reference.description;

	public bool needUnlock => _needUnlock;

	public int level => _level;

	public int price => _prices[_level];

	public int[] prices => _prices;

	public Sprite icon => _reference.icon;

	public Sprite thumbnail => _reference.thumbnail;

	public Type type => _type;

	public ValueForDescription[] valuesByLevel => _valuesByLevel;

	public bool removable => _removable;

	public int maxLevel => _upgrade.maxLevel;

	public int returnCost
	{
		get
		{
			int num = 0;
			for (int i = 0; i < _level; i++)
			{
				num += _prices[i];
			}
			return num;
		}
	}

	public string dataKey => displayNameKey;

	public UpgradeResource.Reference reference
	{
		get
		{
			return _reference;
		}
		set
		{
			_reference = value;
		}
	}

	public UpgradeObject GetOrigin()
	{
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if ((Object)(object)levelManager == (Object)null)
		{
			return this;
		}
		Character player = levelManager.player;
		if ((Object)(object)player == (Object)null)
		{
			return this;
		}
		UpgradeInventory upgrade = player.playerComponents.inventory.upgrade;
		int num = upgrade.IndexOf(this);
		if (num == -1)
		{
			return this;
		}
		return upgrade.upgrades[num];
	}

	public UpgradeAbility GetCurrentAbility()
	{
		return _upgrade.GetAbility(_level);
	}

	public int GetCurrentLevel()
	{
		return _reference.GetCurrentLevel();
	}

	public UpgradeObject LevelUpOrigin()
	{
		UpgradeObject origin = GetOrigin();
		origin.LevelUp();
		return origin;
	}

	public void SetLevel(int level)
	{
		_level = level;
		_upgrade.LevelUp(_level);
	}

	private void LevelUp()
	{
		_level = Mathf.Min(level + 1, maxLevel);
		_upgrade.LevelUp(_level);
	}

	public void Attach(Character target)
	{
		_level = 1;
		_upgrade.Attach(target);
	}

	public void Detach()
	{
		_upgrade.Detach();
	}

	private void OnDestroy()
	{
		Detach();
	}

	public int CompareTo(UpgradeObject other)
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
