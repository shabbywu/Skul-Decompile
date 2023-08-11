using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Gear;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class Chest : InteractiveObject, ILootable
{
	private const int rewardCount = 3;

	private const int _randomSeed = 2028506624;

	private const float _delayToDrop = 0.5f;

	[SerializeField]
	[GetComponent]
	private Animator _animator;

	[SerializeField]
	private RuntimeAnimatorController _commonChest;

	[SerializeField]
	private RuntimeAnimatorController _rareChest;

	[SerializeField]
	private RuntimeAnimatorController _uniqueChest;

	[SerializeField]
	private RuntimeAnimatorController _legendaryChest;

	private Gear[] _rewards;

	private List<GearRequest> _toDropRequests;

	private List<GearReference> _toDropReferences;

	private Rarity _rarity;

	private Rarity _gearRarity;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	private int _discardCount;

	private Random _random;

	public bool looted { get; private set; }

	public event Action onLoot;

	protected override void Awake()
	{
		base.Awake();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	private void Start()
	{
		Initialize();
		Load();
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged += Load;
	}

	private void ReleaseRequests()
	{
		if (_toDropRequests == null)
		{
			return;
		}
		foreach (GearRequest toDropRequest in _toDropRequests)
		{
			toDropRequest?.Release();
		}
	}

	private void Load()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		_toDropReferences.Clear();
		ReleaseRequests();
		_toDropRequests.Clear();
		int num = 50;
		int num2 = 0;
		for (int i = 0; i < 3; i++)
		{
			GearReference gearReference;
			do
			{
				num2++;
				EvaluateGearRarity();
				gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _gearRarity);
				if (gearReference == null)
				{
					continue;
				}
				foreach (GearReference toDropReference in _toDropReferences)
				{
					if (toDropReference.name.Equals(gearReference.name))
					{
						gearReference = null;
						break;
					}
				}
				if (num2 >= num)
				{
					Debug.LogError((object)"######## 이 에러가 나오면 개발팀에 알려주세요.");
				}
			}
			while (gearReference == null && num2 < num);
			_toDropReferences.Add(gearReference);
			_toDropRequests.Add(gearReference.LoadAsync());
		}
	}

	private void Initialize()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected I4, but got Unknown
		_rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(_random);
		Rarity rarity = _rarity;
		switch ((int)rarity)
		{
		case 0:
			_animator.runtimeAnimatorController = _commonChest;
			break;
		case 1:
			_animator.runtimeAnimatorController = _rareChest;
			break;
		case 2:
			_animator.runtimeAnimatorController = _uniqueChest;
			break;
		case 3:
			_animator.runtimeAnimatorController = _legendaryChest;
			break;
		}
		_toDropRequests = new List<GearRequest>(3);
		_toDropReferences = new List<GearReference>(3);
		_rewards = new Gear[3];
	}

	private void EvaluateGearRarity()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		_gearRarity = Settings.instance.containerPossibilities[_rarity].Evaluate(_random);
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

	public override void InteractWith(Character character)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= Load;
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		((MonoBehaviour)this).StartCoroutine(CDelayedDrop());
		Deactivate();
		IEnumerator CDelayedDrop()
		{
			List<DropMovement> dropMovements = new List<DropMovement>();
			float elapsed = 0f;
			for (int i = 0; i < 3; i++)
			{
				GearRequest request = _toDropRequests[i];
				while (!request.isDone)
				{
					elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
					yield return null;
				}
			}
			while (elapsed <= 0.5f)
			{
				elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
				yield return null;
			}
			for (int j = 0; j < 3; j++)
			{
				GearRequest gearRequest = _toDropRequests[j];
				Gear gear = Singleton<Service>.Instance.levelManager.DropGear(gearRequest, ((Component)this).transform.position);
				gear.dropped.dropMovement.Pause();
				gear.dropped.onLoot += OnLoot;
				gear.onDiscard += OnDiscard;
				dropMovements.Add(gear.dropped.dropMovement);
				_rewards[j] = gear;
				((Component)gear.dropped).gameObject.AddComponent<BossRewardEffect>();
				gear.dropped.additionalPopupUIOffsetY = float.MaxValue;
				gear.dropped.dropMovement.SetMultipleRewardMovement(1f);
			}
			foreach (DropMovement item in dropMovements)
			{
				item.Move();
			}
			DropMovement.SetMultiDropHorizontalInterval(dropMovements);
			this.onLoot?.Invoke();
			looted = true;
		}
	}

	private void OnDiscard(Gear gear)
	{
		Array.Sort(_rewards, (Gear gear1, Gear gear2) => (gear1.rarity >= gear2.rarity) ? 1 : (-1));
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			if (!flag && _rewards[i].type != Gear.Type.Quintessence)
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
			if ((Object)(object)((Component)_rewards[i]).gameObject != (Object)null && _rewards[i].state == Gear.State.Dropped)
			{
				((Component)_rewards[i]).gameObject.SetActive(false);
			}
		}
	}

	private void OnLoot(Character character)
	{
		for (int i = 0; i < 3; i++)
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

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= Load;
			ReleaseRequests();
		}
	}
}
