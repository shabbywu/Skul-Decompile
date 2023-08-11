using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Gear.Items;
using Characters.Player;
using Data;
using FX;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public sealed class FieldDruid : FieldNpc
{
	private const int _randomSeed = 2028506624;

	[SerializeField]
	private Item[] _items;

	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private EffectInfo _dropEffect;

	[SerializeField]
	private SoundInfo _dropSound;

	private Item _selected;

	private Random _random;

	protected override NpcType _type => NpcType.FieldDruid;

	protected override void Start()
	{
		base.Start();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
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
		DropWeapon();
		_phase = Phase.Gave;
		for (int j = lastIndex; j < base._greeting.Length; j++)
		{
			yield return _npcConversation.CConversation(base._greeting[j]);
		}
		LetterBox.instance.Disappear();
	}

	private void Load()
	{
		ItemInventory item = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
		int num = 7;
		for (int i = 0; i < num; i++)
		{
			_selected = ExtensionMethods.Random<Item>((IEnumerable<Item>)_items, _random);
			if (!item.Has(_selected))
			{
				break;
			}
		}
	}

	private void DropWeapon()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Load();
		Singleton<Service>.Instance.levelManager.DropItem(_selected, _dropPosition.position);
		_dropEffect.Spawn(_dropPosition.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dropSound, ((Component)this).transform.position);
	}
}
