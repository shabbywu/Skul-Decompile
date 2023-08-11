using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear.Items;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public sealed class AdventurerGearReward : InteractiveObject
{
	private sealed class ExtraSeed : MonoBehaviour
	{
		public short value;
	}

	[Serializable]
	public class ItemWeight : IComparer<ItemWeight>
	{
		public Item gear;

		[Range(0f, 100f)]
		public int weight;

		public bool onlyHardmode;

		public int Compare(ItemWeight x, ItemWeight y)
		{
			if (x.weight > y.weight)
			{
				return 1;
			}
			if (x.weight < y.weight)
			{
				return -1;
			}
			return 0;
		}
	}

	[Serializable]
	public class PotionWeight : IComparer<PotionWeight>
	{
		public PurchasablePotion potion;

		[Range(0f, 100f)]
		public int weight;

		public int Compare(PotionWeight x, PotionWeight y)
		{
			if (x.weight > y.weight)
			{
				return 1;
			}
			if (x.weight < y.weight)
			{
				return -1;
			}
			return 0;
		}
	}

	[SerializeField]
	[Range(0f, 100f)]
	private int _dropChance;

	[SerializeField]
	private PotionWeight[] _potionWeights;

	[SerializeField]
	private ItemWeight[] _gearWeights;

	[SerializeField]
	private SpriteRenderer _body;

	[SerializeField]
	private float _power = 0.2f;

	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private float _interval = 0.1f;

	private readonly int count = 3;

	private readonly int _randomSeed = 1177618293;

	private int _remainCount;

	private bool _dropDone;

	private Random _random;

	private void Start()
	{
		ExtraSeed extraSeed = default(ExtraSeed);
		if (!((Component)Map.Instance).TryGetComponent<ExtraSeed>(ref extraSeed))
		{
			extraSeed = ((Component)Map.Instance).gameObject.AddComponent<ExtraSeed>();
		}
		extraSeed.value++;
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + _randomSeed + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex + extraSeed.value);
		_remainCount = count;
		((MonoBehaviour)this).StartCoroutine(CWaitForClear());
		if (!GameData.HardmodeProgress.hardmode)
		{
			IEnumerable<ItemWeight> source = _gearWeights.Where((ItemWeight weight) => !weight.onlyHardmode);
			_gearWeights = source.ToArray();
		}
	}

	private IEnumerator CWaitForClear()
	{
		while (Map.Instance.waveContainer.state != 0)
		{
			yield return null;
		}
		Activate();
	}

	public override void InteractWith(Character character)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StartCoroutine(CShake());
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		Vector2Int goldrewardAmount = levelManager.currentChapter.currentStage.goldrewardAmount;
		int x = ((Vector2Int)(ref goldrewardAmount)).x;
		goldrewardAmount = levelManager.currentChapter.currentStage.goldrewardAmount;
		float num = (float)Random.Range(x, ((Vector2Int)(ref goldrewardAmount)).y) * levelManager.currentChapter.adventurerGoldRewardMultiplier;
		levelManager.DropGold((int)num, 5, ((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		PurchasablePotion purchasablePotion = TakeOne(_potionWeights);
		if ((Object)(object)purchasablePotion != (Object)null)
		{
			PurchasablePotion purchasablePotion2 = Object.Instantiate<PurchasablePotion>(purchasablePotion, ((Component)this).transform.position + Vector3.up, Quaternion.identity);
			((Object)purchasablePotion2).name = ((Object)purchasablePotion).name;
			((Component)purchasablePotion2).transform.parent = ((Component)Map.Instance).transform;
			purchasablePotion2.Initialize();
			purchasablePotion2.dropMovement.Move(MMMaths.RandomBool() ? Random.Range(0.5f, 2.5f) : Random.Range(-2.5f, 0.5f), Random.Range(12, 20));
		}
		if (!_dropDone)
		{
			Item item = TryToDropItem();
			if ((Object)(object)item != (Object)null)
			{
				item.dropped.dropMovement.Move(MMMaths.RandomBool() ? Random.Range(0.5f, 2.5f) : Random.Range(-2.5f, 0.5f), Random.Range(12, 20));
				_dropDone = true;
			}
		}
		_remainCount--;
		if (_remainCount <= 0)
		{
			Deactivate();
		}
	}

	private Item TryToDropItem()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (MMMaths.PercentChance(_random, _dropChance))
		{
			Item item = TakeOne(_gearWeights);
			if ((Object)(object)item != (Object)null && !Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item.HasGroup(item))
			{
				return Singleton<Service>.Instance.levelManager.DropItem(item, ((Component)this).transform.position + Vector3.up);
			}
		}
		return null;
	}

	public Item TakeOne(ItemWeight[] gearWeights)
	{
		int num = 0;
		foreach (ItemWeight itemWeight in gearWeights)
		{
			num += itemWeight.weight;
		}
		int num2 = _random.Next(0, num) + 1;
		for (int j = 0; j < gearWeights.Length; j++)
		{
			num2 -= gearWeights[j].weight;
			if (num2 <= 0)
			{
				return gearWeights[j].gear;
			}
		}
		return null;
	}

	public PurchasablePotion TakeOne(PotionWeight[] potionWeights)
	{
		int num = 0;
		foreach (PotionWeight potionWeight in potionWeights)
		{
			num += potionWeight.weight;
		}
		int num2 = _random.Next(0, num) + 1;
		for (int j = 0; j < potionWeights.Length; j++)
		{
			num2 -= potionWeights[j].weight;
			if (num2 <= 0)
			{
				return potionWeights[j].potion;
			}
		}
		return null;
	}

	private IEnumerator CShake()
	{
		float elapsed = 0f;
		float intervalElapsed = 0f;
		Vector3 shakeVector = Vector3.zero;
		Vector3 originPosition = ((Component)_body).transform.localPosition;
		while (elapsed <= _curve.duration)
		{
			float deltaTime = ((ChronometerBase)Chronometer.global).deltaTime;
			elapsed += deltaTime;
			intervalElapsed -= deltaTime;
			shakeVector -= 60f * deltaTime * shakeVector;
			if (intervalElapsed <= 0f)
			{
				intervalElapsed = _interval;
				float num = 1f - _curve.Evaluate(elapsed);
				shakeVector.x = Random.Range(0f - _power, _power) * num;
				shakeVector.y = Random.Range(0f - _power, _power) * num;
			}
			((Component)_body).transform.localPosition = originPosition + shakeVector;
			yield return null;
		}
		((Component)_body).transform.localPosition = originPosition;
	}
}
