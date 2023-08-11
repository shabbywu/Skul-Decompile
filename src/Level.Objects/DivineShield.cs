using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Characters.Operations;
using FX;
using Hardmode;
using Level.Waves;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level.Objects;

public class DivineShield : MonoBehaviour
{
	[SerializeField]
	private Pin _normalTargetPin;

	[SerializeField]
	private Pin _atargetPin;

	[SerializeField]
	private Pin _btargetPin;

	[SerializeField]
	private Pin _ctargetPin;

	[SerializeField]
	private Prop _prop;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _smallShield;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _hitOperation;

	[SerializeField]
	private DivineShieldEffect _divineShieldEffect;

	[SerializeField]
	private EffectInfo _smallEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private EffectInfo _medium = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private EffectInfo _bigEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private EffectInfo _extraBigEffect = new EffectInfo
	{
		subordinated = true
	};

	private Pin _targetPin;

	private Character _target;

	private EffectPoolInstance _spawnedLoopEffect;

	private void Start()
	{
		switch (Singleton<HardmodeManager>.Instance.GetEnemyStep())
		{
		case HardmodeManager.EnemyStep.Normal:
			_targetPin = _normalTargetPin;
			break;
		case HardmodeManager.EnemyStep.A:
			_targetPin = _atargetPin;
			break;
		case HardmodeManager.EnemyStep.B:
			_targetPin = _btargetPin;
			break;
		case HardmodeManager.EnemyStep.C:
			_targetPin = _ctargetPin;
			break;
		}
		if (!((Object)(object)_targetPin == (Object)null))
		{
			((MonoBehaviour)this).StartCoroutine(CTryAttach());
		}
	}

	private IEnumerator CTryAttach()
	{
		yield return CWaitForSpawend();
		ICollection<Character> characters = _targetPin.characters;
		if (characters.Count >= 2)
		{
			Debug.LogError((object)"대상이 2명 이상입니다");
			yield break;
		}
		_target = ExtensionMethods.Random<Character>((IEnumerable<Character>)characters);
		Initialize();
		AttachShield();
	}

	private IEnumerator CWaitForSpawend()
	{
		while (!_targetPin.spawned)
		{
			yield return null;
		}
	}

	private void Initialize()
	{
		_divineShieldEffect.Activate(_target);
		_smallShield.Initialize();
		_hitOperation.Initialize();
	}

	private void AttachShield()
	{
		((MonoBehaviour)this).StartCoroutine(CAttach());
	}

	private IEnumerator CAttach()
	{
		while (!((Component)_target).gameObject.activeSelf)
		{
			yield return null;
		}
		_target.ability.Add(_smallShield.ability);
		switch (_target.sizeForEffect)
		{
		case Character.SizeForEffect.Small:
		case Character.SizeForEffect.None:
			_spawnedLoopEffect = _smallEffect.Spawn(((Component)_target).transform.position, _target);
			break;
		case Character.SizeForEffect.Medium:
			_spawnedLoopEffect = _medium.Spawn(((Component)_target).transform.position, _target);
			break;
		case Character.SizeForEffect.Large:
			_spawnedLoopEffect = _bigEffect.Spawn(((Component)_target).transform.position, _target);
			break;
		case Character.SizeForEffect.ExtraLarge:
			_spawnedLoopEffect = _extraBigEffect.Spawn(((Component)_target).transform.position, _target);
			break;
		default:
			_spawnedLoopEffect = _smallEffect.Spawn(((Component)_target).transform.position, _target);
			break;
		}
		_target.health.onTakeDamage.Add(int.MaxValue, (TakeDamageDelegate)OnTakeDamage);
		_prop.onDestroy += DetachShield;
		_target.onDie += InstantDestroy;
	}

	private void DetachShield()
	{
		if (!((Object)(object)_target == (Object)null) && _target.liveAndActive)
		{
			if ((Object)(object)_spawnedLoopEffect != (Object)null)
			{
				_spawnedLoopEffect.Stop();
				_spawnedLoopEffect = null;
			}
			_target.ability.Remove(_smallShield.ability);
			_target.health.onTakeDamage.Remove((TakeDamageDelegate)OnTakeDamage);
		}
	}

	private bool OnTakeDamage(ref Damage damage)
	{
		((Component)_hitOperation).gameObject.SetActive(true);
		_hitOperation.Run(_target);
		return true;
	}

	private void InstantDestroy()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (!_prop.destroyed)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Damage damage = new Damage(player, 10000.0, Vector2.zero, Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Basic, 1.0, 0f, 0.0, 1.0, 1.0, canCritical: true, @null: false, 0.0, 0.0, 0);
			_prop.Hit(player, ref damage);
		}
	}

	private void OnDestroy()
	{
		if ((Object)(object)_spawnedLoopEffect != (Object)null)
		{
			_spawnedLoopEffect.Stop();
			_spawnedLoopEffect = null;
		}
	}
}
