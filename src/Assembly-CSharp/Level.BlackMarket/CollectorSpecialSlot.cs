using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Data;
using FX;
using Hardmode.Darktech;
using Platforms;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level.BlackMarket;

public sealed class CollectorSpecialSlot : MonoBehaviour
{
	private const int _randomSeed = 716722307;

	[SerializeField]
	private CollectorReroll _reroll;

	[SerializeField]
	private Transform _itemPosition;

	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private SoundInfo _buySound;

	private Random _random;

	private DroppedPurchasableReward _dropped;

	private List<DarktechSetting.ItemRotationEquipmentInfo> _remainList;

	private HashSet<DroppedPurchasableReward> _cycle;

	private List<(DroppedPurchasableReward, float)> _itemWeights;

	private DarktechManager _darkTechManager;

	private float _priceMultiplierByStage;

	public Vector3 itemPosition => _itemPosition.position;

	public DroppedPurchasableReward dropped
	{
		get
		{
			return _dropped;
		}
		set
		{
			_dropped = value;
			_text.text = _dropped.price.ToString();
		}
	}

	private void Awake()
	{
		_reroll.onInteracted += DisplayItem;
	}

	private void OnEnable()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 716722307 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		_darkTechManager = Singleton<DarktechManager>.Instance;
		_remainList = _darkTechManager.setting.품목순화장치가중치.ToList();
		_cycle = new HashSet<DroppedPurchasableReward>();
		_itemWeights = new List<(DroppedPurchasableReward, float)>();
		_priceMultiplierByStage = _darkTechManager.setting.품목순환장치상품별가격Dict[(currentChapter.type, currentChapter.stageIndex)];
		DisplayItem();
	}

	private void DisplayItem()
	{
		if (_remainList.Count != 0)
		{
			DroppedPurchasableReward item = TakeOne();
			DropItem(item);
		}
	}

	private DroppedPurchasableReward TakeOne()
	{
		if (_cycle.Count == _remainList.Count)
		{
			_cycle.Clear();
		}
		_itemWeights.Clear();
		for (int i = 0; i < _remainList.Count; i++)
		{
			if (!_cycle.Contains(_remainList[i].item))
			{
				_itemWeights.Add((_remainList[i].item, _remainList[i].weight));
			}
		}
		WeightedRandomizer<DroppedPurchasableReward> weightedRandomizer = new WeightedRandomizer<DroppedPurchasableReward>(_itemWeights);
		DroppedPurchasableReward droppedPurchasableReward;
		if (_itemWeights.Count > 1)
		{
			do
			{
				droppedPurchasableReward = weightedRandomizer.TakeOne(_random);
			}
			while ((Object)(object)dropped != (Object)null && ((Object)droppedPurchasableReward).name.Equals(((Object)dropped).name, StringComparison.OrdinalIgnoreCase));
		}
		else
		{
			droppedPurchasableReward = weightedRandomizer.TakeOne(_random);
		}
		_cycle.Add(droppedPurchasableReward);
		return droppedPurchasableReward;
	}

	private void DropItem(DroppedPurchasableReward item)
	{
		_ = Singleton<Service>.Instance.levelManager.currentChapter;
		_ = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
		_ = Settings.instance.marketSettings;
		DroppedPurchasableReward droppedPurchasableReward = Object.Instantiate<DroppedPurchasableReward>(item, _itemPosition);
		((Object)droppedPurchasableReward).name = ((Object)item).name;
		int price = 1;
		for (int i = 0; i < _darkTechManager.setting.품목순화장치가중치.Length; i++)
		{
			DarktechSetting.ItemRotationEquipmentInfo itemRotationEquipmentInfo = _darkTechManager.setting.품목순화장치가중치[i];
			if (((Object)itemRotationEquipmentInfo.item).name.Equals(((Object)droppedPurchasableReward).name, StringComparison.OrdinalIgnoreCase))
			{
				price = (int)((float)itemRotationEquipmentInfo.basePrice * _priceMultiplierByStage);
			}
		}
		droppedPurchasableReward.price = price;
		droppedPurchasableReward.onLoot += OnLoot;
		if ((Object)(object)dropped != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)dropped).gameObject);
		}
		dropped = droppedPurchasableReward;
		void OnLoot(Character character)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			ExtensionMethods.Set((Type)69);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_buySound, ((Component)this).transform.position);
			dropped.onLoot -= OnLoot;
			for (int num = _remainList.Count - 1; num >= 0; num--)
			{
				if (((Object)dropped).name.Equals(((Object)_remainList[num].item).name, StringComparison.OrdinalIgnoreCase))
				{
					if (_cycle.Contains(_remainList[num].item))
					{
						_cycle.Remove(_remainList[num].item);
					}
					_remainList.RemoveAt(num);
					break;
				}
			}
		}
	}

	private void Update()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_dropped == (Object)null || !((Component)_dropped).gameObject.activeInHierarchy)
		{
			_text.text = "---";
			((Graphic)_text).color = Color.white;
		}
		else if (_dropped.price > 0)
		{
			((Graphic)_text).color = (GameData.Currency.gold.Has(_dropped.price) ? Color.white : Color.red);
		}
		else
		{
			_text.text = "---";
			((Graphic)_text).color = Color.white;
		}
	}
}
