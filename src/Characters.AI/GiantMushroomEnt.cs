using System.Collections;
using Characters.AI.Behaviours;
using Characters.Abilities;
using Runnables.States;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class GiantMushroomEnt : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[SerializeField]
	[Subcomponent(typeof(ChaseAndAttack))]
	private ChaseAndAttack _attack;

	[Subcomponent(typeof(ChaseAndAttack))]
	[SerializeField]
	private ChaseAndAttack _attackOnWeak;

	[SerializeField]
	private Behaviour _groggy;

	[SerializeField]
	private Behaviour _recover;

	[SerializeField]
	private float _weakStateTime = 3f;

	[SerializeField]
	private CharacterAnimation _charactrerAnimation;

	[SerializeField]
	private AnimationClip _idleClip;

	[SerializeField]
	private AnimationClip _walkClip;

	[SerializeField]
	private AnimationClip _idleOnWeakClip;

	[SerializeField]
	private AnimationClip _walkOnWeakClip;

	[SerializeField]
	private State _normalState;

	[SerializeField]
	private State _weakState;

	[SerializeField]
	private StateMachine _stateMachine;

	[SerializeField]
	private Collider2D _platformCollider;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _onGroggy;

	private bool _sequenceLock;

	private void Awake()
	{
		_onGroggy.Initialize();
		SetNormalAnimationClip();
	}

	private void OnDestroy()
	{
		_idleClip = null;
		_walkClip = null;
		_idleOnWeakClip = null;
		_walkOnWeakClip = null;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(CCheckWeakable());
	}

	private IEnumerator CCheckWeakable()
	{
		while ((Object)(object)_stateMachine.currentState != (Object)(object)_weakState)
		{
			yield return null;
		}
		((MonoBehaviour)this).StartCoroutine(CReturnToNormalState(_weakStateTime));
	}

	private IEnumerator CReturnToNormalState(float delay)
	{
		yield return CGroggy();
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)character.chronometer.master, delay);
		yield return CRecover();
	}

	private IEnumerator CGroggy()
	{
		_sequenceLock = true;
		character.CancelAction();
		_attackOnWeak.StopPropagation();
		_attack.StopPropagation();
		SetWeakAnimationClip();
		character.ability.Add(_onGroggy.ability);
		yield return _groggy.CRun(this);
		_sequenceLock = false;
	}

	private IEnumerator CRecover()
	{
		_sequenceLock = true;
		_stateMachine.TransitTo(_normalState);
		SetNormalAnimationClip();
		character.CancelAction();
		_attackOnWeak.StopPropagation();
		_attack.StopPropagation();
		yield return _recover.CRun(this);
		character.ability.Remove(_onGroggy.ability);
		((Behaviour)_platformCollider).enabled = true;
		_sequenceLock = false;
		((MonoBehaviour)this).StartCoroutine(CCheckWeakable());
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _wander.CRun(this);
		while (!base.dead)
		{
			if (base.stuned || _sequenceLock)
			{
				yield return null;
			}
			else if ((Object)(object)_stateMachine.currentState == (Object)(object)_weakState)
			{
				yield return _attackOnWeak.CRun(this);
			}
			else
			{
				yield return _attack.CRun(this);
			}
		}
	}

	private void SetWeakAnimationClip()
	{
		_charactrerAnimation.SetIdle(_idleOnWeakClip);
		_charactrerAnimation.SetWalk(_walkOnWeakClip);
	}

	private void SetNormalAnimationClip()
	{
		_charactrerAnimation.SetIdle(_idleClip);
		_charactrerAnimation.SetWalk(_walkClip);
	}
}
