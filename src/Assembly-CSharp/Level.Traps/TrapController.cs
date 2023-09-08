using System;
using System.Collections;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps;

public class TrapController : MonoBehaviour
{
	[Serializable]
	public class Config
	{
		internal enum Condition
		{
			None,
			OnCharacterDie,
			OnTargetWaveSpawn,
			OnTargetWaveClear,
			PlayerOnTriggerEnter,
			PlayerOnTriggerExit,
			OnPropDestory
		}

		[SerializeField]
		internal Condition condition;

		[SerializeField]
		internal Character character;

		[SerializeField]
		internal EnemyWave targetWave;

		[SerializeField]
		internal Collider2D range;

		[SerializeField]
		internal Prop prop;

		[SerializeField]
		internal bool once;
	}

	[SerializeField]
	private ControlableTrap _targetTrap;

	[SerializeField]
	private Config _activate;

	[SerializeField]
	private Config _deactivate;

	private Action OnColliderEnter;

	private Action OnColliderExit;

	private static readonly NonAllocOverlapper _lapper;

	static TrapController()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_lapper = new NonAllocOverlapper(15);
		((ContactFilter2D)(ref _lapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
	}

	private void Start()
	{
		Initialize(_activate, _targetTrap.Activate);
		Initialize(_deactivate, _targetTrap.Deactivate);
	}

	private void Initialize(Config config, Action run)
	{
		switch (config.condition)
		{
		case Config.Condition.None:
			if (config == _activate)
			{
				_targetTrap.Activate();
			}
			break;
		case Config.Condition.OnCharacterDie:
			config.character.health.onDied += run;
			break;
		case Config.Condition.OnTargetWaveSpawn:
			if (config.targetWave.state == Wave.State.Spawned)
			{
				run?.Invoke();
			}
			else
			{
				config.targetWave.onSpawn += run;
			}
			break;
		case Config.Condition.OnTargetWaveClear:
			config.targetWave.onClear += run;
			break;
		case Config.Condition.PlayerOnTriggerEnter:
			((MonoBehaviour)this).StartCoroutine(CTriggerEnter(config));
			OnColliderEnter = (Action)Delegate.Combine(OnColliderEnter, run);
			break;
		case Config.Condition.PlayerOnTriggerExit:
			((MonoBehaviour)this).StartCoroutine(CTriggerExit(config));
			OnColliderExit = (Action)Delegate.Combine(OnColliderExit, run);
			break;
		case Config.Condition.OnPropDestory:
			config.prop.onDestroy += run;
			break;
		}
	}

	private IEnumerator CTriggerEnter(Config config)
	{
		while (true)
		{
			if ((Object)(object)_lapper.OverlapCollider(config.range).GetComponent<Character>() == (Object)null)
			{
				yield return null;
				continue;
			}
			OnColliderEnter?.Invoke();
			if (config.once)
			{
				break;
			}
			while ((Object)(object)_lapper.OverlapCollider(config.range).GetComponent<Character>() != (Object)null)
			{
				yield return null;
			}
		}
	}

	private IEnumerator CTriggerExit(Config config)
	{
		while (true)
		{
			if ((Object)(object)_lapper.OverlapCollider(config.range).GetComponent<Character>() == (Object)null)
			{
				yield return null;
				continue;
			}
			while ((Object)(object)_lapper.OverlapCollider(config.range).GetComponent<Character>() != (Object)null)
			{
				yield return null;
			}
			OnColliderExit?.Invoke();
			if (config.once)
			{
				break;
			}
		}
	}
}
