using System;
using System.Collections;
using Characters;
using Characters.Gear;
using Data;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace Level;

public sealed class GearReward : MonoBehaviour
{
	private const int _randomSeed = -201960844;

	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private bool _hasMovements;

	[SerializeField]
	private GearPossibilities _gearPossibilities;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	[SerializeField]
	private UnityEvent _onDrop;

	[SerializeField]
	private UnityEvent _onDestroy;

	[SerializeField]
	private UnityEvent _onLoot;

	private GearReference _gearReference;

	private GearRequest _gearRequest;

	private Gear _droppedGear;

	private Random _random;

	private void Awake()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + -201960844 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	private void OnDestroy()
	{
		_gearRequest?.Release();
	}

	public void Lock()
	{
		_droppedGear.destructible = false;
		_droppedGear.lootable = false;
	}

	public void Unlock()
	{
		_droppedGear.destructible = true;
		_droppedGear.lootable = true;
	}

	public void DropIndestructibleGear()
	{
		((MonoBehaviour)this).StartCoroutine("CDropIndestructibleGear");
	}

	private IEnumerator CDropIndestructibleGear()
	{
		Load();
		while (!_gearRequest.isDone)
		{
			yield return null;
		}
		_droppedGear = Singleton<Service>.Instance.levelManager.DropGear(_gearRequest, _dropPoint.position);
		if (!_hasMovements)
		{
			_droppedGear.dropped.dropMovement.Stop();
		}
		_droppedGear.destructible = false;
		UnityEvent onDrop = _onDrop;
		if (onDrop != null)
		{
			onDrop.Invoke();
		}
		_droppedGear.dropped.onLoot += OnGearLoot;
		_droppedGear.dropped.onLoot += OnLootIndestructibleGear;
		_droppedGear.dropped.onDestroy += OnGearDestroy;
		void OnLootIndestructibleGear(Character character)
		{
			Unlock();
			_droppedGear.dropped.onLoot -= OnLootIndestructibleGear;
		}
	}

	public void Drop()
	{
		((MonoBehaviour)this).StartCoroutine("CDrop");
	}

	private IEnumerator CDrop()
	{
		Load();
		while (!_gearRequest.isDone)
		{
			yield return null;
		}
		_droppedGear = Singleton<Service>.Instance.levelManager.DropGear(_gearRequest, _dropPoint.position);
		if (!_hasMovements)
		{
			_droppedGear.dropped.dropMovement.Stop();
		}
		UnityEvent onDrop = _onDrop;
		if (onDrop != null)
		{
			onDrop.Invoke();
		}
		_droppedGear.dropped.onLoot += OnGearLoot;
		_droppedGear.dropped.onDestroy += OnGearDestroy;
	}

	private void Load()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		do
		{
			Rarity val = _rarityPossibilities.Evaluate(_random);
			Gear.Type? type = _gearPossibilities.Evaluate(_random);
			Rarity rarity = Settings.instance.containerPossibilities[val].Evaluate(_random);
			switch (type)
			{
			case Gear.Type.Weapon:
				_gearReference = Singleton<Service>.Instance.gearManager.GetWeaponToTake(_random, rarity);
				break;
			case Gear.Type.Item:
				_gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, rarity);
				break;
			case Gear.Type.Quintessence:
				_gearReference = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(_random, rarity);
				break;
			}
		}
		while (_gearReference == null);
		_gearRequest = _gearReference.LoadAsync();
	}

	private void OnGearLoot(Character character)
	{
		_droppedGear.dropped.onLoot -= OnGearLoot;
		_droppedGear.dropped.onDestroy -= OnGearDestroy;
		UnityEvent onLoot = _onLoot;
		if (onLoot != null)
		{
			onLoot.Invoke();
		}
	}

	private void OnGearDestroy(Character character)
	{
		_droppedGear.dropped.onLoot -= OnGearLoot;
		_droppedGear.dropped.onDestroy -= OnGearDestroy;
		UnityEvent onDestroy = _onDestroy;
		if (onDestroy != null)
		{
			onDestroy.Invoke();
		}
	}
}
