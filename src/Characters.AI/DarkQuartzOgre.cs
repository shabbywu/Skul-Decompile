using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class DarkQuartzOgre : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _counterAttack;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	[SerializeField]
	private Collider2D _attackTrigger;

	[SerializeField]
	[Range(0f, 1f)]
	private float _counterChance;

	private bool _counter;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _attack, _chase, _counterAttack, _idle };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		character.health.onTookDamage += Health_onTookDamage;
		character.health.onDie += delegate
		{
			character.health.onTookDamage -= Health_onTookDamage;
		};
	}

	private void Health_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_counter)
		{
			if (character.health.dead || base.dead || character.health.percent <= damageDealt)
			{
				_counter = true;
			}
			else if (_attack.result != Behaviour.Result.Doing && MMMaths.Chance(_counterChance))
			{
				StopAllBehaviour();
				_counter = true;
			}
		}
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _wander.CRun(this);
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null || base.stuned)
			{
				continue;
			}
			if (_counter && character.health.currentHealth > 0.0 && !base.dead)
			{
				yield return _counterAttack.CRun(this);
				_counter = false;
			}
			if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				yield return _attack.CRun(this);
				continue;
			}
			yield return _chase.CRun(this);
			if (_chase.result == Behaviour.Result.Success)
			{
				yield return _attack.CRun(this);
			}
		}
	}
}
