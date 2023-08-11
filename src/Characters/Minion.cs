using System;
using System.Collections;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Minions;
using Characters.Movements;
using Characters.Player;
using Scenes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters;

[DisallowMultipleComponent]
[RequireComponent(typeof(Character))]
public class Minion : MonoBehaviour
{
	public enum State
	{
		Unsummoned,
		Summoned,
		Expired,
		Unsummoning
	}

	public delegate void OnSummonDelegate(Character owner, Character summoned);

	public delegate void OnUnsummonDelegate(Character owner, Character summoned);

	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[GetComponent]
	[SerializeField]
	private Character _character;

	[SerializeField]
	private MinionSetting _defaultSetting;

	[SerializeField]
	private CharacterSynchronization _syncWithOwner;

	[FormerlySerializedAs("_onDespawn")]
	[SerializeField]
	private Characters.Actions.Action _onExpired;

	private State _state;

	private MinionGroup _group;

	private float _elapsedLifeTime;

	private bool _initialized;

	public State state => _state;

	private bool isActivated => ((Component)this).gameObject.activeInHierarchy;

	public int maxCount => _defaultSetting.maxCount;

	public float lifeTime { get; private set; }

	public Character character => _character;

	public MinionLeader leader { get; private set; }

	public event OnSummonDelegate onSummon;

	public event OnUnsummonDelegate onUnsummon;

	private void Awake()
	{
		if ((Object)(object)_onExpired != (Object)null)
		{
			_onExpired.Initialize(_character);
		}
	}

	private void Update()
	{
		if (isActivated)
		{
			TryToExpire();
		}
	}

	public Minion Summon(MinionLeader minionOwner, Vector3 position, MinionGroup group, MinionSetting overrideSetting)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		Minion component = ((Component)_poolObject.Spawn(false)).GetComponent<Minion>();
		if ((Object)(object)overrideSetting != (Object)null)
		{
			component._defaultSetting = overrideSetting;
		}
		component.InitializeState(minionOwner, group);
		Movement movement = component.character.movement;
		if ((Object)(object)movement != (Object)null && (movement.config.type == Movement.Config.Type.Walking || movement.config.type == Movement.Config.Type.AcceleratingWalking))
		{
			component.character.movement.verticalVelocity = 0f;
			component.character.animationController.ForceUpdate();
		}
		((Component)component).transform.position = position;
		return component;
	}

	private void InitializeState(MinionLeader leader, MinionGroup group)
	{
		_state = State.Summoned;
		_initialized = true;
		this.leader = leader;
		JoinGroup(group);
		_elapsedLifeTime = 0f;
		_syncWithOwner.Synchronize(_character, leader.player);
		AttachEvents();
		if (!_defaultSetting.despawnOnMapChanged)
		{
			Scene<GameBase>.instance.poolObjectContainer.Push(_poolObject);
		}
		((Component)this).gameObject.SetActive(true);
		this.onSummon?.Invoke(leader.player, _character);
	}

	private void TryToExpire()
	{
		_elapsedLifeTime += ((ChronometerBase)Chronometer.global).deltaTime;
		if (_elapsedLifeTime > _defaultSetting.lifeTime && _state != State.Unsummoning)
		{
			_state = State.Expired;
		}
		if (_state == State.Expired && _state != State.Unsummoning)
		{
			Despawn();
		}
	}

	public void Despawn()
	{
		LeaveGroup();
		if ((Object)(object)_onExpired == (Object)null)
		{
			RevertState();
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CDespawn());
		}
	}

	public void DespawnImmediately()
	{
		LeaveGroup();
		RevertState();
	}

	public void OnDisable()
	{
		if (_initialized && _state != 0)
		{
			LeaveGroup();
			RevertState();
		}
	}

	private IEnumerator CDespawn()
	{
		_state = State.Unsummoning;
		_onExpired.TryStart();
		while (_onExpired.running)
		{
			yield return null;
		}
		RevertState();
	}

	private void RevertState()
	{
		_state = State.Unsummoned;
		this.onUnsummon?.Invoke(leader.player, _character);
		DettachEvents();
		_poolObject.Despawn();
	}

	private void JoinGroup(MinionGroup group)
	{
		_group = group;
		_group.Join(this);
	}

	private void LeaveGroup()
	{
		if (_group != null)
		{
			_group.Leave(this);
		}
	}

	private void AttachEvents()
	{
		if (_defaultSetting.triggerOnKilled)
		{
			Character obj = _character;
			obj.onKilled = (Character.OnKilledDelegate)Delegate.Combine(obj.onKilled, new Character.OnKilledDelegate(OnKilled));
		}
		if (_defaultSetting.triggerOnGiveDamage)
		{
			((PriorityList<GiveDamageDelegate>)_character.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)OnGiveDamage);
		}
		if (_defaultSetting.triggerOnGaveDamage)
		{
			Character obj2 = _character;
			obj2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(obj2.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}
		if (_defaultSetting.triggerOnGaveStatus)
		{
			Character obj3 = _character;
			obj3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(obj3.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
		}
		foreach (Stat.OnUpdatedDelegate item in leader.player.stat.onUpdated)
		{
			_character.stat.onUpdated.Add(0, item);
		}
		if (_defaultSetting.despawnOnSwap)
		{
			leader.player.onStartAction += OnSwapAction;
		}
		if (_defaultSetting.despawnOnEssenceChanged)
		{
			leader.player.playerComponents.inventory.quintessence.onChanged += Despawn;
		}
		if (_defaultSetting.despawnOnWeaponDropped)
		{
			leader.player.playerComponents.inventory.weapon.onChanged += OnWeaponChanged;
		}
	}

	private void DettachEvents()
	{
		if (leader != null)
		{
			if (_defaultSetting.triggerOnKilled)
			{
				Character obj = _character;
				obj.onKilled = (Character.OnKilledDelegate)Delegate.Remove(obj.onKilled, new Character.OnKilledDelegate(OnKilled));
			}
			if (_defaultSetting.triggerOnGiveDamage)
			{
				((PriorityList<GiveDamageDelegate>)_character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
			}
			if (_defaultSetting.triggerOnGaveDamage)
			{
				Character obj2 = _character;
				obj2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(obj2.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
			}
			if (_defaultSetting.triggerOnGaveStatus)
			{
				Character obj3 = _character;
				obj3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(obj3.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
			}
			_character.stat.onUpdated.Clear();
			if (_defaultSetting.despawnOnSwap)
			{
				leader.player.onStartAction -= OnSwapAction;
			}
			if (_defaultSetting.despawnOnEssenceChanged)
			{
				leader.player.playerComponents.inventory.quintessence.onChanged -= Despawn;
			}
			if (_defaultSetting.despawnOnWeaponDropped)
			{
				leader.player.playerComponents.inventory.weapon.onChanged -= OnWeaponChanged;
			}
		}
	}

	private void OnKilled(ITarget target, ref Damage damage)
	{
		leader.player.onKilled?.Invoke(target, ref damage);
	}

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
		return leader.player.onGiveDamage.Invoke(target, ref damage);
	}

	private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		leader.player.onGaveDamage?.Invoke(target, in originalDamage, in gaveDamage, damageDealt);
	}

	private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
	{
		leader.player.onGaveStatus?.Invoke(target, applyInfo, result);
	}

	private void OnSwapAction(Characters.Actions.Action action)
	{
		if (action.type == Characters.Actions.Action.Type.Swap)
		{
			Despawn();
		}
	}

	private void OnWeaponChanged(Weapon old, Weapon @new)
	{
		Despawn();
	}
}
