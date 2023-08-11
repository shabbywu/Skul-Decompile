using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Abilities;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class EssenceRecruitAI : AIController
{
	[SerializeField]
	private Collider2D _attckTrigger;

	[Subcomponent(typeof(CheckWithinSightContinuous))]
	[SerializeField]
	private CheckWithinSightContinuous _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[Subcomponent(typeof(MoveToTarget))]
	[SerializeField]
	private MoveToTarget _moveToTarget;

	[SerializeField]
	[Subcomponent(typeof(ChaseTeleport))]
	private ChaseTeleport _chaseTeleport;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private Attack _found;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private Attack _attack;

	[Space]
	[AbilityAttacher.Subcomponent]
	[SerializeField]
	private AbilityAttacher _abilityAttacher;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _moveToTarget, _chaseTeleport, _attack };
		_abilityAttacher.Initialize(character);
		_abilityAttacher.StartAttach();
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
		yield return _found.CRun(this);
		_abilityAttacher.StopAttach();
		while (!base.dead)
		{
			if ((Object)(object)FindClosestPlayerBody(_attckTrigger) == (Object)null)
			{
				yield return _moveToTarget.CRun(this);
				if (_moveToTarget.result == Behaviour.Result.Fail)
				{
					yield return _chaseTeleport.CRun(this);
				}
			}
			yield return _attack.CRun(this);
		}
	}
}
