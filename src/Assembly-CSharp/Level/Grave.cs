using System;
using System.Collections;
using Characters;
using Data;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class Grave : InteractiveObject, ILootable
{
	private const int _randomSeed = 925585528;

	private const float _delayToDrop = 0.4f;

	[SerializeField]
	[GetComponent]
	private Animator _animator;

	[SerializeField]
	private RuntimeAnimatorController _common;

	[SerializeField]
	private RuntimeAnimatorController _rare;

	[SerializeField]
	private RuntimeAnimatorController _unique;

	[SerializeField]
	private RuntimeAnimatorController _legendary;

	[SerializeField]
	private RewardEffect _effect;

	private Rarity _rarity;

	private Rarity _gearRarity;

	private WeaponReference _weaponToDrop;

	private WeaponRequest _weaponRequest;

	private Random _random;

	public Rarity rarity => _rarity;

	public bool looted { get; private set; }

	public event Action onLoot;

	protected override void Awake()
	{
		base.Awake();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 925585528 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	private void Start()
	{
		_rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(_random);
		switch (_rarity)
		{
		case Rarity.Common:
			_animator.runtimeAnimatorController = _common;
			break;
		case Rarity.Rare:
			_animator.runtimeAnimatorController = _rare;
			break;
		case Rarity.Unique:
			_animator.runtimeAnimatorController = _unique;
			break;
		case Rarity.Legendary:
			_animator.runtimeAnimatorController = _legendary;
			break;
		}
		Load();
	}

	private void OnDestroy()
	{
		_weaponRequest?.Release();
	}

	private void Load()
	{
		do
		{
			EvaluateGearRarity();
			_weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(_random, _gearRarity);
		}
		while (_weaponToDrop == null);
		_weaponRequest = _weaponToDrop.LoadAsync();
	}

	private void EvaluateGearRarity()
	{
		_gearRarity = Settings.instance.containerPossibilities[_rarity].Evaluate(_random);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		_animator.Play(InteractiveObject._activateHash);
		_effect.Play(_rarity);
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_animator.Play(InteractiveObject._deactivateHash);
	}

	public override void InteractWith(Character character)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		((MonoBehaviour)this).StartCoroutine(CDrop());
		Deactivate();
		IEnumerator CDrop()
		{
			float delay = 0.4f;
			delay += Time.unscaledTime;
			while (!_weaponRequest.isDone)
			{
				yield return null;
			}
			delay -= Time.unscaledTime;
			if (delay > 0f)
			{
				yield return Chronometer.global.WaitForSeconds(delay);
			}
			Singleton<Service>.Instance.levelManager.DropWeapon(_weaponRequest, ((Component)this).transform.position);
			this.onLoot?.Invoke();
			looted = true;
		}
	}
}
