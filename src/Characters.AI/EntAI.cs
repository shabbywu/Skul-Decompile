using System.Collections;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class EntAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Wander.Subcomponent(true)]
	[SerializeField]
	private Wander _wander;

	[Subcomponent(typeof(ChaseAndAttack))]
	[SerializeField]
	private ChaseAndAttack _chaseAndAttack;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	protected override void OnEnable()
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
			yield return _idle.CRun(this);
			yield return _chaseAndAttack.CRun(this);
		}
	}
}
