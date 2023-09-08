using System;
using System.Collections;
using Characters;
using Characters.Gear.Items;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Npc.FieldNpcs;

public sealed class MagicalSlime : FieldNpc
{
	private const int _randomSeed = -699075432;

	[SerializeField]
	private Transform _itemDropPoint;

	[SerializeField]
	private AnimationClip _polymorphAnimationClip;

	[SerializeField]
	private SoundInfo _polymorphStartSound;

	[SerializeField]
	private SoundInfo _polymorphEndSound;

	[SerializeField]
	private UnityEvent _onPolymorphEnd;

	[SerializeField]
	[Header("아이템이 없을 때")]
	private Item _specialItem;

	private ReusableAudioSource _spawnedPolymorphStartSound;

	private readonly int _polymorphCastingHash = Animator.StringToHash("Polymorph_Casting");

	private readonly int _idle = Animator.StringToHash("Idle");

	protected override NpcType _type => NpcType.MagicalSlime;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_polymorphAnimationClip = null;
		_specialItem = null;
	}

	protected override void OnStopConversation()
	{
		base.OnStopConversation();
		_animator.Play(_idle);
		if ((Object)(object)_spawnedPolymorphStartSound != (Object)null)
		{
			_spawnedPolymorphStartSound.Stop();
		}
	}

	protected override void Interact(Character character)
	{
		base.Interact(character);
		if (_phase != Phase.Gave)
		{
			((MonoBehaviour)this).StartCoroutine(CGreetingAndPolymorph());
		}
	}

	private IEnumerator CGreetingAndPolymorph()
	{
		yield return LetterBox.instance.CAppear();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		Random random = new Random(GameData.Save.instance.randomSeed + -699075432 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		Item item = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item.GetRandomItem(random);
		if ((Object)(object)item == (Object)null)
		{
			yield return CNoItem();
		}
		else
		{
			yield return CGreeting();
			yield return CPolymorphToRandomItem(item);
		}
		Close();
		((Component)this).gameObject.SetActive(false);
	}

	private IEnumerator CNoItem()
	{
		_npcConversation.skippable = true;
		yield return _npcConversation.CConversation(base._noMoney);
		_animator.Play(_polymorphCastingHash);
		_spawnedPolymorphStartSound = PersistentSingleton<SoundManager>.Instance.PlaySound(_polymorphStartSound, ((Component)this).transform.position);
		yield return Chronometer.global.WaitForSeconds(_polymorphAnimationClip.length);
		UnityEvent onPolymorphEnd = _onPolymorphEnd;
		if (onPolymorphEnd != null)
		{
			onPolymorphEnd.Invoke();
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_polymorphEndSound, ((Component)this).transform.position);
		Singleton<Service>.Instance.levelManager.DropItem(_specialItem, ((Object)(object)_itemDropPoint == (Object)null) ? ((Component)this).transform.position : _itemDropPoint.position);
		_phase = Phase.Gave;
	}

	private IEnumerator CPolymorphToRandomItem(Item targetItem)
	{
		ItemReference itemByKey = Singleton<Service>.Instance.gearManager.GetItemByKey(((Object)targetItem).name);
		ItemRequest request = itemByKey.LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		_animator.Play(_polymorphCastingHash);
		_spawnedPolymorphStartSound = PersistentSingleton<SoundManager>.Instance.PlaySound(_polymorphStartSound, ((Component)this).transform.position);
		yield return Chronometer.global.WaitForSeconds(_polymorphAnimationClip.length);
		UnityEvent onPolymorphEnd = _onPolymorphEnd;
		if (onPolymorphEnd != null)
		{
			onPolymorphEnd.Invoke();
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_polymorphEndSound, ((Component)this).transform.position);
		Item item = Singleton<Service>.Instance.levelManager.DropItem(request, ((Object)(object)_itemDropPoint == (Object)null) ? ((Component)this).transform.position : _itemDropPoint.position);
		item.keyword1 = targetItem.keyword1;
		item.keyword2 = targetItem.keyword2;
		IStackable componentInChildren = ((Component)targetItem).GetComponentInChildren<IStackable>();
		if (componentInChildren != null)
		{
			((Component)item).GetComponentInChildren<IStackable>().stack = componentInChildren.stack;
		}
		_phase = Phase.Gave;
	}
}
