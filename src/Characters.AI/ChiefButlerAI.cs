using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public class ChiefButlerAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _counterAttack;

	[SerializeField]
	[Subcomponent(typeof(SpawnEnemy))]
	private SpawnEnemy _spawnEnemy;

	[SerializeField]
	[Header("Spawn")]
	private Collider2D _spawnCollider;

	private bool _counter;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _counterAttack, _spawnEnemy };
		character.health.onTookDamage += TryCounterAttack;
	}

	private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_counter && _counterAttack.CanUse() && _spawnEnemy.result != Behaviour.Result.Doing)
		{
			StopAllBehaviour();
			_counter = true;
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
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null))
			{
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_spawnCollider)))
				{
					yield return _spawnEnemy.CRun(this);
				}
				if (_counter && character.health.currentHealth > 0.0 && !base.dead)
				{
					yield return _counterAttack.CRun(this);
					_counter = false;
				}
				LookTarget();
			}
		}
	}

	private void LookTarget()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		float targetX = ((Component)base.target).transform.position.x - ((Component)character).transform.position.x;
		character.ForceToLookAt(targetX);
	}
}
