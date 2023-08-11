using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class GoldmaneRecruitAI : AIController
{
	[Header("Behaviours")]
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private ActionAttack _attack;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[SerializeField]
	[Header("Guard")]
	private Action _guard;

	[SerializeField]
	[Header("Range")]
	private Collider2D _attackTrigger;

	[SerializeField]
	private Collider2D _guardTrigger;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _wander, _attack, _chase, _idle };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		((MonoBehaviour)this).StartCoroutine(ChangeStopTrigger());
		while (!base.dead)
		{
			yield return _wander.CRun(this);
			yield return Combat();
		}
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (base.stuned || (Object)(object)base.target == (Object)null || !character.movement.controller.isGrounded)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_guardTrigger) != (Object)null)
			{
				if (_guard.canUse)
				{
					yield return Guard();
				}
				else if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
				{
					yield return _attack.CRun(this);
				}
				else
				{
					yield return _chase.CRun(this);
				}
			}
			else
			{
				yield return _chase.CRun(this);
			}
		}
	}

	private IEnumerator Guard()
	{
		_guard.TryStart();
		while (_guard.running)
		{
			yield return null;
		}
		yield return _idle.CRun(this);
	}

	private IEnumerator ChangeStopTrigger()
	{
		while (!base.dead)
		{
			if (_guard.canUse)
			{
				stopTrigger = _guardTrigger;
			}
			else
			{
				stopTrigger = _attackTrigger;
			}
			yield return null;
		}
	}
}
