using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class DarkOgre : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[Subcomponent(typeof(ChaseAndAttack))]
	[SerializeField]
	private ChaseAndAttack _chaseAndAttack;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chaseAndAttack };
	}

	public void StartCombat()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		while (!base.dead)
		{
			yield return _wander.CRun(this);
			yield return _chaseAndAttack.CRun(this);
		}
	}
}
