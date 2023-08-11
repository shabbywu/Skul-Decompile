using System;
using System.Collections;
using Data;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Items;

public sealed class DropByRarity : CharacterOperation
{
	private static int _extraSeed;

	private const int _randomSeed = 2028506624;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	private Random _random;

	private Rarity _rarity;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	public override void Initialize()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_extraSeed++;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + _extraSeed + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_rarity = _rarityPossibilities.Evaluate(_random);
	}

	private void Load()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		_itemRequest?.Release();
		do
		{
			_itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, _rarity);
		}
		while (_itemToDrop == null);
		_itemRequest = _itemToDrop.LoadAsync();
	}

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private IEnumerator CRun()
	{
		Load();
		while (!_itemRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.levelManager.DropItem(_itemRequest, ((Component)this).transform.position);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= Load;
		_extraSeed--;
		_itemRequest?.Release();
	}
}
