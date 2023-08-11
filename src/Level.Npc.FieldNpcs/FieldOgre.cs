using System;
using System.Collections;
using Characters;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public sealed class FieldOgre : FieldNpc
{
	private const int _randomSeed = 2028506624;

	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private EffectInfo _dropEffect;

	[SerializeField]
	private SoundInfo _dropSound;

	[SerializeField]
	private CustomFloat _currencyAmount;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	private Random _random;

	protected override NpcType _type => NpcType.FieldOgre;

	private RarityPossibilities _headPossibilities => Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.sleepySekeletonHeadPossibilities;

	protected override void Start()
	{
		base.Start();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	private void Load()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		do
		{
			Rarity rarity = _headPossibilities.Evaluate(_random);
			_itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(_random, rarity);
		}
		while (_itemToDrop == null);
		_itemRequest = _itemToDrop.LoadAsync();
	}

	protected override void Interact(Character character)
	{
		base.Interact(character);
		switch (_phase)
		{
		case Phase.Initial:
		case Phase.Greeted:
			((MonoBehaviour)this).StartCoroutine(CGreetingAndConfirm(character));
			break;
		case Phase.Gave:
			((MonoBehaviour)this).StartCoroutine(CChat());
			break;
		}
	}

	private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.skippable = true;
		int lastIndex = 3;
		for (int j = 0; j < lastIndex; j++)
		{
			yield return _npcConversation.CConversation(base._greeting[j]);
		}
		((MonoBehaviour)this).StartCoroutine(CDropWeapon());
		_phase = Phase.Gave;
		for (int j = lastIndex; j < base._greeting.Length; j++)
		{
			yield return _npcConversation.CConversation(base._greeting[j]);
		}
		LetterBox.instance.Disappear();
	}

	private IEnumerator CDropWeapon()
	{
		Load();
		while (!_itemRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= Load;
		Singleton<Service>.Instance.levelManager.DropItem(_itemRequest, _dropPosition.position);
		int num = (int)_currencyAmount.value;
		Singleton<Service>.Instance.levelManager.DropGold(num, Mathf.Min(num / 10, 30));
		_dropEffect.Spawn(_dropPosition.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dropSound, ((Component)this).transform.position);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_itemRequest?.Release();
	}
}
