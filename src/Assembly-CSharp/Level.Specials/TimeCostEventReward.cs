using System;
using System.Collections;
using Characters;
using Data;
using GameResources;
using Runnables;
using Runnables.Triggers;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Specials;

public class TimeCostEventReward : InteractiveObject
{
	private const int _randomSeed = 1281776104;

	private const float _delayToDrop = 0.5f;

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
	private RarityPossibilities _rarityPossibilities;

	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[SerializeField]
	private Runnable _runnable;

	private Rarity _rarity;

	private Rarity _gearRarity;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	private Random _random;

	public Rarity rarity => _rarity;

	private void Start()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 1281776104 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_rarity = _rarityPossibilities.Evaluate(_random);
		EvaluateGearRarity();
		switch (_rarity)
		{
		case Rarity.Common:
			_animator.runtimeAnimatorController = _commonChest;
			break;
		case Rarity.Rare:
			_animator.runtimeAnimatorController = _rareChest;
			break;
		case Rarity.Unique:
			_animator.runtimeAnimatorController = _uniqueChest;
			break;
		case Rarity.Legendary:
			_animator.runtimeAnimatorController = _legendaryChest;
			break;
		}
		Load();
	}

	private void OnDestroy()
	{
		_itemRequest?.Release();
	}

	private void Load()
	{
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
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (_trigger.IsSatisfied())
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
			((MonoBehaviour)this).StartCoroutine(CDelayedDrop());
			_runnable.Run();
			Deactivate();
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
		}
	}
}
