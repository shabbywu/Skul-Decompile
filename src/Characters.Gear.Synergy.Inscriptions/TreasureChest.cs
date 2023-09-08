using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Characters.Operations;
using Data;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class TreasureChest : InteractiveObject, ILootable
{
	public enum Level
	{
		Set2,
		Set4
	}

	public enum RewardType
	{
		Gold,
		CurrencyBag,
		Items
	}

	[Serializable]
	public class RewardTypeBoolArray : EnumArray<RewardType, bool>
	{
		public RewardTypeBoolArray()
		{
		}

		public RewardTypeBoolArray(params bool[] values)
		{
			int num = Math.Min(base.Array.Length, values.Length);
			for (int i = 0; i < num; i++)
			{
				base.Array[i] = values[i];
			}
		}
	}

	public static readonly ReadOnlyCollection<RewardType> rewardValues = EnumValues<RewardType>.Values;

	private const int _randomSeed = 2028506624;

	private const float _delayToDrop = 0.5f;

	[SerializeField]
	private Level _level;

	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private DropMovement _dropMovement;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onDrop;

	[Header("레전더리 효과")]
	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onLegendary;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	private Random _random;

	private RewardType _rewardType;

	private Rarity _gearRarity;

	public bool looted { get; private set; }

	public event Action onLoot;

	protected override void Awake()
	{
		base.Awake();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_onDrop.Initialize();
		_onLegendary.Initialize();
		if ((Object)(object)_dropMovement == (Object)null)
		{
			Activate();
		}
		else
		{
			_dropMovement.onGround += Activate;
		}
	}

	private void Start()
	{
		if (_level != 0)
		{
			EvaluateGearRarity();
			Load();
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		((MonoBehaviour)this).StartCoroutine(_onDrop.CRun(Singleton<Service>.Instance.levelManager.player));
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_animator.Play(InteractiveObject._deactivateHash);
	}

	public override void InteractWithByPressing(Character character)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		Drop();
		Deactivate();
	}

	private T Evalualte<T>(Random random, int[] possibilities, IList<T> values)
	{
		int maxValue = possibilities.Sum();
		int num = random.Next(0, maxValue) + 1;
		for (int i = 0; i < possibilities.Length; i++)
		{
			num -= possibilities[i];
			if (num <= 0)
			{
				return values[i];
			}
		}
		return values[0];
	}

	private void Load()
	{
		Treasure.StageInfo treasureInfo = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.treasureInfo;
		int[] possibilities = new int[3] { treasureInfo.goldChestWeight, treasureInfo.currencyBagChestWeight, treasureInfo.itemChechWeight };
		_rewardType = Evalualte(_random, possibilities, rewardValues);
		if (_rewardType == RewardType.Items)
		{
			LoadItem();
		}
	}

	private void LoadItem()
	{
		_itemRequest?.Release();
		do
		{
			EvaluateGearRarity();
			_itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _gearRarity);
		}
		while (_itemToDrop == null);
		_itemRequest = _itemToDrop.LoadAsync();
	}

	private void EvaluateGearRarity()
	{
		_gearRarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.treasureInfo.itemRarityPossibilities.Evaluate(_random);
	}

	private void Drop()
	{
		IStageInfo currentStage = Singleton<Service>.Instance.levelManager.currentChapter.currentStage;
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if (_level == Level.Set2)
		{
			((MonoBehaviour)this).StartCoroutine(CDelayedGoldDrop());
			return;
		}
		Rarity rarity;
		GameData.Currency.Type type;
		int amountInBag;
		switch (_rewardType)
		{
		case RewardType.Gold:
			((MonoBehaviour)this).StartCoroutine(CDelayedGoldDrop());
			break;
		case RewardType.CurrencyBag:
			rarity = currentStage.treasureInfo.currencyRarityPossibilities.Evaluate(_random);
			type = GameData.Currency.Type.Bone;
			amountInBag = currentStage.treasureInfo.boneRangeByRarity.Evaluate(rarity);
			((MonoBehaviour)this).StartCoroutine(CDelayedCurrencyBagDrop());
			break;
		case RewardType.Items:
			Load();
			((MonoBehaviour)this).StartCoroutine(CDelayedDrop());
			if (_gearRarity == Rarity.Legendary)
			{
				((MonoBehaviour)this).StartCoroutine(_onLegendary.CRun(Singleton<Service>.Instance.levelManager.player));
			}
			break;
		}
		IEnumerator CDelayedCurrencyBagDrop()
		{
			yield return Chronometer.global.WaitForSeconds(0.5f);
			if (rarity == Rarity.Legendary)
			{
				((MonoBehaviour)this).StartCoroutine(_onLegendary.CRun(Singleton<Service>.Instance.levelManager.player));
			}
			Singleton<Service>.Instance.levelManager.DropCurrencyBag(type, rarity, amountInBag, Mathf.Min(amountInBag * 2, 15), _dropPoint.position);
		}
		IEnumerator CDelayedDrop()
		{
			float delay = 0.5f;
			delay += Time.unscaledTime;
			while (!_itemRequest.isDone)
			{
				yield return null;
			}
			delay -= Time.unscaledTime;
			if (delay > 0f)
			{
				yield return Chronometer.global.WaitForSeconds(delay);
			}
			Singleton<Service>.Instance.levelManager.DropItem(_itemRequest, _dropPoint.position);
			this.onLoot?.Invoke();
			looted = true;
		}
		IEnumerator CDelayedGoldDrop()
		{
			yield return Chronometer.global.WaitForSeconds(0.5f);
			float value2 = currentStage.treasureInfo.goldAmount2Set.value;
			levelManager.DropGold((int)value2, Random.Range(30, 40), _dropPoint.position);
		}
		IEnumerator CDelayedGoldDrop()
		{
			yield return Chronometer.global.WaitForSeconds(0.5f);
			float value = currentStage.treasureInfo.goldAmount4Set.value;
			levelManager.DropGold((int)value, Random.Range(65, 75), _dropPoint.position);
		}
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= Load;
			_itemRequest?.Release();
		}
	}
}
