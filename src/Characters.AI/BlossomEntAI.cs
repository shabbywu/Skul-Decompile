using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Level.Traps;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class BlossomEntAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Wander.Subcomponent(true)]
	private Wander _wander;

	[Subcomponent(typeof(WanderForDuration))]
	[SerializeField]
	private WanderForDuration _wanderAfterAttack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _attack;

	[SerializeField]
	private BlossomFog _fogPrefab;

	[SerializeField]
	private Transform _fogPoint;

	private bool _attacking;

	private void Awake()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Object.Instantiate<BlossomFog>(_fogPrefab, _fogPoint.position, Quaternion.identity, _fogPoint);
		base.behaviours = new List<Behaviour> { _wander, _wanderAfterAttack, _attack };
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
		((MonoBehaviour)this).StartCoroutine(StartAttackLoop());
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null) && !_attacking)
			{
				yield return _wanderAfterAttack.CRun(this);
			}
		}
	}

	private IEnumerator StartAttackLoop()
	{
		while (!base.dead)
		{
			if (_attack.CanUse())
			{
				_attacking = true;
				StopAllBehaviour();
				yield return _attack.CRun(this);
				_attacking = false;
			}
			else
			{
				yield return null;
			}
		}
	}
}
