using System;
using System.Collections;
using Characters;
using Characters.Operations.Fx;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public class SleepySkeleton : FieldNpc
{
	private const int _randomSeed = 2028506624;

	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private EffectInfo _dropEffect;

	[SerializeField]
	private SoundInfo _dropSound;

	[SerializeField]
	private Characters.Operations.Fx.Vignette _vignette;

	[SerializeField]
	private ShaderEffect _shaderEffect;

	private WeaponReference _weaponToDrop;

	private WeaponRequest _weaponRequest;

	private Random _random;

	protected override NpcType _type => NpcType.SleepySkeleton;

	private int _healthPercentToTake => Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.sleepySekeletonHealthPercentCost;

	private RarityPossibilities _headPossibilities => Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.sleepySekeletonHeadPossibilities;

	protected override void Start()
	{
		base.Start();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		Load();
		Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged += Load;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= Load;
		_weaponRequest?.Release();
	}

	private void Load()
	{
		do
		{
			Rarity rarity = _headPossibilities.Evaluate(_random);
			_weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(_random, rarity);
		}
		while (_weaponToDrop == null);
		_weaponRequest = _weaponToDrop.LoadAsync();
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
		string[] scripts = ((_phase == Phase.Initial) ? base._greeting : base._regreeting);
		_phase = Phase.Greeted;
		_npcConversation.skippable = true;
		int lastIndex = scripts.Length - 1;
		for (int i = 0; i < lastIndex; i++)
		{
			yield return _npcConversation.CConversation(scripts[i]);
		}
		_npcConversation.skippable = true;
		_npcConversation.body = ((confirmArg == null) ? scripts[lastIndex] : string.Format(scripts[lastIndex], confirmArg));
		yield return _npcConversation.CType();
		yield return (object)new WaitForSecondsRealtime(0.3f);
		_npcConversation.OpenConfirmSelector(delegate
		{
			OnConfirmed(character);
		}, base.Close);
	}

	private void OnConfirmed(Character character)
	{
		((MonoBehaviour)this).StartCoroutine(CDropHead());
		IEnumerator CDropHead()
		{
			yield return CDropWeapon();
			GiveDamage(character);
			_phase = Phase.Gave;
			_npcConversation.skippable = true;
			yield return _npcConversation.CConversation(base._confirmed);
			LetterBox.instance.Disappear();
		}
	}

	private IEnumerator CDropWeapon()
	{
		while (!_weaponRequest.isDone)
		{
			yield return null;
		}
		Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= Load;
		Singleton<Service>.Instance.levelManager.DropWeapon(_weaponRequest, _dropPosition.position);
		_dropEffect.Spawn(_dropPosition.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dropSound, ((Component)this).transform.position);
	}

	private void GiveDamage(Character character)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		double num = character.health.maximumHealth * (double)_healthPercentToTake * 0.01;
		if (Math.Floor(character.health.currentHealth) <= num)
		{
			num = character.health.currentHealth - 1.0;
		}
		_vignette.Run(character);
		_shaderEffect.Run(character);
		character.health.TakeHealth(num);
		Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(num, Vector2.op_Implicit(((Component)character).transform.position));
	}
}
