using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Gear;
using Characters.Operations.Fx;
using Data;
using FX;
using GameResources;
using Hardmode.Darktech;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level;

public class HardmodeChest : InteractiveObject, ILootable
{
	private const int _randomSeed = 2028506624;

	private const float _delayToDrop = 0.5f;

	[Header("흉조 아이템")]
	[SerializeField]
	private int _normalChestItemCount;

	[SerializeField]
	private int _omenChestItemCount;

	[SerializeField]
	private RuntimeAnimatorController _commonOmenChest;

	[SerializeField]
	private RuntimeAnimatorController _rareOmenChest;

	[SerializeField]
	private RuntimeAnimatorController _uniqueOmenChest;

	[SerializeField]
	private RuntimeAnimatorController _legendaryOmenChest;

	[SerializeField]
	private Transform _commonCrawPosition;

	[SerializeField]
	private Transform _rareCrawPosition;

	[SerializeField]
	private Transform _uniqueCrawPosition;

	[SerializeField]
	private Transform _legendaryCrawPosition;

	[SerializeField]
	private GameObject _omenObejct;

	[SerializeField]
	private GameObject _cat;

	[SerializeField]
	private ReactiveProp _craw;

	[Header("기본 설정")]
	[SerializeField]
	private Transform _dropPoint;

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private RuntimeAnimatorController _commonChest;

	[SerializeField]
	private RuntimeAnimatorController _rareChest;

	[SerializeField]
	private RuntimeAnimatorController _uniqueChest;

	[SerializeField]
	private RuntimeAnimatorController _legendaryChest;

	[SerializeField]
	[Subcomponent(typeof(PlaySound))]
	private PlaySound _omenChestSound;

	[SerializeField]
	[Subcomponent(typeof(PlaySound))]
	private PlaySound _flySound;

	private Rarity _rarity;

	private Rarity _gearRarity;

	private Gear[] _rewards;

	private List<GearRequest> _toDropRequests;

	private List<GearReference> _toDropReferences;

	private Random _random;

	private int _rewardCount;

	private int _discardCount;

	private bool _isOmenChest;

	private bool _alreadyTryToChangeOmenChest;

	public bool looted { get; private set; }

	public bool isOmenChest => _isOmenChest;

	public event Action onLoot;

	public bool TryToChangeOmenChest()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		if (Singleton<DarktechManager>.Instance.IsActivated(DarktechData.Type.OmenAmplifier))
		{
			int stageIndex = currentChapter.stageIndex;
			float value = Singleton<DarktechManager>.Instance.setting.흉조증폭기확률[(currentChapter.type, stageIndex)].value;
			_isOmenChest = MMMaths.Chance(_random, value);
		}
		if (_isOmenChest)
		{
			_cat.transform.parent = ((Component)Map.Instance).transform;
		}
		_alreadyTryToChangeOmenChest = true;
		return _isOmenChest;
	}

	private void Start()
	{
		if (!_alreadyTryToChangeOmenChest)
		{
			TryToChangeOmenChest();
		}
		Initialize();
		Load();
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged += Load;
	}

	private void Initialize()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		_rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(_random);
		switch (_rarity)
		{
		case Rarity.Common:
			if (_isOmenChest)
			{
				((Component)_craw).transform.position = _commonCrawPosition.position;
				_animator.runtimeAnimatorController = _commonOmenChest;
			}
			else
			{
				_animator.runtimeAnimatorController = _commonChest;
			}
			break;
		case Rarity.Rare:
			if (_isOmenChest)
			{
				((Component)_craw).transform.position = _rareCrawPosition.position;
				_animator.runtimeAnimatorController = _rareOmenChest;
			}
			else
			{
				_animator.runtimeAnimatorController = _rareChest;
			}
			break;
		case Rarity.Unique:
			if (_isOmenChest)
			{
				((Component)_craw).transform.position = _uniqueCrawPosition.position;
				_animator.runtimeAnimatorController = _uniqueOmenChest;
			}
			else
			{
				_animator.runtimeAnimatorController = _uniqueChest;
			}
			break;
		case Rarity.Legendary:
			if (_isOmenChest)
			{
				((Component)_craw).transform.position = _legendaryCrawPosition.position;
				_animator.runtimeAnimatorController = _legendaryOmenChest;
			}
			else
			{
				_animator.runtimeAnimatorController = _legendaryChest;
			}
			break;
		}
		_omenObejct.SetActive(_isOmenChest);
		_rewardCount = (_isOmenChest ? _omenChestItemCount : _normalChestItemCount);
		_toDropRequests = new List<GearRequest>(_rewardCount);
		_toDropReferences = new List<GearReference>(_rewardCount);
		_rewards = new Gear[_rewardCount];
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
		_toDropReferences.Clear();
		ReleaseRequests();
		_toDropRequests.Clear();
		int num = _rewardCount;
		int num2 = 50;
		int num3 = 0;
		if (_isOmenChest)
		{
			num--;
			GearReference omenItems;
			do
			{
				EvaluateGearRarity();
				omenItems = Singleton<Service>.Instance.gearManager.GetOmenItems(_random);
				num3++;
				if (num3 >= num2)
				{
					Debug.LogError((object)"######## 이 에러가 나오면 개발팀에 알려주세요. 흉조");
				}
			}
			while (omenItems == null && num3 < num2);
			if (omenItems != null)
			{
				_toDropRequests.Add(omenItems.LoadAsync());
			}
		}
		num3 = 0;
		for (int i = 0; i < num; i++)
		{
			GearReference gearReference;
			do
			{
				num3++;
				EvaluateGearRarity();
				gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _gearRarity);
				if (gearReference == null)
				{
					continue;
				}
				foreach (GearReference toDropReference in _toDropReferences)
				{
					if (toDropReference == null || gearReference == null)
					{
						Debug.Log((object)"gear is null");
					}
					else if (toDropReference.name.Equals(gearReference.name))
					{
						gearReference = null;
						break;
					}
				}
				if (num3 >= num2)
				{
					Debug.LogError((object)"######## 이 에러가 나오면 개발팀에 알려주세요. 일반");
				}
			}
			while (gearReference == null && num3 < num2);
			_toDropReferences.Add(gearReference);
			_toDropRequests.Add(gearReference.LoadAsync());
		}
	}

	private void EvaluateGearRarity()
	{
		_gearRarity = Settings.instance.containerPossibilities[_rarity].Evaluate(_random);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		_animator.Play(InteractiveObject._activateHash);
		if (_isOmenChest)
		{
			_omenChestSound.Run(Singleton<Service>.Instance.levelManager.player);
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_animator.Play(InteractiveObject._deactivateHash);
		if (_isOmenChest)
		{
			_flySound.Run(Singleton<Service>.Instance.levelManager.player);
		}
		((Behaviour)_craw).enabled = true;
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
			for (int i = 0; i < _rewardCount; i++)
			{
				GearRequest request = _toDropRequests[i];
				while (!request.isDone)
				{
					elapsed += Chronometer.global.deltaTime;
					yield return null;
				}
			}
			while (elapsed < 0.5f)
			{
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			for (int j = 0; j < _rewardCount; j++)
			{
				GearRequest gearRequest = _toDropRequests[j];
				Gear gear = Singleton<Service>.Instance.levelManager.DropGear(gearRequest, ((Component)_dropPoint).transform.position);
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
		Array.Sort(_rewards, delegate(Gear gear1, Gear gear2)
		{
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
		for (int i = 0; i < _rewardCount; i++)
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
			if (_rewards[i].state == Gear.State.Dropped && (Object)(object)_rewards[i] != (Object)null && (Object)(object)((Component)_rewards[i]).gameObject != (Object)null)
			{
				((Component)_rewards[i]).gameObject.SetActive(false);
			}
		}
	}

	private void OnLoot(Character character)
	{
		for (int i = 0; i < _rewardCount; i++)
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
