using System;
using System.Collections;
using Characters.Gear;
using Data;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public sealed class DropGear : Runnable
{
	private const int _randomSeed = -1728020458;

	[SerializeField]
	private GearPossibilities _gearPossibilities;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	private GearReference _gearReference;

	private GearRequest _gearRequest;

	public override void Run()
	{
		((MonoBehaviour)this).StartCoroutine("CDrop");
	}

	private void OnDestroy()
	{
		_gearRequest?.Release();
	}

	private IEnumerator CDrop()
	{
		Load();
		while (!_gearRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.levelManager.DropGear(_gearRequest, ((Component)this).transform.position);
	}

	private void Load()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		Random random = new Random(GameData.Save.instance.randomSeed + -1728020458 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		Rarity key = _rarityPossibilities.Evaluate();
		Gear.Type? type = _gearPossibilities.Evaluate();
		do
		{
			Rarity rarity = Settings.instance.containerPossibilities[key].Evaluate(random);
			switch (type)
			{
			case Gear.Type.Weapon:
				_gearReference = Singleton<Service>.Instance.gearManager.GetWeaponToTake(random, rarity);
				break;
			case Gear.Type.Item:
				_gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(random, rarity);
				break;
			case Gear.Type.Quintessence:
				_gearReference = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(random, rarity);
				break;
			}
		}
		while (_gearReference == null);
		_gearRequest = _gearReference.LoadAsync();
	}
}
