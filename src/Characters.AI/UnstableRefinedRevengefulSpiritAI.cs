using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class UnstableRefinedRevengefulSpiritAI : AIController
{
	[SerializeField]
	private bool _introSkip;

	[SerializeField]
	private Collider2D _dashTrigger;

	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(FlyChase))]
	[SerializeField]
	private FlyChase _flyChase;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private ActionAttack _attack;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private ActionAttack _dash;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	[SerializeField]
	private Action _intro;

	[SerializeField]
	[Range(0f, 100f)]
	private float _attackAccelerationNear;

	[SerializeField]
	[Range(0f, 100f)]
	private float _attackAccelerationFar;

	[SerializeField]
	private float _suicideDelay = 0.1f;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _flyChase, _attack, _dash };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CDash());
	}

	protected override IEnumerator CProcess()
	{
		character.movement.config.acceleration = _attackAccelerationFar;
		character.movement.MoveHorizontal(Vector2.op_Implicit(Random.rotation * Vector3.forward));
		yield return CPlayStartOption();
		if (!_introSkip)
		{
			_intro.TryStart();
		}
		yield return _idle.CRun(this);
		while (!base.dead)
		{
			yield return CCombat();
		}
	}

	private IEnumerator CCombat()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null))
			{
				if ((Object)(object)FindClosestPlayerBody(_dashTrigger) != (Object)null)
				{
					yield return _dash.CRun(this);
					character.movement.config.keepMove = false;
					yield return Chronometer.global.WaitForSeconds(_suicideDelay);
					yield return _attack.CRun(this);
				}
				else
				{
					yield return _flyChase.CRun(this);
				}
			}
		}
	}

	private IEnumerator CDash()
	{
		while (!base.dead)
		{
			if ((Object)(object)FindClosestPlayerBody(_dashTrigger) != (Object)null)
			{
				character.movement.config.acceleration = _attackAccelerationNear;
			}
			else
			{
				character.movement.config.acceleration = _attackAccelerationFar;
			}
			yield return null;
		}
	}
}
