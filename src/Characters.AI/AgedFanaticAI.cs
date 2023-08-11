using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class AgedFanaticAI : AIController
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
	[Subcomponent(typeof(CircularProjectileAttack))]
	private CircularProjectileAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(KeepDistance))]
	private KeepDistance _keepDistance;

	[SerializeField]
	[Subcomponent(typeof(Sacrifice))]
	private Sacrifice _sacrifice;

	[Space]
	[SerializeField]
	[Header("Tools")]
	private Collider2D _attackTrigger;

	[SerializeField]
	private Collider2D _keepDistanceTrigger;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _attack, _keepDistance, _sacrifice };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CProcess());
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
			if (_sacrifice.result.Equals(Behaviour.Result.Doing))
			{
				yield return null;
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_keepDistanceTrigger) != (Object)null)
			{
				yield return _keepDistance.CRun(this);
			}
			if (_sacrifice.result.Equals(Behaviour.Result.Doing))
			{
				yield return null;
				continue;
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
