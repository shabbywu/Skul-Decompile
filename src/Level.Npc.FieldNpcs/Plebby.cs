using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public class Plebby : FieldNpc
{
	private const int _randomSeed = 2028506624;

	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private EffectInfo _dropEffect;

	[SerializeField]
	private SoundInfo _dropSound;

	private ItemReference _itemToDrop;

	private ItemRequest _itemRequest;

	private Random _random;

	protected override NpcType _type => NpcType.Plebby;

	protected string _displayNameA => Localization.GetLocalizedString($"npc/{_type}/A/name");

	protected string _displayNameB => Localization.GetLocalizedString($"npc/{_type}/B/name");

	private int _goldCost => Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.plebbyGoldCost;

	private RarityPossibilities _itemPossibilities => Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.plebbyItemPossibilities;

	protected override void Start()
	{
		base.Start();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_itemRequest?.Release();
	}

	private void Load()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		_itemRequest?.Release();
		do
		{
			Rarity rarity = _itemPossibilities.Evaluate(_random);
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

	private IEnumerator CGreetingAndConfirm(Character character)
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.skippable = true;
		string arg = ((_phase == Phase.Initial) ? "greeting" : "regreeting");
		string[] greeting = Localization.GetLocalizedStringArray($"npc/{_type}/{arg}");
		string[] speaker = Localization.GetLocalizedStringArray($"npc/{_type}/{arg}/speaker");
		_phase = Phase.Greeted;
		int lastIndex = greeting.Length - 1;
		for (int i = 0; i < lastIndex; i++)
		{
			_npcConversation.name = speaker[i];
			yield return _npcConversation.CConversation(greeting[i]);
		}
		_npcConversation.name = speaker[lastIndex];
		_npcConversation.body = string.Format(greeting[lastIndex], _goldCost);
		_npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.Gold);
		yield return _npcConversation.CType();
		yield return (object)new WaitForSecondsRealtime(0.3f);
		_npcConversation.OpenConfirmSelector(OnConfirmed, base.Close);
	}

	private void OnConfirmed()
	{
		_npcConversation.CloseCurrencyBalancePanel();
		if (GameData.Currency.gold.Has(_goldCost))
		{
			_phase = Phase.Gave;
			((MonoBehaviour)this).StartCoroutine(CConfirmed());
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CNoMoneyAndClose());
		}
	}

	private IEnumerator CConversation(string key)
	{
		string[] localizedStringArray = Localization.GetLocalizedStringArray(key);
		string[] localizedStringArray2 = Localization.GetLocalizedStringArray(key + "/speaker");
		yield return CConversation(localizedStringArray2, localizedStringArray);
	}

	private IEnumerator CConversation(string[] speakers, string[] scripts)
	{
		for (int i = 0; i < scripts.Length; i++)
		{
			_npcConversation.name = speakers[i];
			yield return _npcConversation.CConversation(scripts[i]);
		}
	}

	private IEnumerator CConfirmed()
	{
		_npcConversation.skippable = true;
		Load();
		yield return CDropItem();
		GameData.Currency.gold.Consume(_goldCost);
		yield return CConversation($"npc/{_type}/confirmed");
		LetterBox.instance.Disappear();
	}

	private IEnumerator CDropItem()
	{
		while (!_itemRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= Load;
		Singleton<Service>.Instance.levelManager.DropItem(_itemRequest, _dropPosition.position);
		_dropEffect.Spawn(_dropPosition.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dropSound, ((Component)this).transform.position);
	}

	private IEnumerator CNoMoneyAndClose()
	{
		_npcConversation.skippable = true;
		yield return CConversation($"npc/{_type}/noMoney");
		LetterBox.instance.Disappear();
	}

	private new IEnumerator CChat()
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.skippable = true;
		string[][] localizedStringArrays = Localization.GetLocalizedStringArrays($"npc/{_type}/chat");
		string[][] localizedStringArrays2 = Localization.GetLocalizedStringArrays($"npc/{_type}/chat/speaker");
		int num = ExtensionMethods.RandomIndex<string[]>((IEnumerable<string[]>)localizedStringArrays);
		yield return CConversation(localizedStringArrays2[num], localizedStringArrays[num]);
		LetterBox.instance.Disappear();
	}
}
