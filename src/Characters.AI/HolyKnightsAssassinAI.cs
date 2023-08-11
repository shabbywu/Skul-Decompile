using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class HolyKnightsAssassinAI : AIController
{
	[Header("Behaviours")]
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(TeleportAttack))]
	private TeleportAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(LightJump))]
	private LightJump _jumpAttack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _evasion;

	[SerializeField]
	private Action _teleportIn;

	[Subcomponent(typeof(ShiftObject))]
	[SerializeField]
	private ShiftObject _shiftObject;

	[SerializeField]
	private Transform _destination;

	[SerializeField]
	[Header("Tools")]
	[Space]
	private Counter _evasionCounter;

	[SerializeField]
	private float _stagger;

	private bool _counter;

	private float _lastStagger;

	private ChronometerTime _time;

	private void Awake()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _attack, _jumpAttack, _evasion };
		_time = new ChronometerTime(character.chronometer.master, (MonoBehaviour)(object)character);
		_evasionCounter.Initialize(_time);
		character.health.onTookDamage += OnTookDamage;
		_teleportIn.onStart += delegate
		{
			_counter = false;
		};
		_evasionCounter.onArrival += Evasion;
	}

	private void Evasion()
	{
		_counter = true;
		StopAllBehaviour();
	}

	private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		_lastStagger = _time.time;
		if (_attack.result == Behaviour.Result.Doing)
		{
			_attack.StopPropagation();
		}
		TryCountForEvasion(damageDealt);
	}

	private void TryCountForEvasion(double damageDealt)
	{
		if (!_counter && _evasion.CanUse())
		{
			if (character.health.dead || base.dead || character.health.currentHealth <= damageDealt)
			{
				StopAllBehaviour();
			}
			else if (_evasion.result != Behaviour.Result.Doing)
			{
				_evasionCounter.Count();
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		yield return _wander.CRun(this);
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null || base.stuned)
			{
				continue;
			}
			if (_counter && character.health.currentHealth > 0.0 && !base.dead)
			{
				yield return _evasion.CRun(this);
				_counter = false;
			}
			else if (!(_lastStagger > _time.time - _stagger))
			{
				_teleportIn.TryStart();
				while (_teleportIn.running)
				{
					yield return null;
				}
				if (_jumpAttack.CanUse(character) && MMMaths.RandomBool())
				{
					yield return CRunLightJump();
				}
				else
				{
					yield return _attack.CRun(this);
				}
			}
		}
	}

	private IEnumerator CRunLightJump()
	{
		_shiftObject.Run(character);
		if (character.movement.controller.Teleport(Vector2.op_Implicit(_destination.position)))
		{
			yield return _jumpAttack.CRun(this);
		}
		else
		{
			yield return _attack.CRun(this);
		}
	}
}
