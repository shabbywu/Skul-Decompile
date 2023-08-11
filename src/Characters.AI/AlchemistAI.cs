using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class AlchemistAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private Attack _attack;

	[Chase.Subcomponent(true)]
	[SerializeField]
	private Chase _chase;

	[SerializeField]
	private Collider2D _attackTrigger;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _idle, _attack, _chase };
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
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				if ((Object)(object)base.target != (Object)null && (Object)(object)base.target.movement != (Object)null && base.target.movement.isGrounded)
				{
					yield return _attack.CRun(this);
				}
				else
				{
					yield return null;
				}
				continue;
			}
			yield return _chase.CRun(this);
			if ((Object)(object)base.target != (Object)null && (Object)(object)base.target.movement != (Object)null && base.target.movement.isGrounded)
			{
				yield return _attack.CRun(this);
			}
			else
			{
				yield return null;
			}
		}
	}
}
