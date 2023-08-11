using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Abilities;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class StrangeSubjectAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Wander.Subcomponent(true)]
	private Wander _wander;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private ActionAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(Confusing))]
	private Confusing _confusing;

	[Chase.Subcomponent(true)]
	[SerializeField]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _wanderSpeedAbilityComponent;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _chaseSpeedAbilityComponent;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _confusingSpeedAbilityComponent;

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		character.ability.Add(_wanderSpeedAbilityComponent.ability);
		yield return _wander.CRun(this);
		character.ability.Remove(_wanderSpeedAbilityComponent.ability);
		yield return _idle.CRun(this);
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		while (!base.dead)
		{
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
				continue;
			}
			if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_attackCollider)))
			{
				yield return CAttack();
				continue;
			}
			character.ability.Add(_chaseSpeedAbilityComponent.ability);
			yield return _chase.CRun(this);
			character.ability.Remove(_chaseSpeedAbilityComponent.ability);
			if (_chase.result == Behaviour.Result.Success)
			{
				yield return CAttack();
			}
		}
	}

	private IEnumerator CAttack()
	{
		yield return _attack.CRun(this);
		character.ability.Add(_confusingSpeedAbilityComponent.ability);
		yield return _confusing.CRun(this);
		character.ability.Remove(_confusingSpeedAbilityComponent.ability);
	}
}
