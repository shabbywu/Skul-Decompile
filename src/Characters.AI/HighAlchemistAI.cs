using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class HighAlchemistAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[SerializeField]
	[Subcomponent(typeof(Chase))]
	private Chase _chase;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private ActionAttack _flaskAttack;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private ActionAttack _summonAttack;

	[Wander.Subcomponent(true)]
	[SerializeField]
	private Wander _wanderAfterAttack;

	[SerializeField]
	private Collider2D _attackTrigger;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _idle, _chase, _flaskAttack, _summonAttack };
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
		yield return _wander.CRun(this);
		yield return _idle.CRun(this);
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				if (_flaskAttack.CanUse())
				{
					if (!character.movement.controller.isGrounded)
					{
						continue;
					}
					yield return _flaskAttack.CRun(this);
				}
				else
				{
					yield return _summonAttack.CRun(this);
				}
				if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
				{
					yield return _wanderAfterAttack.CRun(this);
				}
				else
				{
					yield return _idle.CRun(this);
				}
			}
			else
			{
				yield return _chase.CRun(this);
			}
		}
	}
}
