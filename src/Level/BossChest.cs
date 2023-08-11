using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class BossChest : InteractiveObject
{
	[Serializable]
	private class BossGears : ReorderableArray<BossGears.Property>
	{
		[Serializable]
		internal class Property
		{
			[SerializeField]
			private float _weight;

			[SerializeField]
			private Gear _gear;

			public float weight => _weight;

			public Gear gear => _gear;
		}
	}

	private const int _randomSeed = 1638136763;

	private const float _delayToDrop = 0.5f;

	private const float _horizontalNoise = 0.5f;

	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private int _count;

	[Range(0f, 100f)]
	[SerializeField]
	[Header("Boss Gears")]
	private int _bossItemDropChance = 10;

	[SerializeField]
	private BossGears _bossGears;

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[Header("Gold")]
	[SerializeField]
	private SoundInfo _goldDropSound;

	[SerializeField]
	private int _goldAmount;

	[SerializeField]
	private int _goldCount;

	[SerializeField]
	[Header("Potion")]
	private PotionPossibilities _potionPossibilities;

	private Rarity _rarity;

	private Rarity _gearRarity;

	private Random _random;

	private Gear[] _rewards;

	private Gear _selected;

	private int _discardCount;

	private bool _alreadyDiscardGear;

	public event Action OnOpen;

	protected override void Awake()
	{
		base.Awake();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 1638136763 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_rewards = new Gear[_count];
	}

	private void EvaluateGearRarity()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		_rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(_random);
		_gearRarity = Settings.instance.containerPossibilities[_rarity].Evaluate(_random);
	}

	public override void InteractWith(Character character)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		((MonoBehaviour)this).StartCoroutine(CDropGold());
		((MonoBehaviour)this).StartCoroutine(CDelayedDrop());
		Deactivate();
		this.OnOpen?.Invoke();
		IEnumerator CDelayedDrop()
		{
			yield return Chronometer.global.WaitForSeconds(1.2f);
			List<DropMovement> dropMovements = new List<DropMovement>();
			for (int itemCount = 0; itemCount < _count; itemCount++)
			{
				Gear gear;
				if (MMMaths.PercentChance(_random, _bossItemDropChance) && !CheckAlreadyDropAllBossItem())
				{
					gear = DropBossGear(dropMovements, itemCount);
				}
				else
				{
					bool flag = MMMaths.PercentChance(_random, 70);
					GearReference gearReference;
					do
					{
						EvaluateGearRarity();
						gearReference = ((!flag) ? ((GearReference)Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(_random, _gearRarity)) : ((GearReference)Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _gearRarity)));
					}
					while (gearReference == null);
					float delay2 = 0.5f;
					delay2 += Time.unscaledTime;
					GearRequest request = gearReference.LoadAsync();
					while (!request.isDone)
					{
						yield return null;
					}
					_ = delay2 - Time.unscaledTime;
					gear = Singleton<Service>.Instance.levelManager.DropGear(request, ((Component)_dropPoint).transform.position);
				}
				gear.dropped.dropMovement.Pause();
				gear.dropped.onLoot += OnLoot;
				gear.onDiscard += OnDiscard;
				dropMovements.Add(gear.dropped.dropMovement);
				_rewards[itemCount] = gear;
				((Component)gear.dropped).gameObject.AddComponent<BossRewardEffect>();
				gear.dropped.additionalPopupUIOffsetY = float.MaxValue;
				gear.dropped.dropMovement.SetMultipleRewardMovement(1f);
			}
			if (character.health.percent < 1.0)
			{
				DropPotion(dropMovements);
			}
			foreach (DropMovement item in dropMovements)
			{
				item.Move();
			}
			DropMovement.SetMultiDropHorizontalInterval(dropMovements);
		}
		IEnumerator CDropGold()
		{
			yield return Chronometer.global.WaitForSeconds(0.7f);
			int count = 80;
			Vector2 val = default(Vector2);
			for (int i = 0; i < count; i++)
			{
				float num = Random.Range(-0.5f, 0.5f);
				((Vector2)(ref val))._002Ector(((Component)_dropPoint).transform.position.x + num, ((Component)_dropPoint).transform.position.y);
				Singleton<Service>.Instance.levelManager.DropGold(_goldAmount / (count / 2), 1, Vector2.op_Implicit(val), Vector2.up * 7f + Vector2.right);
				Singleton<Service>.Instance.levelManager.DropGold(_goldAmount / (count / 2), 1, Vector2.op_Implicit(val), Vector2.up * 7f + Vector2.left);
				PersistentSingleton<SoundManager>.Instance.PlaySound(_goldDropSound, ((Component)_dropPoint).transform.position);
				yield return null;
			}
		}
	}

	private Gear DropBossGear(List<DropMovement> dropMovements, int itemCount)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		Gear gear = null;
		do
		{
			BossGears.Property[] values = ((ReorderableArray<BossGears.Property>)_bossGears).values;
			double num = _random.NextDouble() * (double)values.Sum((BossGears.Property a) => a.weight);
			for (int i = 0; i < values.Length; i++)
			{
				num -= (double)values[i].weight;
				if (num <= 0.0)
				{
					gear = values[i].gear;
					break;
				}
			}
		}
		while (ContainsInRewards(gear));
		return Singleton<Service>.Instance.levelManager.DropGear(gear, ((Component)_dropPoint).transform.position);
	}

	private void DropPotion(List<DropMovement> dropMovements)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Potion potion = _potionPossibilities.Get();
		potion = CommonResource.instance.potions[Potion.Size.Large];
		if ((Object)(object)potion != (Object)null)
		{
			Singleton<Service>.Instance.levelManager.DropPotion(potion, ((Component)_dropPoint).transform.position);
		}
	}

	private bool ContainsInRewards(Gear target)
	{
		if ((Object)(object)target == (Object)null)
		{
			return true;
		}
		Gear[] rewards = _rewards;
		foreach (Gear gear in rewards)
		{
			if ((Object)(object)gear == (Object)null)
			{
				return false;
			}
			if (((Object)gear).name.Equals(((Object)target).name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckAlreadyDropAllBossItem()
	{
		for (int i = 0; i < ((ReorderableArray<BossGears.Property>)_bossGears).values.Length; i++)
		{
			BossGears.Property property = ((ReorderableArray<BossGears.Property>)_bossGears).values[i];
			if (!ContainsInRewards(property.gear))
			{
				return false;
			}
		}
		return true;
	}

	public override void OnActivate()
	{
		base.OnActivate();
		_animator.Play(InteractiveObject._activateHash);
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_animator.Play(InteractiveObject._deactivateHash);
	}

	private void OnDiscard(Gear gear)
	{
		Array.Sort(_rewards, delegate(Gear gear1, Gear gear2)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (gear1.type != gear2.type)
			{
				if (gear1.type == Gear.Type.Item)
				{
					return 1;
				}
				if (gear2.type == Gear.Type.Item)
				{
					return -1;
				}
			}
			return (gear1.rarity >= gear2.rarity) ? 1 : (-1);
		});
		bool flag = false;
		for (int i = 0; i < _count; i++)
		{
			if (!flag)
			{
				_discardCount++;
				flag = true;
			}
			if (_discardCount >= 2)
			{
				_rewards[i].destructible = false;
				_rewards[i].onDiscard -= OnDiscard;
			}
			_rewards[i].dropped.onLoot -= OnLoot;
			if (_rewards[i].state == Gear.State.Dropped && (Object)(object)_rewards[i] != (Object)null)
			{
				((Component)_rewards[i]).gameObject.SetActive(false);
			}
		}
	}

	private void OnLoot(Character character)
	{
		for (int i = 0; i < _count; i++)
		{
			_rewards[i].dropped.onLoot -= OnLoot;
			_rewards[i].onDiscard -= OnDiscard;
			if (_rewards[i].state == Gear.State.Dropped)
			{
				_rewards[i].destructible = false;
				((Component)_rewards[i]).gameObject.SetActive(false);
			}
		}
	}
}
