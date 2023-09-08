using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Movements;
using Characters.Operations;
using Characters.Player;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Weapons.Yaksha;

public sealed class YakshaHome : MonoBehaviour
{
	[SerializeField]
	private Weapon _yaksha;

	[SerializeField]
	[Header("사용자 설정")]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onActivate;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onEnter;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onExit;

	[SerializeField]
	[Header("영역 설정")]
	private float _duration;

	[SerializeField]
	private CircleCollider2D _enterBoundary;

	[SerializeField]
	private CircleCollider2D _exitBoundary;

	[Header("적 설정")]
	[SerializeField]
	private PushInfo _pushInfo = new PushInfo(ignoreOtherForce: false, expireOnGround: false);

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private Curve _fastenCurve;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent.Subcomponents _abilityComponents;

	private List<Character> _enemies;

	private NonAllocOverlapper _overlapper;

	private float _remainTime;

	private bool _playerisStaying;

	private bool _activated;

	private bool _ranActivateAction;

	private Character _owner;

	private WeaponInventory _inventory;

	private bool isYaksha => ((Object)_inventory.polymorphOrCurrent).name.Equals(((Object)_yaksha).name);

	private void Awake()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(128);
		_enemies = new List<Character>(128);
		_onActivate.Initialize();
		_onEnter.Initialize();
		_onExit.Initialize();
		_owner = _yaksha.owner;
		_inventory = _owner.playerComponents.inventory.weapon;
		_abilityComponents.Initialize();
		Singleton<Service>.Instance.levelManager.onMapLoaded += Disappear;
	}

	public void Update()
	{
		if ((Object)(object)_yaksha == (Object)null)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		if (_activated)
		{
			if (!_ranActivateAction)
			{
				((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(_onActivate.CRun(_owner));
				_ranActivateAction = true;
			}
			_remainTime -= Chronometer.global.deltaTime;
			if (_remainTime < 0f)
			{
				Disappear();
				return;
			}
			TryToAddEnemy();
			UpdateOwnerState();
			UpdateInnerEnemeyForce();
			CheckOutgoingEnemy();
		}
	}

	public void Appear()
	{
		_ranActivateAction = false;
		ClearEnmies();
		ResetValue();
		_inventory.onSwap -= Disappear;
		_inventory.onSwap += Disappear;
		if ((Object)(object)_yaksha != (Object)null)
		{
			_yaksha.onDropped -= Disappear;
			_yaksha.onDropped += Disappear;
		}
		TryToAddEnemy();
		CheckOutgoingEnemy();
		((Component)this).gameObject.SetActive(true);
	}

	private void ResetValue()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_remainTime = _duration;
		((Component)this).transform.position = ((Component)_owner).transform.position;
		_activated = true;
	}

	public void Disappear()
	{
		try
		{
			((MonoBehaviour)_owner).StartCoroutine(CExit());
			ClearEnmies();
		}
		catch (Exception arg)
		{
			Debug.LogError((object)$"Yaksha Home Error \n{arg}");
		}
		_activated = false;
		_playerisStaying = false;
		_ranActivateAction = false;
		if ((Object)(object)_inventory != (Object)null)
		{
			_inventory.onSwap -= Disappear;
		}
		if ((Object)(object)_yaksha != (Object)null)
		{
			_yaksha.onDropped -= Disappear;
		}
		if ((Object)(object)((Component)this).gameObject != (Object)null)
		{
			((Component)this).gameObject.SetActive(false);
		}
	}

	private void UpdateOwnerState()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (InEnterArea(Vector2.op_Implicit(((Component)_owner).transform.position)))
		{
			if (!_playerisStaying)
			{
				EnterPlayer();
			}
		}
		else if (_playerisStaying && ((Object)(object)_owner.runningMotion == (Object)null || (_owner.runningMotion.action.type != Characters.Actions.Action.Type.Skill && _owner.runningMotion.action.type != 0 && _owner.runningMotion.action.type != Characters.Actions.Action.Type.JumpAttack)))
		{
			ExitPlayer();
		}
	}

	private void TryToAddEnemy()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_owner).gameObject));
		List<Target> components = _overlapper.OverlapCircle(Vector2.op_Implicit(((Component)_enterBoundary).transform.position), _enterBoundary.radius).GetComponents<Target>(true);
		if (components.Count == 0)
		{
			return;
		}
		for (int i = 0; i < components.Count; i++)
		{
			Character character = components[i].character;
			if (!((Object)(object)character == (Object)null) && !_enemies.Contains(character))
			{
				_enemies.Add(character);
				for (int j = 0; j < _abilityComponents.components.Length; j++)
				{
					character.ability.Add(_abilityComponents.components[j].ability);
				}
			}
		}
	}

	private void ClearEnmies()
	{
		foreach (Character enemy in _enemies)
		{
			AbilityComponent[] components = _abilityComponents.components;
			foreach (AbilityComponent abilityComponent in components)
			{
				enemy.ability.Remove(abilityComponent.ability);
			}
		}
		_enemies.Clear();
	}

	private void UpdateInnerEnemeyForce()
	{
		foreach (Character enemy in _enemies)
		{
			TryFastenEnemy(enemy);
		}
	}

	private void CheckOutgoingEnemy()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		for (int num = _enemies.Count - 1; num >= 0; num--)
		{
			if (OutExitArea(Vector2.op_Implicit(((Component)_enemies[num]).transform.position)))
			{
				AbilityComponent[] components = _abilityComponents.components;
				foreach (AbilityComponent abilityComponent in components)
				{
					_enemies[num].ability.Remove(abilityComponent.ability);
				}
				_enemies.RemoveAt(num);
			}
		}
	}

	private void EnterPlayer()
	{
		_playerisStaying = true;
		if (isYaksha)
		{
			_onEnter.StopAll();
			((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(_onEnter.CRun(_owner));
		}
	}

	private void ExitPlayer()
	{
		_playerisStaying = false;
		_onEnter.StopAll();
		((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(_onExit.CRun(_owner));
	}

	private IEnumerator CExit()
	{
		while ((Object)(object)_owner.runningMotion != (Object)null && (_owner.runningMotion.action.type == Characters.Actions.Action.Type.Skill || _owner.runningMotion.action.type == Characters.Actions.Action.Type.Dash || _owner.runningMotion.action.type == Characters.Actions.Action.Type.JumpAttack))
		{
			yield return null;
		}
		ExitPlayer();
	}

	private void TryFastenEnemy(Character enemy)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (Chronometer.global.timeScale != 0f && !InEnterArea(Vector2.op_Implicit(((Component)enemy).transform.position)))
		{
			enemy.movement.push.ApplyKnockback(_owner, _pushInfo);
		}
	}

	private bool InEnterArea(Vector2 position)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (Vector2.SqrMagnitude(Vector2.op_Implicit(((Component)_enterBoundary).transform.position) - position) < _enterBoundary.radius * _enterBoundary.radius)
		{
			return true;
		}
		return false;
	}

	private bool OutExitArea(Vector2 position)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (Vector2.SqrMagnitude(Vector2.op_Implicit(((Component)_enterBoundary).transform.position) - position) > _exitBoundary.radius * _exitBoundary.radius)
		{
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= Disappear;
		}
	}
}
