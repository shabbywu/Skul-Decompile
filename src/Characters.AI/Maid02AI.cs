using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class Maid02AI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[Subcomponent(typeof(Confusing))]
	[SerializeField]
	private Confusing _confusing;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _jumpAttack;

	[SerializeField]
	private Collider2D _attackCollider;

	public void Awake()
	{
		character.health.onTookDamage += Health_onTookDamage;
	}

	private void Health_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		_jumpAttack.StopPropagation();
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
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_attackCollider) != (Object)null)
			{
				yield return _jumpAttack.CRun(this);
				yield return _confusing.CRun(this);
				continue;
			}
			yield return _chase.CRun(this);
			if (_chase.result == Behaviour.Result.Success)
			{
				yield return _jumpAttack.CRun(this);
				yield return _confusing.CRun(this);
			}
		}
	}
}
