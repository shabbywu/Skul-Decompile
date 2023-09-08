using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Abilities;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class LooseSubject : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
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

	[SerializeField]
	[Chase.Subcomponent(true)]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	private Collider2D _dontChseCollider;

	[SerializeField]
	private Action _landing;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _wanderSpeedAbilityComponent;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _chaseSpeedAbilityComponent;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _confusingSpeedAbilityComponent;

	[SerializeField]
	private Action _groggy;

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		((Behaviour)character.collider).enabled = true;
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
			if ((Object)(object)base.target == (Object)null || ((Object)(object)_groggy != (Object)null && _groggy.running))
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
		_landing.TryStart();
		while (_landing.running)
		{
			yield return null;
		}
		character.ability.Add(_confusingSpeedAbilityComponent.ability);
		yield return _confusing.CRun(this);
		character.ability.Remove(_confusingSpeedAbilityComponent.ability);
	}
}
